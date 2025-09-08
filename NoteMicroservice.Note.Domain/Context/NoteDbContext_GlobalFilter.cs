using Microsoft.EntityFrameworkCore;
using NoteMicroservice.Note.Domain.Entity;

namespace NoteMicroservice.Note.Domain.Context
{
    public partial class NoteDbContext
    {
        private void ConfigGlobalFilter(ModelBuilder builder)
        {
            /*-------------------------------------------------------AUTHORIZATION-----------------------------------------------------*/
            builder.Entity<NoteContent>().HasQueryFilter(e => !e.IsDeleted);
            builder.Entity<NoteContentPermission>().HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
