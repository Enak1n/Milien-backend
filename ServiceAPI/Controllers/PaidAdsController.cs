using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Millien.Domain.Entities;
using Millien.Domain.UnitOfWork.Interfaces;


namespace ServiceAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class PaidAdsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaidAdsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteExpiredRows()
        {
            DateTime currentTime = DateTime.Now;

            var ads = await _unitOfWork.PaidAds.FindRange(a => a.ExpiryTime < currentTime);

            foreach(var ad in ads)
            {
                var currentAd = await _unitOfWork.Ads.Find(a => a.Id == ad.AdId);

                currentAd.Premium = false;
            }
            await _unitOfWork.PaidAds.RemoveRange(ads);
            await _unitOfWork.Save();

            return Ok();
        }
    }
}
