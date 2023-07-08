using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MilienAPI.Helpers;
using MilienAPI.Models;
using MilienAPI.Models.Requests;
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
        private readonly IFavoriteService _favoriteService;
        private readonly IPaidAdService _paidAdService;
        private readonly IMapper _mapper;

        public AdController(IUnitOfWork unitOfWork, IAdService adService, IFavoriteService service, IMapper mapper, IPaidAdService paidAdService)
        {
            _unitOfWork = unitOfWork;
            _adService = adService;
            _favoriteService = service;
            _mapper = mapper;
            _paidAdService = paidAdService;
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

            return Ok(res);
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

            return Ok(new { Ads = adList, Categories = descriptionAttributes });
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

            var ads = await _favoriteService.GetFavoriteAds(authorizedUser);

            return Ok(ads);
        }

        [HttpGet]
        [Authorize]
        public async Task<bool> IsFavorite(int id)
        {
            var authorizedUser = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

            return await _favoriteService.IsFavorite(id, authorizedUser);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateAd([FromForm] AdRequest ad)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            List<string> uniqueFileNames = new List<string>();

            if (ad.Images != null)
                uniqueFileNames = FileUploader.UploadImageToServer(ad.Images, "/var/images");

            var createdAd = _mapper.Map<AdRequest, Ad>(ad);

            createdAd.PhotoPath = uniqueFileNames.ToArray();
            createdAd.CustomerId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

            await _unitOfWork.Ads.Add(createdAd);

            await _unitOfWork.Save();

            return Ok(createdAd);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddToFavorite([FromBody] int id)
        {
            var authorizedUser = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

            Favorite favorite = new Favorite(authorizedUser, id);

            await _unitOfWork.Favorites.Add(favorite);
            await _unitOfWork.Save();

            return Ok();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreatePaidAd([FromForm] AdRequest adRequest)
        {
            var authorizedUser = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var res = await _paidAdService.CreatePaidAd(adRequest.Title, authorizedUser);
            await _unitOfWork.Save();
            return Ok(res);

        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpgradeToPremium([FromBody] int id)
        {
            await _paidAdService.UpgradeToPremium(id);

            await _unitOfWork.Save();

            return Ok();
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> EditAd([FromForm] List<string> urls, [FromForm] AdRequest adRequest)
        {
            await _adService.EditAd(urls, adRequest.Id, adRequest);

            return Ok();
        }
    }
}