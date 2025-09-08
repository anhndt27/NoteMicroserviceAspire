using NoteMicroservice.Note.Domain.Abstract.Repository;
using NoteMicroservice.Note.Domain.Abstract.Service;
using NoteMicroservice.Note.Domain.Entity;
using NoteMicroservice.Note.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NoteMicroservice.Note.Domain.Dtos;
using NoteMicroservice.Note.Domain.Dtos.BaseDtos;
using NoteMicroservice.Note.Domain.Extensions;
using NoteMicroservice.Note.Domain.ViewModel;
using NoteDbContext = NoteMicroservice.Note.Domain.Context.NoteDbContext;

namespace NoteMicroservice.Note.Domain.Implement.Service
{
	public class NoteService : INoteService
	{
		private readonly INoteRepository _noteRepository;
		private readonly IRepository<NoteContent> _repository;
		private readonly NoteDbContext _context;
		public NoteService(IRepository<NoteContent> repository, INoteRepository noteRepository, NoteDbContext context)
		{
			_repository = repository;
			_noteRepository = noteRepository;
			_context = context;
		}

		public async Task<bool> CreateNote(string userId, NoteRequestDto request)
		{
			await _repository.AddAsync(new NoteContent() { 
				NoteString = request.NoteString,
				Title = request.Title,
				DateTime = DateTime.Now,
			});

			return true;
		}

		public async Task<bool> DeleteNote(string id)
		{
			var res = await _repository.GetByIdAsync(id);

			await _repository.DeleteAsync(res); 

			return true;
		}

		public async Task<NoteResponseDto> GetNote(string id)
		{
			var res = await _repository.GetByIdAsync(id);

			return new NoteResponseDto()
			{
				Id = id,
				NoteString = res.NoteString,
				Title = res.Title,
				DateTime = res.DateTime
			}; 
		}

		public async Task<PaginatedListDto<NoteDtos>> Search(string userId, List<string> groupIds, NoteSearchDto searchDto)
		{
			searchDto.TryCreateSingleQuery(_context, out IQueryable<NoteContent> query);

			var paginationDto = await query.CreatePaginationAsync(searchDto.PageIndex, searchDto.PageSize,
				e => new NoteDtos()
				{
					Id = e.Id,
					Title = e.Title,
					Content = e.NoteString,
				}, false);

			return paginationDto;
		}

		public async Task<bool> UpdateNote(string id, NoteReactDto request)
		{

			var res = await _repository.GetByIdAsync(id);

			res.Title = request.Title;
			res.NoteString = request.NoteString;
			res.DateTime = DateTime.Now;
			await _repository.UpdateAsync(res);

			return true;
		}
	}
}
