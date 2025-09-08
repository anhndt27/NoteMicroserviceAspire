namespace NoteMicroservice.Note.Domain.Entity;

public class NoteContentPermission  : BaseModel, IUserTrackableModels, ITimeTrackableModel, ISoftDeletedModel
{
    public string NoteId { get; set; }
    public NoteContent Note { get; set; }

    public string PrincipalId { get; set; }
    public Permissions Permission { get; set; }
    public PrincipalType PrincipalType { get; set; }
    public AccessLevel AccessLevel { get; set; }
    
    public string CreatedByUserId { get; set; }
    public string UpdatedByUserId { get; set; }
    public DateTimeOffset CreatedTimeUtc { get; set; }
    public DateTimeOffset? UpdatedTimeUtc { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedTimeUtc { get; set; }
    public string DeletedByUserId { get; set; }
}

public enum Permissions
{
    List,
    View,
    Edit,
    Delete,
    ManagePermissions
}

public enum PrincipalType
{
    User,
    Group
}

public enum AccessLevel
{
    Allow,
    Deny
}
