using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MilienAPI.DataBase;
using MilienAPI.Models;
using MilienAPI.UnitOfWork.Interfaces;

namespace MilienAPI.UnitOfWork
{
    public class AdRepository : GenericRepository<Ad>, IAddRepository
    {
        public AdRepository(Context context, IMemoryCache memoryCache) : base(context, memoryCache)
        {
            
        }
    }
}
