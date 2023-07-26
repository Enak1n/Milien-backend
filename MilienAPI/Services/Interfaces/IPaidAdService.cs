using Millien.Domain.Entities;
using MilienAPI.Models.Requests;

namespace MilienAPI.Services.Interfaces
{
    public interface IPaidAdService
    {
        Task<PaidAd> CreatePaidAd(string title, int userId);
        Task UpgradeToPremium(int id);
    }
}
