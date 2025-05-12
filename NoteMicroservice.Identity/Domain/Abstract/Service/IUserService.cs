using NoteMicroservice.Identity.Domain.Entities;
using NoteMicroservice.Identity.Domain.Dto;

namespace NoteMicroservice.Identity.Domain.Abstract.Service
{
    public interface IUserService
    {
        Task<UserResponseDto> GetUserByIdAsync(string id);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task CreateUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(User user);
    }
}
