using System.Linq.Expressions;

namespace MilienAPI.UnitOfWork.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<List<T>> GetAll();
        Task<T> GetById(int id);
        Task Add(T entity);
        Task AddRange(T entity);
        Task Edit(T entity);
        Task Remove(T entity);
        Task RemoveRange(List<T> entities);
        Task<List<T>> Find(Expression<Func<T, bool>> predicate);
    }
}
