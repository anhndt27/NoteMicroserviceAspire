namespace NoteMicroservice.Identity.Domain.Entities;

public class Role : BaseModel, ITimeTrackableModel, IUserTrackableModels, ISoftDeletedModel
{
    public string Name { get; set; }
    public string Description { get; set; }

    public List<RolePermission> RolePermissions { get; set; }
    public List<UserRole> UserRoles { get; set; }

    public DateTimeOffset CreatedTimeUtc { get; set; }
    public DateTimeOffset? UpdatedTimeUtc { get; set; }
    public string CreatedByUserId { get; set; }
    public User CreatedByUser { get; set; }
    public string UpdatedByUserId { get; set; }
    public User UpdatedByUser { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedTimeUtc { get; set; }
    public string DeletedByUserId { get; set; }
    public User DeletedByUser { get; set; }
}