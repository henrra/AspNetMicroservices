using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Ordering.Application.Contracts.Persistence;
using Ordering.Domain.Common;
using Ordering.Infrastructure.Persistence;

namespace Ordering.Infrastructure.Repositories
{
    public abstract class RepositoryBase<T> : IAsyncRepository<T> where T : EntityBase
    {
        protected readonly OrderContext OrderDatabase;
    
        protected RepositoryBase(OrderContext dbContext)
        {
            OrderDatabase = dbContext;
        }
    
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await GetAsyncQuery(null, null, string.Empty, false)
                .ToListAsync();
        }
    
        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await GetAsyncQuery(predicate, null, string.Empty, false)
                .ToListAsync();
        }
    
        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy)
        {
            return await GetAsyncQuery(predicate, orderBy, string.Empty, false)
                .ToListAsync();
        }
    
        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, string includeString)
        {
            return await GetAsyncQuery(predicate, orderBy, includeString, false)
                .ToListAsync();
        }
    
        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
            string includeString, bool disableTracking)
        {
            return await GetAsyncQuery(predicate,
                    orderBy,
                    includeString,
                    disableTracking)
                .ToListAsync();
        }
    
        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
            List<Expression<Func<T, object>>> includes)
        {
            return await GetAsyncQuery(predicate, orderBy, includes, false).ToListAsync();
        }
    
        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, List<Expression<Func<T, object>>> includes,
            bool disableTracking)
        {
            return await GetAsyncQuery(predicate, orderBy, includes, disableTracking).ToListAsync();
        }
    
        public async Task<T> GetByIdAsync(int id)
        {
            return await OrderDatabase.Table<T>().FindAsync(id);
        }
    
        public async Task<T> AddAsync(T entity)
        {
            EntityEntry<T> added = await OrderDatabase.Table<T>().AddAsync(entity);
            await OrderDatabase.SaveChangesAsync();
            return added.Entity;
        }
    
        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            OrderDatabase.Table<T>().AddRange(entities);
            await OrderDatabase.SaveChangesAsync();
        }
    
        public async Task UpdateAsync(T entity)
        {
            OrderDatabase.Entry(entity).State = EntityState.Modified;
            await OrderDatabase.SaveChangesAsync();
        }
    
        public async Task DeleteAsync(T entity)
        {
            OrderDatabase.Table<T>().Remove(entity);
            await OrderDatabase.SaveChangesAsync();
        }
    
        private IQueryable<T> GetAsyncQuery(Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
            string includeString, bool disableTracking)
        {
            IQueryable<T> query = OrderDatabase.Table<T>();
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
    
            if (!string.IsNullOrEmpty(includeString))
            {
                query = query.Include(includeString);
            }
    
            if (orderBy != null)
            {
                query = orderBy(query);
            }
    
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }
    
            return query;
        }
    
        private IQueryable<T> GetAsyncQuery(Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, 
            IReadOnlyCollection<Expression<Func<T, object>>> includes,
            bool disableTracking)
        {
            IQueryable<T> query = OrderDatabase.Table<T>();
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
    
            if (orderBy != null)
            {
                query = orderBy(query);
            }
    
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
    
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }
    
            return query;
        }
    }
}