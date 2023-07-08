using Microsoft.Extensions.Caching.Memory;
using MilienAPI.Models;
using MilienAPI.Models.Requests;
using MilienAPI.Services.Interfaces;
using MilienAPI.UnitOfWork.Interfaces;

namespace MilienAPI.Services
{
    public class PaidAdService : IPaidAdService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _cache;

        public PaidAdService(IUnitOfWork unitOfWork, IMemoryCache memoryCache)
        {
            _unitOfWork = unitOfWork;
            _cache = memoryCache;

        }
        public async Task<PaidAd> CreatePaidAd(string title, int userId)
        {
            var ads = await _unitOfWork.Ads.Find(a => a.Title == title && a.CustomerId == userId);
            var res = ads.OrderByDescending(a => a.Id).FirstOrDefault();
            res.Premium = true;
            PaidAd paidAd = new PaidAd(res.Id, DateTime.UtcNow.AddDays(10));
            await _unitOfWork.PaidAd.Add(paidAd);

            return paidAd;
        }

        public async Task UpgradeToPremium(int id)
        {
            var res = await _unitOfWork.Ads.GetById(id);
            res.Premium = true;
            PaidAd paidAd = new PaidAd(res.Id, DateTime.UtcNow.AddDays(10));
            await _unitOfWork.PaidAd.Add(paidAd);
        }
    }
}
