using NoteMicroservice.Identity.Domain.Entities;
using NoteMicroservice.Identity.Domain.ViewModel;

namespace NoteMicroservice.Identity.Domain.Abstract.Service
{
    public interface IUserService
    {
        Task<UserResponseViewModel> GetUserByIdAsync(string id);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task CreateUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(User user);
    }
}
