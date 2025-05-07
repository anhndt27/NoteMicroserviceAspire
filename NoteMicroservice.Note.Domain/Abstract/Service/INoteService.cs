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
		Task<bool> UpdateNote(string id, NoteReactViewModel request);
		Task<bool> DeleteNote(string id);
		Task<NoteResponseViewModel> GetNote(string id);
		Task<List<NoteSimpleResponseViewModel>> GetListNotes(string userId, string groupId);
		Task<NoteListFilter> SearchListNotes(string filter, string orderby, string userId, string groupId);
		Task<bool> UpdateCategory(string id, UpdateCategoryRequest request);
	}
}
