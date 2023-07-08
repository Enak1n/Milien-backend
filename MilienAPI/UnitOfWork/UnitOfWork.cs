using Microsoft.Extensions.Caching.Memory;
using MilienAPI.DataBase;
using MilienAPI.UnitOfWork.Interfaces;

namespace MilienAPI.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Context _context;

        public UnitOfWork(Context context, IMemoryCache memoryCache)
        {
             _context = context;
            Ads = new AdRepository(context, memoryCache);
            Favorites = new FavoriteRepository(context, memoryCache);
            Customers = new CustomerRepository(context, memoryCache);
            PaidAd = new PaidAdRepository(context, memoryCache);
        }
        public IAddRepository Ads { get; private set; }
        public IFavoriteRepository Favorites { get; private set; }
        public ICustomerRepository Customers { get; private set; }
        public IPaidAdRepository PaidAd { get; private set; }

        public UnitOfWork(IAddRepository addRepository, IFavoriteRepository favorites, ICustomerRepository customers, IPaidAdRepository paidAds)
        {
            Ads = addRepository;
            Favorites = favorites;
            Customers = customers;
            PaidAd = paidAds;
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
