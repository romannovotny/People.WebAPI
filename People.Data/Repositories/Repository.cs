using Microsoft.EntityFrameworkCore;
using People.Data.Interfaces;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace People.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected PeopleContext Context { get; set; }

        public Repository(PeopleContext context)
        {
            this.Context = context;
        }

        public IQueryable<T> FindAll()
        {
            return Context.Set<T>().AsNoTracking();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return Context.Set<T>().Where(expression).AsNoTracking();
        }

        public async Task<T> CreateAsync(T entity)
        {
            await Context.Set<T>().AddAsync(entity);

            await Context.SaveChangesAsync();

            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            Context.Set<T>().Update(entity);
            await Context.SaveChangesAsync();
            return entity;
        }

        public async Task<T> DeleteAsync(T entity)
        {
            Context.Set<T>().Remove(entity);
            await Context.SaveChangesAsync();
            return entity;
        }
    }
}