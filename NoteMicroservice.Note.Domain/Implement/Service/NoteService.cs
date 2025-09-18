using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using NoteMicroservice.Note.Domain.Abstract.Repository;
using NoteMicroservice.Note.Domain.Abstract.Service;
using NoteMicroservice.Note.Domain.Context;
using NoteMicroservice.Note.Domain.Dtos;
using NoteMicroservice.Note.Domain.Dtos.BaseDtos;
using NoteMicroservice.Note.Domain.Entity;
using NoteMicroservice.Note.Domain.Extensions;
using NoteMicroservice.Note.Domain.Resources;

namespace NoteMicroservice.Note.Domain.Implement.Service;

public class NoteService : INoteService
{
    private readonly NoteDbContext _context;
    private readonly IPermissionRepository _permissionRepository;
    private readonly IStringLocalizer<CommonTitles> _title;
    private readonly IStringLocalizer<CommonMessages> _message;

    public NoteService(NoteDbContext context, IPermissionRepository permissionRepository,
        IStringLocalizer<CommonMessages> message, IStringLocalizer<CommonTitles> title)
    {
        _context = context;
        _permissionRepository = permissionRepository;
        _message = message;
        _title = title;
    }

    public async Task<ResponseMessage> CreateNote(string userId, string email, string groupId, NoteRequestDto request)
    {
        _context.SetOperatingUser(userId);

        var noteContent = new NoteContent
        {
            Title = request.Title,
            NoteString = request.NoteString,
            NoteContentPermissions = new List<NoteContentPermission>()
        };

        if (!string.IsNullOrEmpty(groupId))
        {
            noteContent.NoteContentPermissions.Add(new NoteContentPermission
            {
                GroupId = groupId,
            });
        }

        if (!string.IsNullOrEmpty(email))
        {
            noteContent.NoteContentPermissions.Add(new NoteContentPermission
            {
                Email = email,
            });
        }

        _context.NoteContents.Add(noteContent);

        var changeCount = await _context.SaveChangesAsync();
        if (changeCount > 0)
        {
            return ResponseMessage.AddedSuccess(_title, _message);
        }

        return ResponseMessage.AddedSuccess(_title, _message);
    }

    public async Task<ResponseMessageDto<NoteDtos>> GetNote(string id, string email,
        List<string> groupId)
    {
        var hasAccess = await _permissionRepository.HasAccessAsync(id, email, groupId);
        if (!hasAccess)
        {
            return new ResponseMessageDto<NoteDtos>(MessageType.Error, _title["Unauthorized"],
                _message["Unauthorized"]);
        }
        
        var note = await _context.NoteContents
            .Where(n => n.Id == id)
            .FirstOrDefaultAsync();

        if (note == null)
        {
            return new ResponseMessageDto<NoteDtos>(MessageType.Success)
            {
                Dto = null
            };
        }

        var noteDto = new NoteDtos
        {
            Title = note.Title,
            NoteString = note.NoteString,
        };

        return new ResponseMessageDto<NoteDtos>(MessageType.Success)
        {
            Dto = noteDto
        };
    }

    public async Task<ResponseMessage> UpdateNote(string id, string userId, string email, List<string> groupId,
        NoteRequestDto request)
    {
        var hasAccess = await _permissionRepository.HasAccessAsync(id, email, groupId);
        if (!hasAccess)
        {
            return ResponseMessage.Unauthorized(_title, _message);
        }
        
        _context.SetOperatingUser(userId);

        var note = await _context.NoteContents
            .Where(n => n.Id == id && !n.IsDeleted)
            .FirstOrDefaultAsync();

        if (note == null)
        {
            return ResponseMessage.DataNotFound(_title, _message);
        }

        note.Title = request.Title;
        note.NoteString = note.NoteString;

        var changeCount = await _context.SaveChangesAsync();
        if (changeCount > 0)
        {
            return ResponseMessage.UpdatedSuccess(_title, _message);
        }

        return ResponseMessage.NoRecordUpdated(_title, _message);
    }

    public async Task<ResponseMessage> DeleteNote(string id, string userId, string email, List<string> groupId)
    {
        var hasAccess = await _permissionRepository.HasAccessAsync(id, email, groupId);
        if (!hasAccess)
        {
            return ResponseMessage.Unauthorized(_title, _message);
        }
        
        _context.SetOperatingUser(userId);

        var note = await _context.NoteContents
            .Include(e => e.NoteContentPermissions)
            .Where(n => n.Id == id && !n.IsDeleted)
            .FirstOrDefaultAsync();

        if (note == null)
        {
            return ResponseMessage.DataNotFound(_title, _message);
        }

        _context.NoteContentPermissions.RemoveRange(note.NoteContentPermissions);
        _context.NoteContents.Remove(note);

        var changeCount = await _context.SaveChangesAsync();
        if (changeCount > 0)
        {
            return ResponseMessage.DeletedSuccess(_title, _message);
        }

        return ResponseMessage.NoRecordDeleted(_title, _message);
    }

    public async Task<ResponseMessageDto<PaginatedListDto<NoteSimpleResponseDto>>> Search(string email, List<string> groupIds, NoteSearchDto searchDto)
    {
        searchDto.Email = email;
        searchDto.GroupIds = groupIds;
        
        searchDto.TryCreateSingleQuery(_context, out IQueryable<NoteContent> query);
        var paginationDto = await query.CreatePaginationAsync(searchDto.PageIndex, searchDto.PageSize,
            e => e.ToNoteSimpleResponseDto());

        return new ResponseMessageDto<PaginatedListDto<NoteSimpleResponseDto>>(MessageType.Success)
        {
            Dto = paginationDto
        };
    }
}