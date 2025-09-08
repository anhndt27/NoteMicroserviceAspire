using Microsoft.Extensions.Localization;
using NoteMicroservice.Note.Domain.Resources;

namespace NoteMicroservice.Note.Domain.Dtos.BaseDtos;

public class EditRequestDto
{
    public string Id { get; set; }

    public virtual DtoValidationResult Validate(IStringLocalizer<CommonTitles> _commonTitles, IStringLocalizer<CommonMessages> _commonMessages)
    {
        if (string.IsNullOrEmpty(Id?.Trim()))
        {
            return DtoValidationResult.Invalid(_commonTitles["InvalidInput"], _commonMessages["InvalidInput"]);
        }

        return DtoValidationResult.Valid;
    }
}