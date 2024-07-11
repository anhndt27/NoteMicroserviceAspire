using Microsoft.EntityFrameworkCore;
using NoteMicroservice.Note.Domain.Abstract.Repository;
using NoteMicroservice.Note.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteMicroservice.Note.Infrastructure.Repository
{
	public class Repository<T> : IRepository<T> where T : class
	{
		protected readonly NoteDbContext _context;
		private readonly DbSet<T> _dbSet;

		public Repository(NoteDbContext context)
		{
			_context = context;
			_dbSet = _context.Set<T>();
		}

		public async Task<T> GetByIdAsync(int id)
		{
			return await _dbSet.FindAsync(id);
		}

		public async Task<IEnumerable<T>> GetAllAsync()
		{
			return await _dbSet.ToListAsync();
		}

		public async Task AddAsync(T entity)
		{
			await _dbSet.AddAsync(entity);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateAsync(T entity)
		{
			_dbSet.Update(entity);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(T entity)
		{
			_dbSet.Remove(entity);
			await _context.SaveChangesAsync();
		}
	}
}
