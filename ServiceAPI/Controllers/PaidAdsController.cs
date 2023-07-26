using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            await _unitOfWork.PaidAds.RemoveRange(ads);

            return Ok();
        }
    }
}
