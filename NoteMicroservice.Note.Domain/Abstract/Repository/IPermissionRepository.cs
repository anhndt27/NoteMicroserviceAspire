using NoteMicroservice.Note.Domain.Entity;

namespace NoteMicroservice.Note.Domain.Abstract.Repository;

public interface IPermissionRepository
{
    Task<List<string>> GetAccessibleNoteIdsForViewAsync(string userId, List<string> userGroupIds);

    Task<bool> HasPermissionAsync(string userId, List<string> userGroupIds, string noteContentId, Permissions permissionType);

    Task AddPermissionAsync(NoteContentPermission permission);
    Task UpdatePermissionAsync(NoteContentPermission permission);
    Task DeletePermissionAsync(NoteContentPermission permission);
    Task<NoteContentPermission> GetPermissionByIdAsync(int permissionId);
    Task<NoteContentPermission> GetPermissionAsync(string noteContentId, PrincipalType principalType, string principalId, Permissions permissionType);
}