using System.Linq.Expressions;
using RealEstate.Domain.Common.Entities;

namespace RealEstate.Domain.Common.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(string id);
        Task<T> CreateAsync(T entity);
        Task UpdateAsync(string id, T entity);
        Task DeleteAsync(string id);
        Task<bool> ExistsAsync(string id);
        Task<long> CountAsync();
    }

    public interface IRepository<T, TKey> : IRepository<T> where T : BaseEntity<TKey>
    {
        Task<T?> GetByIdAsync(TKey id);
        Task UpdateAsync(TKey id, T entity);
        Task DeleteAsync(TKey id);
        Task<bool> ExistsAsync(TKey id);
    }

    public interface IQueryableRepository<T> : IRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T?> FindOneAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetPagedAsync(int skip, int take);
        Task<IEnumerable<T>> GetPagedAsync(Expression<Func<T, bool>> predicate, int skip, int take);
        Task<long> CountAsync(Expression<Func<T, bool>> predicate);
    }

    public interface IAsyncQueryableRepository<T> : IQueryableRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<TResult>> ProjectAsync<TResult>(Expression<Func<T, TResult>> selector);
        Task<IEnumerable<TResult>> ProjectAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task<T> FirstAsync(Expression<Func<T, bool>> predicate);
    }
}