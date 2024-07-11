using Microsoft.EntityFrameworkCore;
using NoteMicroservice.Note.Domain.Entity;

namespace NoteMicroservice.Note.Infrastructure.Context;

public class NoteDbContext : DbContext
{
    public NoteDbContext(DbContextOptions<NoteDbContext> options)
       : base(options)
    {
    }

    public DbSet<NoteContent> NoteContents { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }

}