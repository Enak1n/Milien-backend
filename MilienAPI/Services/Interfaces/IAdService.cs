using MilienAPI.Models;
using MilienAPI.Models.Requests;

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
        Task<List<Ad>> Filtration(int limit, int page, string category = null, string subcategory = null,
            int min = 0, int max = int.MaxValue);
        Task EditAd(List<string> urls, int id, AdRequest adRequest);
        Task DeleteAd(int id);
    }
}
