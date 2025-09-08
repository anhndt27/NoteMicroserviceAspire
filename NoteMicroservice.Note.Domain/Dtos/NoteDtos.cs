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
    public string Content { get; set; }
}

public class NoteSearchDto : SearchRequestDto<NoteContent>
{
    public override List<string> OrderByValues  {
        get
        {
            var values = BaseOrderValues;
            values.AddRange([]);
            return values;
        }
    }
    
    public DtoValidationResult Validate(IStringLocalizer<CommonTitles> commonTitle, IStringLocalizer<CommonMessages> commonMessage)
    {
        var result = ValidateBaseProperties(commonTitle, commonMessage);
        if (!result.IsValid)
        {
            return result;
        }

        return result;
    }
    
    public IQueryable<NoteContent> CreateSearchQuery(NoteDbContext context)
    {
        try
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

            return query;
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    
    public override bool TryCreateSingleQuery(NoteDbContext context, out IQueryable<NoteContent> query)
    {
        try
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
        catch (Exception ex)
        {
            query = default;
            return false;
        }
    }
}