using System.Text.Json.Serialization;

namespace NoteMicroservice.Identity.Domain.Dto.BaseDtos;

public class PaginatedListDto<TDto>
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int ItemsCount { get; set; }
    public int PagesCount { get; set; }
    public List<TDto> Items { get; set; }

    public PaginatedListDto(int itemsCount, int pagesCount, int pageIndex, int pageSize)
    {
        Items = new List<TDto>();
        PageIndex = pageIndex;
        PageSize = pageSize;
        ItemsCount = itemsCount;
        PagesCount = pagesCount;
    }

    [JsonIgnore]
    public int ActualSkipCount => (PageIndex - 1) * PageSize;

    [JsonIgnore]
    public int ActualTakeCount => ItemsCount > PageIndex * PageSize ? PageSize : ItemsCount - ActualSkipCount;
}