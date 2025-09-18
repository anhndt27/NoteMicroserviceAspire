using NoteMicroservice.Note.Domain.Dtos.BaseDtos;
using NoteMicroservice.Note.Domain.Entity;

namespace NoteMicroservice.Note.Domain.Dtos;

public class PermissionRequestDto
{
    public string NoteId { get; set; }
    public List<string> Emails { get; set; }
    public List<string> GroupIds { get; set; }
}

public class PermissionResponseDto
{
    public string NoteId { get; set; }
    public List<string> Emails { get; set; }
    public List<string> GroupIds { get; set; }
}

public static class PermissionExtension
{
    public static PermissionResponseDto ToPermissionResponseDto(this List<NoteContentPermission> permission)
    {
        var dto = new PermissionResponseDto();
        
        if (permission == null || !permission.Any())
            return dto;
        
        dto.NoteId = permission.First().NoteId;
        dto.Emails = permission.Where(p => !string.IsNullOrEmpty(p.Email)).Select(p => p.Email).Distinct().ToList();
        dto.GroupIds = permission.Where(p => !string.IsNullOrEmpty(p.GroupId)).Select(p => p.GroupId).Distinct().ToList();
    
        return dto;
    }
}