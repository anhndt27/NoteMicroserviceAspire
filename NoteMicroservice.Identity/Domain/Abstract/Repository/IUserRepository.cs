using NoteMicroservice.Identity.Domain.Dto;
using NoteMicroservice.Identity.Domain.Dto.BaseDtos;

namespace NoteMicroservice.Identity.Domain.Abstract.Repository
{
	public interface IUserRepository
	{
		Task<UserResponseDto> GetUserById(string id);
		Task<PaginatedListDto<UserResponseDto>> Search(string identityId, UserSearchRequestDto request);
	}
}
