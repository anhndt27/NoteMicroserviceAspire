using NoteMicroservice.Note.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteMicroservice.Note.Domain.Abstract.Repository
{
    public interface INoteRepository
    {
        Task<List<NoteSimpleResponseViewModel>> GetListNotes(string userId, int? groupId);
		Task<NoteListFilter> GetListNotesFilter(string userId, int? groupId, string? filter, string? orderby);
	}
}
