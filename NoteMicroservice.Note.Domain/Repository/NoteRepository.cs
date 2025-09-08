using Microsoft.EntityFrameworkCore;
using NoteMicroservice.Note.Domain.Abstract.Repository;
using NoteMicroservice.Note.Domain.Entity;
using NoteMicroservice.Note.Domain.Dto;
using NoteMicroservice.Note.Infrastructure.Context;

namespace NoteMicroservice.Note.Infrastructure.Repository
{
	public class NoteRepository : INoteRepository
	{
		private readonly NoteDbContext _context;

		public NoteRepository(NoteDbContext context)
		{
			_context = context;
		}

		public async Task<List<NoteSimpleResponseDto>> GetListNotes(string userId, string groupId)
		{
			var notes = new List<NoteSimpleResponseDto>();

			if (groupId != null) 
			{
				notes = await _context.NoteContents
					.OrderByDescending(n => n.DateTime)
					.Select(n => new NoteSimpleResponseDto
					{
						Id = n.Id,
						Title = n.Title, 
						DateTime = n.DateTime,
						Category = "Group"
					})
					.ToListAsync();
			}
			else if (userId != null && groupId == null) 
			{
				notes = await _context.NoteContents
				.OrderByDescending(n => n.DateTime)
				.Select(n => new NoteSimpleResponseDto
				{
					Id = n.Id,
					Title = n.Title,
					DateTime = n.DateTime,
					Category = "Private"
				})
				.ToListAsync();
			}

			return notes;
		}

		public async Task<NoteListFilter> GetListNotesFilter(string userId, string groupId, string filter, string orderby)
		{
			var lts = new NoteListFilter();

			IQueryable<NoteContent> query = _context.NoteContents
				.Where(n => n.UserId == userId);

			if (groupId != null)
			{
				lts.Category = "Group";
				query = query.Where(n => n.GroupId == groupId);
			}
			else
			{
				lts.Category = "Private";
				query = query.Where(n => n.GroupId == null);
			}

			if(filter != null)
			{
				query = query.Where(n => n.Title.Contains(filter) || n.NoteString.Contains(filter));
			}

			if (orderby != null)
			{
				if (orderby.ToLower().EndsWith("desc"))
				{
					query = query.OrderByDescending(n => n.DateTime);
				}
				else
				{
					query = query.OrderBy(n => n.DateTime);
				}
			}
			else
			{
				query = query.OrderByDescending(n => n.DateTime);
			}

			lts.Data = await query
				.Select(n => new NoteDataItem
				{
					Id = n.Id,
					Title = n.Title,
					DateTime = n.DateTime,
					Category = lts.Category,
				})
				.ToListAsync();

			return lts;
		}
	}
}
