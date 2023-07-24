using Microsoft.Extensions.Caching.Memory;
using ServiceAPI.Data;
using ServiceAPI.Models;
using ServiceAPI.UnitOfWork.Interfaces;

namespace ServiceAPI.UnitOfWork
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(Context context, IMemoryCache memoryCache) : base(context, memoryCache)
        {

        }
    }
}
