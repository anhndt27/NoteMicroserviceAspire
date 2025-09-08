using Microsoft.EntityFrameworkCore;
using NoteMicroservice.Note.Domain.Entity;

namespace NoteMicroservice.Note.Infrastructure.Context;

public partial class NoteDbContext : DbContext
{
    public NoteDbContext(DbContextOptions<NoteDbContext> options)
        : base(options)
    {
    }

    public string OperatingUserId { get; private set; }
    public DbSet<NoteContent> NoteContents { get; set; }
    public DbSet<NoteContentPermission> NoteContentPermissions { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        ConfigDefaultValue(builder);
        ConfigGlobalFilter(builder);
    }

    public void SetOperatingUser(string userId)
    {
        OperatingUserId = userId;
    }

    private void ConfigDefaultValue(ModelBuilder builder)
    {
    }

    public override int SaveChanges()
    {
        HandleChangedEntries();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        HandleChangedEntries();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void HandleChangedEntries()
    {
        var AddedEntries = ChangeTracker.Entries().Where(e => e.State == EntityState.Added);
        foreach (var entry in AddedEntries)
        {
            if (entry.Entity is ITimeTrackableModel)
            {
                ((ITimeTrackableModel)entry.Entity).CreatedTimeUtc = DateTime.UtcNow;
                ((ITimeTrackableModel)entry.Entity).UpdatedTimeUtc = DateTime.UtcNow;
            }

            if (entry.Entity is IUserTrackableModels)
            {
                ((IUserTrackableModels)entry.Entity).CreatedByUserId = OperatingUserId;
                ((IUserTrackableModels)entry.Entity).UpdatedByUserId = OperatingUserId;
            }
        }

        var updatedEntries = ChangeTracker.Entries()
            .Where(e => e.Entity is BaseModel && (e.State == EntityState.Modified));
        foreach (var entry in updatedEntries)
        {
            if (entry.Entity is ITimeTrackableModel)
            {
                ((ITimeTrackableModel)entry.Entity).UpdatedTimeUtc = DateTime.UtcNow;
            }

            if (entry.Entity is IUserTrackableModels)
            {
                ((IUserTrackableModels)entry.Entity).UpdatedByUserId = OperatingUserId;
            }
        }

        var deletedEntries = ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted);
        foreach (var entry in deletedEntries)
        {
            if (entry.Entity is ISoftDeletedModel)
            {
                entry.State = EntityState.Modified;
                ((ISoftDeletedModel)entry.Entity).DeletedTimeUtc = DateTime.UtcNow;
                ((ISoftDeletedModel)entry.Entity).IsDeleted = true;
                ((ISoftDeletedModel)entry.Entity).DeletedByUserId = OperatingUserId;
            }
        }
    }
}