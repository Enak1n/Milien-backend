using Microsoft.Extensions.Caching.Memory;
using Millien.Domain.Entities;
using MilienAPI.Services.Interfaces;

using Millien.Domain.UnitOfWork.Interfaces;

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
            var ad = await _unitOfWork.Ads.Find(a => a.Title == title && a.CustomerId == userId);
            ad.Premium = true;
            PaidAd paidAd = new PaidAd(ad.Id, DateTime.UtcNow.AddDays(10));
            await _unitOfWork.PaidAds.Add(paidAd);

            return paidAd;
        }

        public async Task UpgradeToPremium(int id)
        {
            var res = await _unitOfWork.Ads.GetById(id);
            res.Premium = true;
            PaidAd paidAd = new PaidAd(res.Id, DateTime.UtcNow.AddDays(10));
            await _unitOfWork.PaidAds.Add(paidAd);
        }
    }
}
