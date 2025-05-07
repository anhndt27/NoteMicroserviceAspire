namespace NoteMicroservice.Note.Domain.Entity;

public interface ITimeTrackableModel
{
    public DateTimeOffset CreatedTimeUtc { get; set; }
    public DateTimeOffset? UpdatedTimeUtc { get; set; }
}