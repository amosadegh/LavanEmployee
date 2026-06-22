using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Employee.DataAccess.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAsync(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IQueryable<T>>? include = null,
            int? skip = null,
            int? take = null);

        Task<T?> GetSingleAsync(
           Expression<Func<T, bool>> filter,
           Func<IQueryable<T>, IQueryable<T>>? include = null);

        Task AddAsync(T entity);

        void Update(T entity);

        void Remove(T entity);

        Task<int> SaveChangesAsync();
    }
}
