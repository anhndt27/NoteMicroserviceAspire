using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoteMicroservice.Note.Domain.Dtos;
using NoteMicroservice.Note.Domain.Dtos.BaseDtos;

namespace NoteMicroservice.Note.Domain.Abstract.Service;

public interface INoteService
{
	Task<ResponseMessage> CreateNote(string userId, string email, string groupId, NoteRequestDto request);
	Task<ResponseMessage> UpdateNote(string id, string userId, string email, List<string> groupId, NoteRequestDto request);
	Task<ResponseMessage> DeleteNote(string id, string userId, string email, List<string> groupId);
	Task<ResponseMessageDto<NoteDtos>> GetNote(string id, string email, List<string> groupId);
	Task<ResponseMessageDto<PaginatedListDto<NoteSimpleResponseDto>>> Search(string userId, List<string> groupIds, NoteSearchDto searchDto);
}