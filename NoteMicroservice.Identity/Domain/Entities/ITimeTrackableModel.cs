namespace NoteMicroservice.Identity.Domain.Entities;

public interface ITimeTrackableModel
{
    public DateTimeOffset CreatedTimeUtc { get; set; }
    public DateTimeOffset? UpdatedTimeUtc { get; set; }
}