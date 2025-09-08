using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace NoteMicroservice.Identity.Domain.Entities
{
    public class User : BaseModel, ISoftDeletedModel, ITimeTrackableModel, IUserTrackableModels
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        
        public List<UserRole> UserRoles { get; set; }
        public List<UserGroups> UserGroups { get; set; }
        
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedTimeUtc { get; set; }
        public string DeletedByUserId { get; set; }
        public User DeletedByUser { get; set; }
        public DateTimeOffset CreatedTimeUtc { get; set; }
        public DateTimeOffset? UpdatedTimeUtc { get; set; }
        public string CreatedByUserId { get; set; }
        public User CreatedByUser { get; set; }
        public string UpdatedByUserId { get; set; }
        public User UpdatedByUser { get; set; }
    }
}
