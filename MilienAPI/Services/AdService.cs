﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MilienAPI.DataBase;
using MilienAPI.Helpers;
using MilienAPI.Models;
using MilienAPI.Models.Requests;
using MilienAPI.Services.Interfaces;
using MilienAPI.UnitOfWork.Interfaces;
using System.Collections.Generic;

namespace MilienAPI.Services
{
    public class AdService : IAdService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _cache;
        private Random _random = new();

        public AdService(IUnitOfWork unitOfWork, IMemoryCache memoryCache)
        {
            _unitOfWork = unitOfWork;
            _cache = memoryCache;

        }

        public async Task DeleteAd(int id)
        {
            var ad = await _unitOfWork.Ads.GetById(id);

            if(ad != null)
            {
                if(ad.PhotoPath != null)
                {
                    foreach (var item in ad.PhotoPath)
                    {
                                            
                        int lastSlashIndex = item.LastIndexOf('/');
                        string path = item.Substring(lastSlashIndex + 1);
                        FileUploader.DeleteFileFromServer($"/var/images/{path}");
                    }
                }
            }

            await _unitOfWork.Ads.Remove(ad);
        }

        public async Task EditAd(List<string> urls, int id, AdRequest adRequest)
        {
            List<string> uniqueFileNames = new List<string>();
            var currentAd = await _unitOfWork.Ads.GetById(id);

            if (currentAd != null)
            {
                foreach(var item in urls)
                {
                    if (!currentAd.PhotoPath.Contains(item))
                    {
                        int lastSlashIndex = item.LastIndexOf('/');
                        string path = item.Substring(lastSlashIndex + 1);
                        FileUploader.DeleteFileFromServer($"/var/images/{path}");
                    }
                }
            }

            if (adRequest.Images != null)
                uniqueFileNames = FileUploader.UploadImageToServer(adRequest.Images, "/var/images");

            uniqueFileNames.InsertRange(0, urls);
            currentAd.Title = adRequest.Title;
            currentAd.Description = adRequest.Description;
            currentAd.Price = adRequest.Price;
            currentAd.Adress = adRequest.Adress;
            currentAd.Category = adRequest.Category;
            currentAd.Subcategory = adRequest.Subcategory;
            currentAd.PhotoPath = uniqueFileNames.ToArray();

            await _unitOfWork.Save();
        }

        public async Task<List<Ad>> GetAdsByCategory(string category)
        {
            var ads = await _unitOfWork.Ads.FindRange(a => a.Category == category);
            return ads;
        }

        public async Task<List<Ad>> GetAdsByCustomerId(int id)
        {
            var allAdsForCustomer = await _unitOfWork.Ads.FindRange(ad => ad.CustomerId == id);

            var res = allAdsForCustomer.OrderBy(a => a.Id).ToList();

            return res;
        }

        public async Task<List<Ad>> GetAll(int limit, int page, bool refreshAds = false)
        {
            if (refreshAds || !_cache.TryGetValue("cacheAds", out List<Ad> cachedAds))
            {
                var allAds = await _unitOfWork.Ads.GetAll();
                cachedAds = allAds.OrderBy(a => _random.Next()).ToList();
                var premiumAds = cachedAds.Where(a => a.Premium).ToList();
                var regularAds = cachedAds.Where(a => !a.Premium).ToList();

                var mergedAds = new List<Ad>();

                int number = _random.Next(1, 5);
                for (int i = 0; i < regularAds.Count; i++)
                {
                    mergedAds.Add(regularAds[i]);

                    if ((i + 1) % number == 0 && premiumAds.Count > 0)
                    {
                        mergedAds.Add(premiumAds[0]);
                        premiumAds.RemoveAt(0);
                        number = _random.Next(1, 5);
                    }
                }
                _cache.Set("cacheAds", mergedAds, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(15)));
            }

            var cahceValue = (List<Ad>)_cache.Get("cacheAds");
            var paginatedData = cahceValue.Skip((page - 1) * limit).Take(limit).ToList();

            return paginatedData;
        }

        public async Task<List<Ad>> GetNewAds()
        {
            DateTime threeDaysAgo = DateTime.Today.AddDays(-3);

            var ads = await _unitOfWork.Ads.FindRange(a => a.DateOfCreation >= threeDaysAgo);

            var randomAds = ads.OrderBy(x => _random.Next()).Take(6).ToList();

            return randomAds;
        }

        public async Task<List<Ad>> GetNewServices()
        {
            var ads = await _unitOfWork.Ads.FindRange(a => a.Category == "Услуги");

            var newServices = ads.OrderBy(a => _random.Next()).Take(6).ToList();
            return newServices;
        }

        public async Task<List<Ad>> Search(string query)
        {
            var allAds = await _unitOfWork.Ads.GetAll();

            var adList = allAds
                .Where(a => a.Title.ToLower().Contains(query.ToLower())
                || a.Adress.ToLower().Contains(query.ToLower()))
                .OrderBy(x => _random.Next())
                .Take(5)
                .ToList();

            return adList;
        }

        public async Task<List<Ad>> SearchByQuery(string query, int page, int limit)
        {
            var allAds = await _unitOfWork.Ads.FindRange(a => a.Title.ToLower().Contains(query.ToLower())
                     || a.Category.ToLower().Contains(query.ToLower())
                     || a.Subcategory.ToLower().Contains(query.ToLower())
                     || a.Adress.ToLower().Contains(query.ToLower()));

            var ads =  allAds.OrderByDescending(x => x.Premium)
                .ThenByDescending(x => x.Id).ToList();

            var paginatedData = ads.Skip((page - 1) * limit).Take(limit).ToList();
            return paginatedData;
        }
    }
}
