namespace NoteMicroservice.Note.Domain.Dtos.BaseDtos;

public class DtoValidationResult
{
    public bool IsValid { get; set; }
    public string ErrorTitles { get; set; }
    public string ErrorMessage { get; set; }

    public DtoValidationResult(bool isValid,string errorTitle = null, string errorMessage = null)
    {
        IsValid = isValid;
        ErrorTitles = errorMessage;
        ErrorMessage = errorMessage;
    }

    public static DtoValidationResult Valid =>
        new DtoValidationResult(true);
    public static DtoValidationResult Invalid(string errorTitles, string errorMessage) =>
        new DtoValidationResult(false, errorMessage);
}