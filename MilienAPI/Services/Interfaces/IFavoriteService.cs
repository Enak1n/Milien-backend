using MilienAPI.Models;

namespace MilienAPI.Services.Interfaces
{
    public interface IFavoriteService
    {
        Task<List<Ad>> GetFavoriteAds(int userId);
        Task<bool> IsFavorite(int id, int userId);
        Task RemoveFromFavorite(int id, int userId);
    }
}
