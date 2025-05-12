using Microsoft.EntityFrameworkCore;
using NoteMicroservice.Identity.Domain.Entities;

namespace NoteMicroservice.Identity.Infrastructure
{
    public partial class ApplicationDbContext
    {
        private void ConfigGlobalFilter(ModelBuilder builder)
        {
            /*-------------------------------------------------------AUTHORIZATION-----------------------------------------------------*/
            builder.Entity<User>().HasQueryFilter(e => !e.IsDeleted);
            builder.Entity<Role>().HasQueryFilter(e => !e.IsDeleted);
            builder.Entity<UserRole>().HasQueryFilter(e => !e.IsDeleted);
            builder.Entity<RolePermission>().HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
