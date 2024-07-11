using NoteMicroservice.Note.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteMicroservice.Note.Domain.Abstract.Service
{
	public interface INoteService
	{
		Task<bool> CreateNote(NoteRequestViewModel request);
		Task<bool> UpdateNote(int id, NoteReactViewModel request);
		Task<bool> DeleteNote(int id);
		Task<NoteResponseViewModel> GetNote(int id);
		Task<List<NoteSimpleResponseViewModel>> GetListNotes(string userId, int? groupId);
		Task<NoteListFilter> SearchListNotes(string? filter, string? orderby, string userId, int? group);
		Task<bool> UpdateCategory(int id, UpdateCategoryRequest request);
	}
}
