using NoteMicroservice.Note.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteMicroservice.Note.Domain.Abstract.Service
{
	public interface INoteService
	{
		Task<bool> CreateNote(NoteRequestDto request);
		Task<bool> UpdateNote(string id, NoteReactDto request);
		Task<bool> DeleteNote(string id);
		Task<NoteResponseDto> GetNote(string id);
		Task<List<NoteSimpleResponseDto>> GetListNotes(string userId, string groupId);
		Task<NoteListFilter> SearchListNotes(string filter, string orderby, string userId, string groupId);
		Task<bool> UpdateCategory(string id, UpdateCategoryRequest request);
	}
}
