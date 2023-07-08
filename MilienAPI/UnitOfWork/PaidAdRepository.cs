using Microsoft.Extensions.Caching.Memory;
using MilienAPI.DataBase;
using MilienAPI.Models;
using MilienAPI.UnitOfWork.Interfaces;

namespace MilienAPI.UnitOfWork
{
    public class PaidAdRepository : GenericRepository<PaidAd>, IPaidAdRepository
    {
        public PaidAdRepository(Context context, IMemoryCache memoryCache) : base(context, memoryCache)
        {

        }
    }
}
