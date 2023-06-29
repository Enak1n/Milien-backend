using MilienAPI.Models;

namespace MilienAPI.Services.Interfaces
{
    public interface IAdService
    {
        Task<List<Ad>> GetAdsByCustomerId(int id);
        Task<List<Ad>> GetAll(int limit, int page, bool refreshAds = false);
    }
}
