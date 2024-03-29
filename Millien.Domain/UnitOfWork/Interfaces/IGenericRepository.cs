﻿using Millien.Domain.Entities;
using System.Linq.Expressions;

namespace Millien.Domain.UnitOfWork.Interfaces
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
        Task<List<T>> FindRange(Expression<Func<T, bool>> predicate);
        Task<T> Find(Expression<Func<T, bool>> predicate);
    }
}
