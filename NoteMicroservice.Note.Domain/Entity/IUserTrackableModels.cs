namespace NoteMicroservice.Note.Domain.Entity;

public interface IUserTrackableModels
{
    public string CreatedByUserId { get; set; }
    public string UpdatedByUserId { get; set; }
}