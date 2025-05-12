namespace NoteMicroservice.Identity.Domain.Entities;

public interface ISoftDeletedModel
{
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedTimeUtc { get; set; }

    public string DeletedByUserId { get; set; }
}
