using Microsoft.Extensions.Localization;
using NoteMicroservice.Identity.Domain.Resources;

namespace NoteMicroservice.Identity.Domain.Dto.BaseDtos;

public class DeleteRangeRequestDto
{
    public List<string> Ids { get; set; }

    public virtual DtoValidationResult Validate(IStringLocalizer<CommonTitles> _commonTitles, IStringLocalizer<CommonMessages> _commonMessages)
    {
        if (Ids == null || !Ids.Any())
        {
            return DtoValidationResult.Invalid(_commonTitles["InvalidInput"], _commonMessages["InvalidInput"]);
        }

        return DtoValidationResult.Valid;
    }
}