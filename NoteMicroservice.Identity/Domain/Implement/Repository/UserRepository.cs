using Microsoft.EntityFrameworkCore;
using NoteMicroservice.Identity.Domain.Abstract.Repository;
using NoteMicroservice.Identity.Domain.Entities;
using NoteMicroservice.Identity.Domain.Dto;
using NoteMicroservice.Identity.Domain.Dto.BaseDtos;
using NoteMicroservice.Identity.Infrastructure;

namespace NoteMicroservice.Identity.Domain.Implement.Repository
{
	public class UserRepository : IUserRepository
	{
		private readonly ApplicationDbContext _context;

		public UserRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<UserResponseDto> GetUserById(string id)
		{
			var user = await _context.Users
				.Include(u => u.UserGroups)
				.ThenInclude(ug => ug.Group)
				.FirstOrDefaultAsync(u => u.Id == id);
			
			if(user is null) throw new Exception("Cant not find User");

			return new UserResponseDto(){
				UserName = user.UserName,
				Email = user.Email,
				Groups = user.UserGroups?.Select(e => e.Group.ToGroupResponseDto()).ToList()
			};
		}

		public Task<PaginatedListDto<UserResponseDto>> Search(string identityId, UserSearchRequestDto request)
		{
			throw new NotImplementedException();
		}
	}
}
