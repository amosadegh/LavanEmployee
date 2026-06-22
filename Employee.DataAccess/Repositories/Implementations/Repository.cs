using Employee.DataAccess.Data;
using Employee.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Employee.DataAccess.Repositories.Implementations
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;
        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public async Task<List<T>> GetAsync(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        Func<IQueryable<T>, IQueryable<T>>? include = null,
        int? skip = null,
        int? take = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (filter != null)
                query = query.Where(filter);

            if (include != null)
                query = include(query);

            if (orderBy != null)
                query = orderBy(query);

            if (skip.HasValue)
                query = query.Skip(skip.Value);

            if (take.HasValue)
                query = query.Take(take.Value);

            return await query.ToListAsync();
        }
        //-------------Simple Include--------------
        //var data = await _repo.GetAsync(
        //include: q => q.Include(x => x.EduOrientations) );
        //----------------Example-------------------------
        //    var data = await _repo.GetAsync(
        //    filter: x => x.StudyFieldTitle.Contains("مهندسی"),
        //    include: q => q.Include(x => x.EduOrientations),
        //    orderBy: q => q.OrderBy(x => x.StudyFieldTitle),
        //    skip: 0,
        //    take: 10);
        //--------------Example-------------------
        //    var data = await _repo.GetAsync(
        //    include: q => q
        //    .Include(x => x.A)
        //    .ThenInclude(a => a.B));
        //-------------Example---------------------
        //    var data = await _repo.GetAsync(
        //    orderBy: q => q.OrderBy(x => x.StudyFieldTitle));
        //------------Filter_Include---------------
        //var data = await _repo.GetAsync(
        //filter: x => x.StudyFieldId == 1,
        //include: q => q.Include(x => x.EduOrientations));
        //---------------Include + thenInclude---------
        //var data = await _repo.GetAsync(
        // include: q => q
        //.Include(x => x.A)
        //.ThenInclude(a => a.B));
        public async Task<T?> GetSingleAsync(
          Expression<Func<T, bool>> filter,
          Func<IQueryable<T>, IQueryable<T>>? include = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (include != null)
                query = include(query);

            return await query.FirstOrDefaultAsync(filter);
        }
        ////----------Example---------------
        //var data = await _repo.GetSingleAsync(
        //    x => x.StudyFieldId == 1,
        //    q => q.Include(x => x.EduOrientations));
        // public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);
        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate) => await _dbSet.Where(predicate).ToListAsync();

        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);
        public void Update(T entity) => _dbSet.Update(entity);
        public void Remove(T entity) => _dbSet.Remove(entity);
        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

    }

}
