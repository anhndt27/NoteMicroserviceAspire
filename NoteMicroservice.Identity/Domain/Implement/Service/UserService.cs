using Microsoft.AspNetCore.Identity;
using NoteMicroservice.Identity.Domain.Abstract.Repository;
using NoteMicroservice.Identity.Domain.Abstract.Service;
using NoteMicroservice.Identity.Domain.Entities;
using NoteMicroservice.Identity.Domain.Dto;

namespace NoteMicroservice.Identity.Domain.Implement.Service
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _repository;
        private readonly IUserRepository _userRepository;
        public UserService(IRepository<User> repository, IUserRepository userRepository)
        {
            _repository = repository;
            _userRepository = userRepository;
        }

        public async Task<UserResponseDto> GetUserByIdAsync(string id)
        {
            var user = await _userRepository.GetUserById(id);

			return new UserResponseDto()
            {
                UserName = user.UserName,
                Email = user.Email,
                Groups = user.Groups
            };
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task CreateUserAsync(User user)
        {
            await _repository.AddAsync(user);
        }

        public async Task UpdateUserAsync(User user)
        {
            await _repository.UpdateAsync(user);
        }

        public async Task DeleteUserAsync(User user)
        {
            await _repository.DeleteAsync(user);
        }
    }
}
