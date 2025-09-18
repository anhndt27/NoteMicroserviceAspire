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

		public Task<List<string>> GetUserGroupIdsAsync(string userId)
		{
			var groupIds = _context.UserGroups
				.Where(ug => ug.UserId == userId)
				.Select(ug => ug.GroupId)
				.ToListAsync();
			
			return groupIds;
		}
	}
}
