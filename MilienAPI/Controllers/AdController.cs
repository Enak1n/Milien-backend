using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MilienAPI.Services.Interfaces;
using MilienAPI.UnitOfWork;
using MilienAPI.UnitOfWork.Interfaces;
using System.ComponentModel;
using System.Reflection;
using System.Security.Claims;

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

        [HttpGet]
        public async Task<IActionResult> Test() 
        {
            var ads = await _unitOfWork.Ads.GetAll();

            return Ok(ads);
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
            var allAds = await _unitOfWork.Ads.GetAll();
            var paginatedData = await _adService.GetAll(limit, page, refreshAds);

            Response.Headers.Add("count", $"{allAds.Count}");

            return Ok(paginatedData);
        }

        [HttpGet]
        public async Task<IActionResult> GetAdById(int id)
        {
            var ad = await _unitOfWork.Ads.GetById(id);
            return ad == null ? NotFound() : Ok(ad);
        }

        [HttpGet]
        public async Task<IActionResult> GetAdsByCategory(string category)
        {
            var allAdswithCategory = await _adService.GetAdsByCategory(category);

            return Ok(allAdswithCategory);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string query)
        {
            var adList = await _adService.Search(query);

            var array = Enum.GetNames(typeof(Category));

            var categories = array.Where(
                a => a.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0).ToList();

            var descriptionAttributes = new List<string>();

            if (categories.Any())
            {
                var enumType = typeof(Category);

                foreach (var categoryName in categories)
                {
                    var descriptionField = enumType.GetField(categoryName);
                    var descriptionAttribute = descriptionField?.
                        GetCustomAttribute<DescriptionAttribute>(false)?.Description;

                    if (!string.IsNullOrEmpty(descriptionAttribute))
                    {
                        descriptionAttributes.Add(descriptionAttribute);
                    }
                }

            }

            return Ok(new {Ads = adList, Categories = descriptionAttributes});
        }

        [HttpGet]
        public async Task<IActionResult> SearchByQuery(string query, int page, int limit)
        {
            var allAds = await _unitOfWork.Ads.GetAll();

            var paginatedData = await _adService.SearchByQuery(query, page, limit);

            Response.Headers.Add("count", $"{allAds.Count}");

            return Ok(paginatedData);
        }

        [HttpGet]
        public async Task<IActionResult> GetNewAds()
        {
            var newAds = await _adService.GetNewAds();
            return Ok(newAds);
        }

        [HttpGet]
        public async Task<IActionResult> GetNewServices()
        {
            var newServices = await _adService.GetNewServices();
            return Ok(newServices);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetFavoritesAds()
        {
            var authorizedUser = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var ads = await _adService.GetFavoriteAds(authorizedUser);

            return Ok(ads);
        }

        [HttpGet]
        [Authorize]
        public async Task<bool> IsFavorite(int id)
        {
            var authorizedUser = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

            return await _adService.IsFavorite(id, authorizedUser);
        }
    }
}
