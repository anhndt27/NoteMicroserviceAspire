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

    public async Task<List<string>> GetAccessibleNoteIdsForViewAsync(string userId, List<string> userGroupIds)
    {
        var allowedNoteIdsQuery = _context.NoteContentPermissions
            .Where(ncp => ncp.AccessLevel == AccessLevel.Allow &&
                          ncp.Permission == Permissions.View && // Sử dụng Enum PermissionType
                          ((ncp.PrincipalType == PrincipalType.User &&
                            ncp.PrincipalId == userId) || // Sử dụng Enum PrincipalType
                           (ncp.PrincipalType == PrincipalType.Group && userGroupIds.Contains(ncp.PrincipalId))))
            .Select(ncp => ncp.NoteId); // Sử dụng NoteContentId

        var deniedNoteIdsQuery = _context.NoteContentPermissions
            .Where(ncp => ncp.AccessLevel == AccessLevel.Deny && // Sử dụng Enum AccessLevel
                          ncp.Permission == Permissions.View && // Sử dụng Enum PermissionType
                          ((ncp.PrincipalType == PrincipalType.User &&
                            ncp.PrincipalId == userId) || // Sử dụng Enum PrincipalType
                           (ncp.PrincipalType == PrincipalType.Group && userGroupIds.Contains(ncp.PrincipalId))))
            .Select(ncp => ncp.NoteId); // Sử dụng NoteContentId

        var allowedNoteIds = await allowedNoteIdsQuery.ToListAsync();
        var deniedNoteIds = await deniedNoteIdsQuery.ToListAsync();

        var finalAccessibleNoteIds = allowedNoteIds.Except(deniedNoteIds).ToList();
        return finalAccessibleNoteIds;
    }

    // Đã sửa lại tham số permissionType là Enum PermissionType
    public async Task<bool> HasPermissionAsync(string userId, List<string> userGroupIds, string noteContentId,
        Permissions permissionType)
    {
        var directUserDeny = await _context.NoteContentPermissions
            .AnyAsync(ncp => ncp.NoteId == noteContentId && // Sử dụng NoteContentId
                             ncp.PrincipalType == PrincipalType.User && // Sử dụng Enum
                             ncp.PrincipalId == userId &&
                             ncp.Permission == permissionType && // Sử dụng Enum
                             ncp.AccessLevel == AccessLevel.Deny); // Sử dụng Enum

        if (directUserDeny)
        {
            return false; // Có quyền Deny trực tiếp cho User -> Từ chối
        }

        // 2. Kiểm tra quyền DENY gán cho bất kỳ Group nào của User
        var groupDeny = await _context.NoteContentPermissions
            .AnyAsync(ncp => ncp.NoteId == noteContentId && // Sử dụng NoteContentId
                             ncp.PrincipalType == PrincipalType.Group && // Sử dụng Enum
                             userGroupIds.Contains(ncp.PrincipalId) &&
                             ncp.Permission == permissionType && // Sử dụng Enum
                             ncp.AccessLevel == AccessLevel.Deny); // Sử dụng Enum

        if (groupDeny)
        {
            return false; // Có quyền Deny cho Group -> Từ chối
        }

        // 3. Kiểm tra quyền ALLOW gán trực tiếp cho User
        var directUserAllow = await _context.NoteContentPermissions
            .AnyAsync(ncp => ncp.NoteId == noteContentId && // Sử dụng NoteContentId
                             ncp.PrincipalType == PrincipalType.User && // Sử dụng Enum
                             ncp.PrincipalId == userId &&
                             ncp.Permission == permissionType && // Sử dụng Enum
                             ncp.AccessLevel == AccessLevel.Allow); // Sử dụng Enum

        if (directUserAllow)
        {
            return true; // Có quyền Allow trực tiếp cho User -> Cho phép
        }

        // 4. Kiểm tra quyền ALLOW gán cho bất kỳ Group nào của User
        var groupAllow = await _context.NoteContentPermissions
            .AnyAsync(ncp => ncp.NoteId == noteContentId && // Sử dụng NoteContentId
                             ncp.PrincipalType == PrincipalType.Group && // Sử dụng Enum
                             userGroupIds.Contains(ncp.PrincipalId) &&
                             ncp.Permission == permissionType && // Sử dụng Enum
                             ncp.AccessLevel == AccessLevel.Allow); // Sử dụng Enum

        if (groupAllow)
        {
            return true; // Có quyền Allow cho Group -> Cho phép
        }

        // Nếu không có quyền Deny hoặc Allow cụ thể nào được tìm thấy, áp dụng chính sách mặc định (Deny)
        return false;
    }

    // Implement CRUD methods
    public async Task AddPermissionAsync(NoteContentPermission permission)
    {
        _context.NoteContentPermissions.Add(permission);
        await _context.SaveChangesAsync();
    }

    public async Task UpdatePermissionAsync(NoteContentPermission permission)
    {
        _context.NoteContentPermissions.Update(permission);
        await _context.SaveChangesAsync();
    }

    public async Task DeletePermissionAsync(NoteContentPermission permission)
    {
        _context.NoteContentPermissions.Remove(permission);
        await _context.SaveChangesAsync();
    }

    public async Task<NoteContentPermission> GetPermissionByIdAsync(int permissionId)
    {
        return await _context.NoteContentPermissions.FindAsync(permissionId);
    }

    // Đã sửa lại tham số permissionType là Enum PermissionType
    public async Task<NoteContentPermission> GetPermissionAsync(string noteContentId, PrincipalType principalType,
        string principalId, Permissions permissionType)
    {
        return await _context.NoteContentPermissions
            .FirstOrDefaultAsync(ncp => ncp.NoteId == noteContentId &&
                                        ncp.PrincipalType == principalType && // Sử dụng Enum
                                        ncp.PrincipalId == principalId &&
                                        ncp.Permission == permissionType); // Sử dụng Enum
    }
}