using Microsoft.EntityFrameworkCore;
using NoteMicroservice.Identity.Domain.Abstract.Repository;
using NoteMicroservice.Identity.Domain.Entities;
using NoteMicroservice.Identity.Domain.ViewModel;
using NoteMicroservice.Identity.Infrastructure;

namespace NoteMicroservice.Identity.Domain.Implement.Repository
{
	public class UserRepository : IUserRepository
	{
		protected readonly ApplicationDbContext _context;
		private readonly DbSet<User> _dbSet;

		public UserRepository(ApplicationDbContext context)
		{
			_context = context;
			_dbSet = _context.Set<User>();
		}

		public async Task<UserResponseViewModel> GetUserById(string id)
		{
			var user = await _context.Users.Include(u => u.Group).FirstOrDefaultAsync(u => u.Id == id);
			
			if(user is null) throw new Exception("Cant not find User");

			return new UserResponseViewModel(){
				UserName = user.UserName,
				Email = user.Email,
				GroupName = user.Group?.Name
			};
		}
	}
}
