using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Millien.Domain.DataBase;
using Millien.Domain.Entities;
using Millien.Domain.UnitOfWork.Interfaces;

namespace Millien.Domain.UnitOfWork
{
    public class AdRepository : GenericRepository<Ad>, IAddRepository
    {
        public AdRepository(Context context, IMemoryCache memoryCache) : base(context, memoryCache)
        {
            
        }
    }
}
