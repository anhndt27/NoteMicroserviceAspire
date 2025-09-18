using Microsoft.EntityFrameworkCore;
using NoteMicroservice.Note.Domain.Abstract.Repository;
using NoteMicroservice.Note.Domain.Entity;
using NoteDbContext = NoteMicroservice.Note.Domain.Context.NoteDbContext;

namespace NoteMicroservice.Note.Domain.Repository;

public class PermissionRepository : IPermissionRepository
{
    private readonly NoteDbContext _context;

    public PermissionRepository(NoteDbContext context)
    {
        _context = context;
    }

    public async Task<bool> HasAccessAsync(string noteId, string email, List<string> groupIds)
    {
        var hasAccess = await _context.NoteContentPermissions
            .Where(p => p.NoteId == noteId && !p.IsDeleted)
            .AnyAsync(p => p.Email == email || groupIds.Contains(p.GroupId));

        return hasAccess;
    }
}