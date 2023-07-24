using Microsoft.Extensions.Caching.Memory;
using MilienAPI.Models;
using MilienAPI.Services.Interfaces;
using MilienAPI.UnitOfWork.Interfaces;

namespace MilienAPI.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FavoriteService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> GetCountOfFavorite(int adId)
        {
            var ads = await _unitOfWork.Favorites.FindRange(a => a.AdId == adId);

            return ads.Count;
        }

        public async Task<List<Ad>> GetFavoriteAds(int userId)
        {
            var ads = await _unitOfWork.Ads.GetAll();
            var favorite = await _unitOfWork.Favorites.GetAll();

            var favoriteAds = ads.Join(favorite,
                                        ad => ad.Id,
                                        favorite => favorite.AdId,
                                        (ad, favorite) => new { Ad = ad, Favorite = favorite })
                                  .Where(joinResult => joinResult.Favorite.CustomerId == userId)
                                  .Select(joinResult => joinResult.Ad)
                                  .ToList();

            return favoriteAds;
        }

        public async Task<bool> IsFavorite(int id, int userId)
        {
            var isFavorite = await _unitOfWork.Favorites.Find(a => a.AdId == id &&
                                                a.CustomerId == userId);

            return isFavorite != null ? true : false;
        }

        public async Task RemoveFromFavorite(int id, int userId)
        {
            var ad = await _unitOfWork.Favorites.Find(a => a.AdId == id &&
            a.CustomerId == userId);

            if(ad != null)
            {
                await _unitOfWork.Favorites.Remove(ad);
            }
        }
    }
}
