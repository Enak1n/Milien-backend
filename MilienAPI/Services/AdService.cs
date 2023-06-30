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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _cache;
        private readonly Context _context;
        private Random _random = new();

        public AdService(IUnitOfWork unitOfWork, IMemoryCache memoryCache, Context context)
        {
            _unitOfWork = unitOfWork;
            _cache = memoryCache;
            _context = context;

        }

        public async Task<List<Ad>> GetAdsByCategory(string category)
        {
            var ads = await _unitOfWork.Ads.Find(a => a.Category == category);
            return ads;
        }

        public async Task<List<Ad>> GetAdsByCustomerId(int id)
        {
            var allAdsForCustomer = await _unitOfWork.Ads.Find(ad => ad.CustomerId == id);

            var res = allAdsForCustomer.OrderBy(a => a.Id).ToList();

            return res;
        }

        public async Task<List<Ad>> GetAll(int limit, int page, bool refreshAds = false)
        {
            if (refreshAds || !_cache.TryGetValue("cacheAds", out List<Ad> cachedAds))
            {
                var allAds = await _unitOfWork.Ads.GetAll();
                cachedAds = allAds.OrderBy(a => _random.Next()).ToList();
                var premiumAds = cachedAds.Where(a => a.Premium).ToList();
                var regularAds = cachedAds.Where(a => !a.Premium).ToList();

                var mergedAds = new List<Ad>();

                int number = _random.Next(1, 5);
                for (int i = 0; i < regularAds.Count; i++)
                {
                    mergedAds.Add(regularAds[i]);

                    if ((i + 1) % number == 0 && premiumAds.Count > 0)
                    {
                        mergedAds.Add(premiumAds[0]);
                        premiumAds.RemoveAt(0);
                        number = _random.Next(1, 5);
                    }
                }
                _cache.Set("cacheAds", mergedAds, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(15)));
            }

            var cahceValue = (List<Ad>)_cache.Get("cacheAds");
            var paginatedData = cahceValue.Skip((page - 1) * limit).Take(limit).ToList();

            return paginatedData;
        }

        public async Task<List<Ad>> GetNewAds()
        {
            DateTime threeDaysAgo = DateTime.Today.AddDays(-3);

            var ads = await _unitOfWork.Ads.Find(a => a.DateOfCreation >= threeDaysAgo);

            var randomAds = ads.OrderBy(x => _random.Next()).Take(6).ToList();

            return randomAds;
        }

        public async Task<List<Ad>> GetNewServices()
        {
            var ads = await _unitOfWork.Ads.Find(a => a.Category == "Услуги");

            var newServices = ads.OrderBy(a => _random.Next()).Take(6).ToList();
            return newServices;
        }

        public async Task<List<Ad>> Search(string query)
        {
            var allAds = await _unitOfWork.Ads.GetAll();

            var adList = allAds
                .Where(a => a.Title.ToLower().Contains(query.ToLower())
                || a.Adress.ToLower().Contains(query.ToLower()))
                .OrderBy(x => _random.Next())
                .Take(5)
                .ToList();

            return adList;
        }

        public async Task<List<Ad>> SearchByQuery(string query, int page, int limit)
        {
            var allAds = await _unitOfWork.Ads.Find(a => a.Title.ToLower().Contains(query.ToLower())
                     || a.Category.ToLower().Contains(query.ToLower())
                     || a.Subcategory.ToLower().Contains(query.ToLower())
                     || a.Adress.ToLower().Contains(query.ToLower()));

            var ads =  allAds.OrderByDescending(x => x.Premium)
                .ThenByDescending(x => x.Id).ToList();

            var paginatedData = ads.Skip((page - 1) * limit).Take(limit).ToList();
            return paginatedData;
        }

        public async Task<List<Ad>> GetFavoriteAds(int userId)
        {
            var ads = await _context.Ads.Join(_context.Favorites,
                ad => ad.Id,
                favorite => favorite.AdId,
                (ad, favorite) => new { Ad = ad, Favorite = favorite })
                .Where(joinResult => joinResult.Favorite.CustomerId == userId)
                .Select(joinResult => joinResult.Ad)
                .ToListAsync();

            return ads;
        }

        public async Task<bool> IsFavorite(int id, int userId)
        {
            var ads = await _context.Favorites.Where(a => a.AdId == id &&
            a.CustomerId == userId).FirstOrDefaultAsync();

            return ads != null;
        }
    }
}
