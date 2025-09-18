using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using NoteMicroservice.Identity.Domain.Dto.BaseDtos;
using NoteMicroservice.Identity.Domain.Entities;
using NoteMicroservice.Identity.Domain.Resources;
using NoteMicroservice.Identity.Infrastructure;

namespace NoteMicroservice.Identity.Domain.Dto
{
	public class GroupRequestDto
	{
        public string GroupName { get; set; }
    }
    
    public class GroupSearchRequestDto : SearchRequestDto<Group>
    {
        public override List<string> OrderByValues { get; }
        
        public DtoValidationResult Validate(IStringLocalizer<CommonTitles> commonTitles, IStringLocalizer<CommonMessages> commonMessages)
        {
            var baseValidateResult = base.ValidateBaseProperties(commonTitles, commonMessages);
            if (!baseValidateResult.IsValid)
            {
                return baseValidateResult;
            }

            return DtoValidationResult.Valid;
        }
        
        public override bool TryCreateSingleQuery(ApplicationDbContext context, out IQueryable<Group> query)
        {
            query = context.Groups.AsNoTracking()
                .Include(e => e.UserGroups)
                .AsQueryable();
            
            query = CreateBaseSortQuery(query);

            return true;
        }
    }

    public class ReactGroupDto
    {
        public List<string> UserIds { get; set; }
        public string GroupId { get; set; }
    }
}
