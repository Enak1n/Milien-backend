using Microsoft.Extensions.Caching.Memory;
using ServiceAPI.Data;
using ServiceAPI.Models;
using ServiceAPI.UnitOfWork.Interfaces;

namespace ServiceAPI.UnitOfWork
{
    public class PaidAdRepository : GenericRepository<PaidAd>, IPaidAdRepository
    {
        public PaidAdRepository(Context context, IMemoryCache memoryCache) : base(context, memoryCache)
        {

        }
    }
}
