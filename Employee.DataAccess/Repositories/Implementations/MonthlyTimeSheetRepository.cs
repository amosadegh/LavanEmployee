using Employee.DataAccess.Data;
using Employee.DataAccess.Repositories.Interfaces;
using Employee.Entities.Models.TimeSheet;
using Microsoft.EntityFrameworkCore;

namespace Employee.DataAccess.Repositories.Implementations
{
    public class MonthlyTimeSheetRepository(ApplicationDbContext context)
        : Repository<MonthlyTimeSheet>(context), IMonthlyTimeSheetRepository
    {
        private readonly ApplicationDbContext _context = context;

        public Task<MonthlyTimeSheet?> GetAsync(int empId, int year, int month) =>
            _context.Set<MonthlyTimeSheet>()
                .FirstOrDefaultAsync(x => x.EmpId == empId && x.Year == year && x.Month == month);

        public Task<MonthlyTimeSheet?> GetByIdAsync(long id) =>
            _context.Set<MonthlyTimeSheet>().FirstOrDefaultAsync(x => x.TimeSheetId == id);

        public async Task<MonthlyTimeSheet> GetOrCreateAsync(int empId, int year, int month)
        {
            var sheet = await GetAsync(empId, year, month);
            if (sheet != null) return sheet;

            sheet = new MonthlyTimeSheet { EmpId = empId, Year = year, Month = month };
            await AddAsync(sheet);
            try { await SaveChangesAsync(); }
            catch (DbUpdateException) // ایجاد هم‌زمان توسط درخواست دیگر
            {
                _context.Entry(sheet).State = EntityState.Detached;
                sheet = await GetAsync(empId, year, month)
                        ?? throw new InvalidOperationException("TimeSheet create failed");
            }
            return sheet;
        }
    }
}
