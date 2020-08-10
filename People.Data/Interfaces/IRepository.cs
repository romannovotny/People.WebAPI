using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace People.Data.Interfaces

{
    public interface IRepository<T>
    {
        IQueryable<T> FindAll();

        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);

        Task<T> CreateAsync(T entity);

        Task<T> UpdateAsync(T entity);

        Task<T> DeleteAsync(T entity);
    }
}