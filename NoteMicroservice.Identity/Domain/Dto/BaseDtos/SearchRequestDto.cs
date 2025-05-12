using Microsoft.Extensions.Localization;
using NoteMicroservice.Identity.Domain.Constants;
using NoteMicroservice.Identity.Domain.Entities;
using NoteMicroservice.Identity.Domain.Extensions;
using NoteMicroservice.Identity.Domain.Resources;
using NoteMicroservice.Identity.Infrastructure;

namespace NoteMicroservice.Identity.Domain.Dto.BaseDtos
{
    public abstract class SearchRequestDto<TModel> where TModel : BaseModel
    {
        // PAGINATION
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        // SORTS
        public string OrderBy { get; set; }
        public bool OrderByDesc { get; set; }

        // SEARCH
        public string SearchText { get; set; }
        public List<string> SearchTexts { get; set; }

        public DateTimeOffset? CreatedTimeUtcFrom { get; set; }
        public DateTimeOffset? CreatedTimeUtcTo { get; set; }

        public DateTimeOffset? UpdatedTimeUtcFrom { get; set; }
        public DateTimeOffset? UpdatedTimeUtcTo { get; set; }

        private ILogger<SearchRequestDto<TModel>> _logger =>
            Program.Services.GetService<ILogger<SearchRequestDto<TModel>>>();

        public abstract List<string> OrderByValues { get; }

        public abstract bool TryCreateSingleQuery(ApplicationDbContext context, out IQueryable<TModel> query);

        protected List<string> BaseOrderValues => ["Id", "CreatedTimeUtc", "UpdatedTimeUtc", "CreatedByUser", "UpdatedByUser"];

        protected DtoValidationResult ValidateBaseProperties(IStringLocalizer<CommonTitles> _commonTitles, IStringLocalizer<CommonMessages> _commonMessage)
        {
            if (PageIndex < 1)
            {
                return DtoValidationResult.Invalid(_commonTitles["InvalidPageIndex"], _commonMessage["InvalidPageIndex"]);
            }

            if (!AppConst.ValidsPageSizes.Contains(PageSize))
            {
                return DtoValidationResult.Invalid(_commonTitles["InvalidPageIndex"],_commonMessage["InvalidPageSize"]);
            }

            if (!string.IsNullOrEmpty(OrderBy) && !OrderByValues.Contains(OrderBy))
            {
                return DtoValidationResult.Invalid(_commonTitles["InvalidPageIndex"], _commonMessage["InValidOrderByValue"]);
            }

            return DtoValidationResult.Valid;
        }

        protected IQueryable<TModel> CreateBaseSearchQuery(IQueryable<TModel> query)
        {
            try
            {
                if (typeof(ITimeTrackableModel).IsAssignableFrom(typeof(TModel)))
                {
                    if (CreatedTimeUtcFrom != null)
                    {
                        query = query.Where(e => ((ITimeTrackableModel)e).CreatedTimeUtc >= CreatedTimeUtcFrom);
                    }
                    if (CreatedTimeUtcTo != null)
                    {
                        query = query.Where(e => ((ITimeTrackableModel)e).CreatedTimeUtc <= CreatedTimeUtcTo);
                    }
                    if (UpdatedTimeUtcFrom != null)
                    {
                        query = query.Where(e => ((ITimeTrackableModel)e).UpdatedTimeUtc >= UpdatedTimeUtcFrom);
                    }
                    if (UpdatedTimeUtcTo != null)
                    {
                        query = query.Where(e => ((ITimeTrackableModel)e).UpdatedTimeUtc <= UpdatedTimeUtcTo);
                    }
                }

                return query;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception when create base search query");
                throw;
            }
        }

        protected IQueryable<TModel> CreateBaseSortQuery(IQueryable<TModel> query)
        {
            try
            {
                if (string.IsNullOrEmpty(OrderBy?.Trim()))
                {
                    OrderBy = "CreatedTimeUtc";
                    OrderByDesc = true;
                }

                switch (OrderBy)
                {
                    case "Id":
                        query = query.OrderBy(OrderByDesc, e => e.Id);
                        break;
                    case "CreatedTimeUtc":
                        if (typeof(ITimeTrackableModel).IsAssignableFrom(typeof(TModel)))
                        {
                            query = query.OrderBy(OrderByDesc, e => ((ITimeTrackableModel)e).CreatedTimeUtc);
                        }
                        break;
                    case "CreatedByUser":
                        if (typeof(IUserTrackableModels).IsAssignableFrom(typeof(TModel)))
                        {
                            query = query.OrderBy(OrderByDesc, e => ((IUserTrackableModels)e).CreatedByUser.UserName);
                        }
                        break;
                    case "UpdatedTimeUtc":
                        if (typeof(ITimeTrackableModel).IsAssignableFrom(typeof(TModel)))
                        {
                            query = query.OrderBy(OrderByDesc, e => ((ITimeTrackableModel)e).UpdatedTimeUtc);
                        }
                        break;
                    case "UpdatedByUser":
                        if (typeof(IUserTrackableModels).IsAssignableFrom(typeof(TModel)))
                        {
                            query = query.OrderBy(OrderByDesc, e => ((IUserTrackableModels)e).UpdatedByUser.UserName);
                        }
                        break;
                }

                return query;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception when create base sort query");
                throw;
            }
        }
    }
}
