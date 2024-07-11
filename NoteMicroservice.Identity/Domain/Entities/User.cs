using Microsoft.AspNetCore.Identity;

namespace NoteMicroservice.Identity.Domain.Entities
{
    public class User : IdentityUser
    {
        public Group Group { get; set; }
        public int? GroupId { get; set; }
    }
}
