using Microsoft.Extensions.Caching.Memory;
using Millien.Domain.DataBase;
using Millien.Domain.Entities;
using Millien.Domain.UnitOfWork.Interfaces;

namespace Millien.Domain.UnitOfWork
{
    public class PaidAdRepository : GenericRepository<PaidAd>, IPaidAdRepository
    {
        public PaidAdRepository(Context context, IMemoryCache memoryCache) : base(context, memoryCache)
        {

        }
    }
}
