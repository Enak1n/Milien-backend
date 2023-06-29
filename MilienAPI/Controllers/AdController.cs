using Microsoft.AspNetCore.Mvc;
using MilienAPI.Services.Interfaces;
using MilienAPI.UnitOfWork;
using MilienAPI.UnitOfWork.Interfaces;

namespace MilienAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AdController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAdService _adService; 

        public AdController(IUnitOfWork unitOfWork, IAdService adService)
        {
             _unitOfWork = unitOfWork;
            _adService = adService;
        }

        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetAdsByCustomerId(int customerId)
        {
            var res = await _adService.GetAdsByCustomerId(customerId);

            return res.Count == 0 ? BadRequest() : Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int limit, int page, bool refreshAds = false)
        {
            var allAdsCount = await _unitOfWork.Ads.GetAll();
            var paginatedData = await _adService.GetAll(limit, page, refreshAds);

            Response.Headers.Add("count", $"{allAdsCount.Count}");

            return Ok(paginatedData);
        }

        [HttpGet]
        public async Task<IActionResult> GetAdById(int id)
        {
            var ad = await _unitOfWork.Ads.GetById(id);
            return ad == null ? NotFound() : Ok(ad);
        }
    }
}
