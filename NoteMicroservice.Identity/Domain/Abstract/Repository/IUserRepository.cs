using NoteMicroservice.Identity.Domain.ViewModel;

namespace NoteMicroservice.Identity.Domain.Abstract.Repository
{
	public interface IUserRepository
	{
		Task<UserResponseViewModel> GetUserById(string id);
	}
}
