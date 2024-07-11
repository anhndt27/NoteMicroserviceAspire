using Microsoft.EntityFrameworkCore;
using NoteMicroservice.Identity.Domain.Abstract.Repository;
using NoteMicroservice.Identity.Domain.Entities;
using NoteMicroservice.Identity.Domain.ViewModel;
using NoteMicroservice.Identity.Infrastructure;

namespace NoteMicroservice.Identity.Domain.Implement.Repository
{
	public class GroupRepository : IGroupRepository
	{
		private readonly ApplicationDbContext _dbContext;

		public GroupRepository(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<int> CreateGroup(GroupRequestViewModel request)
		{
			var group = new Group()
			{
				Name = request.GroupName,
				Users = new List<User>()
			};

			var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == request.UserId);

			if (user != null) 
			{ 
				group.Users.Add(user); 
			}

			_dbContext.Groups.Add(group);
			await _dbContext.SaveChangesAsync();

			return group.Id;
		}

		public async Task<bool> JoinGroup(ReactGroupViewModel request)
		{
			var group = await _dbContext.Groups.Include(g => g.Users).FirstOrDefaultAsync(x => x.Id == request.GroupId);

			var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == request.UserId);
			if (group != null && user != null)
			{
				group.Users.Add(user);
			}

			_dbContext.Groups.Update(group);
			await _dbContext.SaveChangesAsync();

			return true;
		}

		public async Task<bool> OutGroup(ReactGroupViewModel request)
		{
			var group = await _dbContext.Groups
			.Include(g => g.Users)
			.FirstOrDefaultAsync(x => x.Id == request.GroupId);

			if (group == null)
				return false;

			var user = group.Users.FirstOrDefault(u => u.Id == request.UserId);
			if (user != null)
			{
				group.Users.Remove(user);
				await _dbContext.SaveChangesAsync();
				return true;
			}

			return false;
		}

    }
}
