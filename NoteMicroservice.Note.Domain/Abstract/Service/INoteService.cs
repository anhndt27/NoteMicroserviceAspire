using NoteMicroservice.Note.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoteMicroservice.Note.Domain.Dtos;
using NoteMicroservice.Note.Domain.Dtos.BaseDtos;
using NoteMicroservice.Note.Domain.ViewModel;

namespace NoteMicroservice.Note.Domain.Abstract.Service
{
	public interface INoteService
	{
		Task<bool> CreateNote(string userId, NoteRequestDto request);
		Task<bool> UpdateNote(string id, NoteReactDto request);
		Task<bool> DeleteNote(string id);
		Task<NoteResponseDto> GetNote(string id);
		Task<PaginatedListDto<NoteDtos>> Search(string userId, List<string> groupIds, NoteSearchDto searchDto);
	}
}
