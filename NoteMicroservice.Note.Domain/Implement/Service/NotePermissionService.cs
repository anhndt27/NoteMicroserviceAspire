using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using NoteMicroservice.Note.Domain.Abstract.Repository;
using NoteMicroservice.Note.Domain.Abstract.Service;
using NoteMicroservice.Note.Domain.Context;
using NoteMicroservice.Note.Domain.Dtos;
using NoteMicroservice.Note.Domain.Dtos.BaseDtos;
using NoteMicroservice.Note.Domain.Entity;
using NoteMicroservice.Note.Domain.Resources;

namespace NoteMicroservice.Note.Domain.Implement.Service;

public class NotePermissionService : INotePermissionService
{
    private readonly NoteDbContext _context;
    private readonly IStringLocalizer<CommonTitles> _title;
    private readonly IStringLocalizer<CommonMessages> _message;
    public NotePermissionService(NoteDbContext context, IStringLocalizer<CommonMessages> message, IStringLocalizer<CommonTitles> title)
    {
        _context = context;
        _message = message;
        _title = title;
    }

    public async Task<ResponseMessageDto<PermissionResponseDto>> GetNotePermissionsAsync(string requestingUserId, string noteContentId)
    {
        var notePermissionResponse = await _context.NoteContentPermissions
            .Where(p => p.NoteId == noteContentId)
            .ToListAsync();

        if (!notePermissionResponse.Any())
        {
            return new ResponseMessageDto<PermissionResponseDto>(MessageType.Success)
            {
                Dto = null
            };
        }
        
        var notePermission = notePermissionResponse.ToPermissionResponseDto();

        return new ResponseMessageDto<PermissionResponseDto>(MessageType.Success)
        {
            Dto = notePermission
        };
    }

    public async Task<ResponseMessage> UpdateNotePermissionAsync(string requestingUserId, PermissionRequestDto request)
    {
        var existingPermissions = await _context.NoteContentPermissions
            .Where(p => p.NoteId == request.NoteId)
            .ToListAsync();

        _context.NoteContentPermissions.RemoveRange(existingPermissions);

        if (request.Emails != null)
        {
            foreach (var email in request.Emails)
            {
                _context.NoteContentPermissions.Add(new NoteContentPermission
                {
                    NoteId = request.NoteId,
                    Email = email,
                });
            }
        }

        if (request.GroupIds != null)
        {
            foreach (var groupId in request.GroupIds)
            {
                _context.NoteContentPermissions.Add(new NoteContentPermission
                {
                    NoteId = request.NoteId,
                    GroupId = groupId,
                });
            }
        }

        var changeCount = await _context.SaveChangesAsync();
        if (changeCount > 0)
        {
            return ResponseMessage.UpdatedSuccess(_title, _message);
        }
        return ResponseMessage.NoRecordUpdated(_title, _message);
    }
}