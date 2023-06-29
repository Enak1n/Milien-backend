using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MilienAPI.DataBase;
using MilienAPI.Models;
using MilienAPI.UnitOfWork.Interfaces;

namespace MilienAPI.UnitOfWork
{
    public class AdRepository : IAddRepository
    {
        private readonly IMemoryCache _memoryCache;
        private readonly Context _context;

        public AdRepository(IMemoryCache cache, Context context)
        {
            _memoryCache = cache;
            _context = context;
        }

        public Task<Ad> Add(Ad entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Ad> Edit(Ad entity, List<string> urls)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Ad>> GetAll()
        {
            if (!_memoryCache.TryGetValue("cacheAds", out List<Ad> cachedAds))
            {
                cachedAds = await _context.Ads.ToListAsync();
                _memoryCache.Set("cacheAds", cachedAds,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(1)));
            }

            return cachedAds;
        }

        public async Task<Ad> GetById(int id)
        {
            var ad = await _context.Ads.FindAsync(id);

            if (ad == null)
                return null;

            return ad;
        }
    }
}
