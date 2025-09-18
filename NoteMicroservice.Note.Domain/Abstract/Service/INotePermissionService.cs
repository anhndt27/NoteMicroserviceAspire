using NoteMicroservice.Note.Domain.Dtos;
using NoteMicroservice.Note.Domain.Dtos.BaseDtos;
using NoteMicroservice.Note.Domain.Entity;

namespace NoteMicroservice.Note.Domain.Abstract.Service;

public interface INotePermissionService
{
    Task<ResponseMessageDto<PermissionResponseDto>> GetNotePermissionsAsync(string requestingUserId, string noteContentId);
    
    Task<ResponseMessage> UpdateNotePermissionAsync(string requestingUserId, PermissionRequestDto request);
}