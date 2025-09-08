using NoteMicroservice.Note.Domain.Entity;

namespace NoteMicroservice.Note.Domain.Abstract.Service;

public interface INotePermissionService
{
    Task AssignNotePermissionAsync(string requestingUserId, List<string> groupIds, string noteContentId, PrincipalType principalType, string principalIdToAssign, Permissions permissionTypeToAssign, AccessLevel accessLevelToAssign);

    Task RevokeNotePermissionAsync(string requestingUserId, List<string> groupIds, string noteContentId, PrincipalType principalType, string principalIdToRevoke, Permissions permissionTypeToRevoke);
}