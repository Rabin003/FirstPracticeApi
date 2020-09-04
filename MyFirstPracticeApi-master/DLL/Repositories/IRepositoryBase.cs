using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DLL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace DLL.Repositories
{
    public interface IRepositoryBase<T> where T : class
    {
        IQueryable<T> QueryAll(Expression<Func<T, bool>> expression = null);
        Task<List<T>> GetList(Expression<Func<T, bool>> expression = null);
        Task CreateAsync(T entry);
        Task CreateAsyncRange(List<T> entryList);

        void update(T entry);
        void updateRange(List<T> entryList);
        void delete(T entry);
        void deleteRange(List<T> entryList);
        Task<T> FindSingleAsync(Expression<Func<T, bool>> expression);
        Task<bool> SaveCompletedAsync();

    }

    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly ApplicationDBContext _context;

        public RepositoryBase(ApplicationDBContext context)
        {
            _context = context;
        }

        public IQueryable<T> QueryAll(Expression<Func<T, bool>> expression = null)
        {
            return expression != null ? _context.Set<T>().AsQueryable().Where(expression).AsTracking() : _context.Set<T>().AsQueryable().AsTracking();
        }

        public async Task<List<T>> GetList(Expression<Func<T, bool>> expression = null)
        {
            return  expression != null ? await _context.Set<T>().AsQueryable().Where(expression).AsTracking().ToListAsync() : 
                await _context.Set<T>().AsQueryable().AsTracking().ToListAsync();
        }

        public async Task CreateAsync(T entry)
        {
            await _context.Set<T>().AddAsync(entry);
        }
        public async Task CreateAsyncRange(List<T> entryList)
        {
            await _context.Set<T>().AddRangeAsync(entryList);
        }

        public void update(T entry)
        {
             _context.Set<T>().Update(entry);
        }

        public void updateRange(List<T> entryList)
        {
            _context.Set<T>().UpdateRange(entryList);
        }

        public void delete(T entry)
        {
            _context.Set<T>().Remove(entry);
        }

        public void deleteRange(List<T> entryList)
        {
            _context.Set<T>().RemoveRange(entryList);
        }

        public async Task<T> FindSingleAsync(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(expression);
        }

        public async Task<bool> SaveCompletedAsync()
        {
            return await _context.SaveChangesAsync() > 0;

        }
    }
}