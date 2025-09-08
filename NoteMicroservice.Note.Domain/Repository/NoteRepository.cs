using Microsoft.EntityFrameworkCore;
using NoteMicroservice.Note.Domain.Abstract.Repository;
using NoteMicroservice.Note.Domain.Dto;
using NoteMicroservice.Note.Domain.Entity;
using NoteDbContext = NoteMicroservice.Note.Domain.Context.NoteDbContext;

namespace NoteMicroservice.Note.Domain.Repository
{
	public class NoteRepository : INoteRepository
	{
		private readonly NoteDbContext _context;

		public NoteRepository(NoteDbContext context)
		{
			_context = context;
		}

		
	}
}
