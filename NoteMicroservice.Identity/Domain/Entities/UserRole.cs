using System.ComponentModel.DataAnnotations;

namespace NoteMicroservice.Identity.Domain.Entities;

public class UserRole : BaseModel, ITimeTrackableModel, IUserTrackableModels, ISoftDeletedModel
{
    [Required]
    public string UserId { get; set; }
    public User User { get; set; }

    [Required]
    public string RoleId { get; set; }
    public Role Role { get; set; }

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