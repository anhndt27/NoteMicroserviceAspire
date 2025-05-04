namespace NoteMicroservice.Note.Domain.Entity;

public class ITimeTrackableModel
{
    public DateTimeOffset CreatedTimeUtc { get; set; }
    public DateTimeOffset? UpdatedTimeUtc { get; set; }
}