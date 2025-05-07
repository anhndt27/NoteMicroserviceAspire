using NoteMicroservice.Note.Domain.Abstract.Repository;
using NoteMicroservice.Note.Domain.Abstract.Service;
using NoteMicroservice.Note.Domain.Entity;
using NoteMicroservice.Note.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace NoteMicroservice.Note.Domain.Implement.Service
{
	public class NoteService : INoteService
	{
		private readonly INoteRepository _noteRepository;
		private readonly IRepository<NoteContent> _repository;
		
		public NoteService(IRepository<Entity.NoteContent> repository, INoteRepository noteRepository)
		{
			_repository = repository;
			_noteRepository = noteRepository;
		}

		public async Task<bool> CreateNote(NoteRequestViewModel request)
		{
			await _repository.AddAsync(new NoteContent() { 
				NoteString = request.NoteString,
				GroupId = request.GroupId,
				UserId = request.UserId,
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

		public async Task<List<NoteSimpleResponseViewModel>> GetListNotes(string userId, string groupId)
		{
			var res = await _noteRepository.GetListNotes(userId, groupId);
			return res;
		}

		public async Task<NoteResponseViewModel> GetNote(string id)
		{
			var res = await _repository.GetByIdAsync(id);

			return new NoteResponseViewModel()
			{
				Id = id,
				NoteString = res.NoteString,
				Title = res.Title,
				UserId = res.UserId,
				DateTime = res.DateTime
			}; 
		}

		public async Task<NoteListFilter> SearchListNotes(string filter, string orderby, string userId, string groupId)
		{
			var res = await _noteRepository.GetListNotesFilter(userId, groupId, filter, orderby);
			return res;
		}

		public async Task<bool> UpdateCategory(string id, UpdateCategoryRequest request)
		{
			var res = await _repository.GetByIdAsync(id);

			if(request.Category == "Private")
			{
				res.GroupId = null;
			}
			else if(request.Category == "Group")
			{
				res.GroupId= request.GroupId;
			}
			await _repository.UpdateAsync(res);
			return true;
		}

		public async Task<bool> UpdateNote(string id, NoteReactViewModel request)
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
