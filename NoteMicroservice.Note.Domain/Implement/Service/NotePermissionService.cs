using NoteMicroservice.Note.Domain.Abstract.Repository;
using NoteMicroservice.Note.Domain.Abstract.Service;
using NoteMicroservice.Note.Domain.Entity;

namespace NoteMicroservice.Note.Domain.Implement.Service;

public class NotePermissionService : INotePermissionService
{
    private readonly IPermissionRepository _permissionRepository;

    public NotePermissionService(IPermissionRepository permissionRepository)
    {
        _permissionRepository = permissionRepository;
    }

    public async Task AssignNotePermissionAsync(string requestingUserId, List<string> groupIds, string noteContentId, PrincipalType principalType, string principalIdToAssign, Permissions permissionTypeToAssign, AccessLevel accessLevelToAssign)
    {
        if (!await _permissionRepository.HasPermissionAsync(requestingUserId, groupIds, noteContentId, Permissions.ManagePermissions))
        {
            throw new UnauthorizedAccessException($"User {requestingUserId} does not have permission to manage permissions for note {noteContentId}.");
        }

        // **Bước 2: Thực hiện logic gán/cập nhật quyền**
        // Tìm xem đã có bản ghi quyền này chưa
        var existingPermission = await _permissionRepository.GetPermissionAsync(noteContentId, principalType, principalIdToAssign, permissionTypeToAssign);

        if (existingPermission != null)
        {
            // Nếu có, cập nhật mức độ truy cập
            existingPermission.AccessLevel = accessLevelToAssign;
            // Có thể cập nhật trường UpdatedByUserId và UpdatedTimeUtc
            await _permissionRepository.UpdatePermissionAsync(existingPermission);
        }
        else
        {
            // Nếu chưa có, tạo bản ghi mới
            var newPermission = new NoteContentPermission
            {
                NoteId = noteContentId,
                PrincipalType = principalType, // Lưu giá trị int của Enum
                PrincipalId = principalIdToAssign,
                Permission = permissionTypeToAssign, // Lưu giá trị int của Enum
                AccessLevel = accessLevelToAssign, // Lưu giá trị int của Enum
            };
            await _permissionRepository.AddPermissionAsync(newPermission);
        }
    }

     public async Task RevokeNotePermissionAsync(string requestingUserId, List<string> groupIds, string noteContentId, PrincipalType principalType, string principalIdToRevoke, Permissions permissionTypeToRevoke)
     {
         if (!await _permissionRepository.HasPermissionAsync(requestingUserId, groupIds, noteContentId, Permissions.ManagePermissions))
         {
             throw new UnauthorizedAccessException($"User {requestingUserId} does not have permission to manage permissions for note {noteContentId}.");
         }
      
         var permissionToRemove = await _permissionRepository.GetPermissionAsync(noteContentId, principalType, principalIdToRevoke, permissionTypeToRevoke);

         if (permissionToRemove != null)
         {
             await _permissionRepository.DeletePermissionAsync(permissionToRemove);
              // **Bước 3: (Tùy chọn) Clear cache liên quan**
         }
         // Nếu không tìm thấy bản ghi, không làm gì cả hoặc log cảnh báo
     }
}