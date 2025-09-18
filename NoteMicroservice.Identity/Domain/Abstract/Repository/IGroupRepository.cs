using NoteMicroservice.Identity.Domain.Dto;
using NoteMicroservice.Identity.Domain.Dto.BaseDtos;

namespace NoteMicroservice.Identity.Domain.Abstract.Repository
{
	public interface IGroupRepository
	{
		Task<ResponseMessage> JoinGroup(string identityId, ReactGroupDto request);
		Task<ResponseMessage> OutGroup(string identityId, ReactGroupDto request);
		Task<ResponseMessage> CreateGroup(string identityId, GroupRequestDto request);
		Task<PaginatedListDto<GroupResponseDto>> SearchGroup (string identityId, GroupSearchRequestDto request);
		Task<ResponseMessage> DeleteGroup(string identityId, string id);
	}
}
