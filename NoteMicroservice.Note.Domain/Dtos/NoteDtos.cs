using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using NoteMicroservice.Note.Domain.Dtos.BaseDtos;
using NoteMicroservice.Note.Domain.Entity;
using NoteMicroservice.Note.Domain.Resources;
using NoteDbContext = NoteMicroservice.Note.Domain.Context.NoteDbContext;

namespace NoteMicroservice.Note.Domain.Dtos;

public class NoteDtos : EntityDto
{
    public string Title { get; set; }
    public string NoteString { get; set; }
}

public class NoteRequestDto
{
    public string Title { get; set; }
    public string NoteString { get; set; }
}

public class NoteSimpleResponseDto : EntityDto
{
    public string Title { get; set; }
}

public class NoteSearchDto : SearchRequestDto<NoteContent>
{
    public string Email { get; set; }
    public List<string> GroupIds { get; set; }
    
    public override List<string> OrderByValues  {
        get
        {
            var values = BaseOrderValues;
            return values;
        }
    }
    
    public IQueryable<NoteContent> CreateSearchQuery(NoteDbContext context)
    {
        var query = context.NoteContents
            .Include(e => e.NoteContentPermissions)
            .AsNoTracking()
            .AsQueryable();

        query = CreateBaseSearchQuery(query);

        if (!string.IsNullOrEmpty(SearchText))
        {
            var lowerCaseSearchText = SearchText.ToLower().Trim();
            query = query.Where(e => e.NoteString.ToLower().Contains(lowerCaseSearchText) || e.Title.ToLower().Contains(lowerCaseSearchText));
        }
        
        if (SearchTexts != null && SearchTexts.Any())
        {
            query = query.Where(e => SearchTexts.Contains(e.Title));
        }

        if (string.IsNullOrEmpty(Email))
        {
            query = query.Where(e => e.NoteContentPermissions.Any(x => x.Email.Equals(Email)));
        }
        
        if (GroupIds != null && GroupIds.Any())
        {
            query = query.Where(e => e.NoteContentPermissions.Any(x => GroupIds.Contains(x.GroupId)));
        }

        return query;
    }
    
    public override bool TryCreateSingleQuery(NoteDbContext context, out IQueryable<NoteContent> query)
    {
        query = CreateSearchQuery(context);
            
        query = CreateBaseSortQuery(query);
        switch (OrderBy)
        {
            case "Title":
                break;
        }
        return true;
    }
}

public static class NoteExtensions
{
    public static NoteDtos ToNoteDto(this NoteContent entity)
    {
        var dto = new NoteDtos();
        if (entity == null)
        {
            return null;
        }

        dto.MapBaseProperties(entity);
    
        dto.Title = entity.Title;
        dto.NoteString = entity.NoteString;
        

        return dto;
    }
    
    public static NoteSimpleResponseDto ToNoteSimpleResponseDto(this NoteContent entity)
    {
        var dto = new NoteSimpleResponseDto();
        if (entity == null)
        {
            return null;
        }
        
        dto.MapBaseProperties(entity);
        dto.Title = entity.Title;

        return dto;
    }
}