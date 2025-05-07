namespace NoteMicroservice.Note.Domain.Entity;

public class NoteContent : BaseModel, ISoftDeletedModel, ITimeTrackableModel, IUserTrackableModels
{
    public string Title { get; set; } = default!;
    public string NoteString { get; set; } = default!;
    public string UserId { get; set; } = default!;
    public string GroupId { get; set; }
    public DateTimeOffset DateTime { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedTimeUtc { get; set; }
    public string DeletedByUserId { get; set; }
    public DateTimeOffset CreatedTimeUtc { get; set; }
    public DateTimeOffset? UpdatedTimeUtc { get; set; }
    public string CreatedByUserId { get; set; }
    public string UpdatedByUserId { get; set; }
}