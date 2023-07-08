using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MilienAPI.DataBase;
using MilienAPI.Models;
using MilienAPI.UnitOfWork.Interfaces;
using System.Linq.Expressions;

namespace MilienAPI.UnitOfWork
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly IMemoryCache _cache;
        private readonly Context _context;

        public GenericRepository(Context context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task Add(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public async Task AddRange(T entity)
        {
            await _context.Set<T>().AddRangeAsync(entity);
        }

        public async Task Edit(T entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<T> Find(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).FirstOrDefaultAsync();
        }

        public async Task<List<T>> FindRange(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<List<T>> GetAll()
        {
            if (!_cache.TryGetValue(typeof(T), out List<T> entities))
            {
                entities = await _context.Set<T>().ToListAsync();
                _cache.Set(typeof(T), entities,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(1)));
            }

            return entities;
        }

        public async Task<T> GetById(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task Remove(T entity)
        {
            await Task.Run(() => _context.Set<T>().Remove(entity));
        }

        public async Task RemoveRange(List<T> entities)
        {
            await Task.Run(() => _context.Set<T>().RemoveRange(entities));
        }
    }
}
