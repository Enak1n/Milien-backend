using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MilienAPI.DataBase;
using MilienAPI.Models;
using MilienAPI.Services.Interfaces;
using MilienAPI.UnitOfWork.Interfaces;
using System.Collections.Generic;

namespace MilienAPI.Services
{
    public class AdService : IAdService
    {
        private readonly Context _context;
        private readonly IMemoryCache _cache;

        public AdService(Context unitOfWork, IMemoryCache cache)
        {
            _context = unitOfWork;
            _cache = cache;

        }
        public async Task<List<Ad>> GetAdsByCustomerId(int id)
        {
            var allAdsForCustomer = await _context.Ads.Where(ad => ad.CustomerId == id)
                                .OrderByDescending(a => a.Id)
                                .ToListAsync();

            return allAdsForCustomer;
        }

        public async Task<List<Ad>> GetAll(int limit, int page, bool refreshAds = false)
        {
            if (refreshAds || !_cache.TryGetValue("cacheAds", out List<Ad> cachedAds))
            {
                Random random = new Random();
                cachedAds = await _context.Ads.OrderByDescending(a => EF.Functions.Random()).ToListAsync();
                var premiumAds = cachedAds.Where(a => a.Premium).ToList();
                var regularAds = cachedAds.Where(a => !a.Premium).ToList();

                var mergedAds = new List<Ad>();

                int number = random.Next(1, 5);
                for (int i = 0; i < regularAds.Count; i++)
                {
                    mergedAds.Add(regularAds[i]);

                    if ((i + 1) % number == 0 && premiumAds.Count > 0)
                    {
                        mergedAds.Add(premiumAds[0]);
                        premiumAds.RemoveAt(0);
                        number = random.Next(1, 5);
                    }
                }
                _cache.Set("cacheAds", mergedAds, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(15)));
            }
            var cahceValue = (List<Ad>)_cache.Get("cacheAds");
            var paginatedData = cahceValue.Skip((page - 1) * limit).Take(limit).ToList();

            return paginatedData;
        }
    }
}
