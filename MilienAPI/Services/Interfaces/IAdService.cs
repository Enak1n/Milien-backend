using MilienAPI.Models;

namespace MilienAPI.Services.Interfaces
{
    public interface IAdService
    {
        Task<List<Ad>> GetAdsByCustomerId(int id);
        Task<List<Ad>> GetAll(int limit, int page, bool refreshAds = false);
        Task<List<Ad>> GetAdsByCategory(string category);
        Task<List<Ad>> GetNewAds();
        Task<List<Ad>> GetNewServices();
        Task<List<Ad>> Search(string query);
        Task<List<Ad>> SearchByQuery(string query, int page, int limit);
    }
}
