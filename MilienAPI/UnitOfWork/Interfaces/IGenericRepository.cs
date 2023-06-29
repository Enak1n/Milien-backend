namespace MilienAPI.UnitOfWork.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<List<T>> GetAll();
        Task<T> GetById(int id);
        Task<T> Add(T entity);
        Task<T> Edit(T entity, List<string> urls);
        Task Delete(int id);
    }
}
