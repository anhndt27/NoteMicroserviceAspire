namespace NoteMicroservice.Note.Domain.Entity;

public class NoteContentPermission  : BaseModel, IUserTrackableModels, ITimeTrackableModel, ISoftDeletedModel
{
    public string NoteId { get; set; }
    public NoteContent Note { get; set; }

    public string Email { get; set; }
    public string GroupId { get; set; }
    public string CreatedByUserId { get; set; }
    public string UpdatedByUserId { get; set; }
    public DateTimeOffset CreatedTimeUtc { get; set; }
    public DateTimeOffset? UpdatedTimeUtc { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedTimeUtc { get; set; }
    public string DeletedByUserId { get; set; }
}