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
        }
        public IAddRepository Ads { get; private set; }

        public UnitOfWork(IAddRepository addRepository)
        {
            Ads = addRepository;
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
