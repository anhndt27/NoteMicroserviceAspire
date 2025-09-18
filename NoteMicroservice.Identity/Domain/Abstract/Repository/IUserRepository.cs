using NoteMicroservice.Identity.Domain.Dto;
using NoteMicroservice.Identity.Domain.Dto.BaseDtos;

namespace NoteMicroservice.Identity.Domain.Abstract.Repository
{
	public interface IUserRepository
	{
		Task<List<string>> GetUserGroupIdsAsync(string userId);
	}
}
