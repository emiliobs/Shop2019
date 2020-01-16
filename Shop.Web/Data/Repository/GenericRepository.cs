using Microsoft.EntityFrameworkCore;
using Shop.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Web.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class , IEntity
    {
		private readonly AppDataContext _context;

		public GenericRepository(AppDataContext context)
		{
			_context = context;
		}

		public IQueryable<T> GetAll()
		{
			return _context.Set<T>().AsNoTracking();
		}

		public async Task<T> GetByIdAsync(int id)
		{
			return await _context.Set<T>()
				.AsNoTracking()
				.FirstOrDefaultAsync(e => e.Id == id);
		}

		public async Task CreateAsync(T entity)
		{
			await _context.Set<T>().AddAsync(entity);
			await SaveAllAsync();
		}

		public async Task UpdateAsync(T entity)
		{
			_context.Set<T>().Update(entity);
			await SaveAllAsync();
		}

		public async Task DeleteAsync(T entity)
		{
			_context.Set<T>().Remove(entity);
			await SaveAllAsync();
		}

		public async Task<bool> ExistAsync(int id)
		{
			return await _context.Set<T>().AnyAsync(e => e.Id == id);

		}

		public async Task<bool> SaveAllAsync()
		{
			return await _context.SaveChangesAsync() > 0;
		}


	}
}
