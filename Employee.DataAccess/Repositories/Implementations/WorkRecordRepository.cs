using Employee.DataAccess.Data;
using Employee.DataAccess.Repositories.Interfaces;
using Employee.Entities.Models.TimeSheet;
using Microsoft.EntityFrameworkCore;

namespace Employee.DataAccess.Repositories.Implementations
{
    public class WorkRecordRepository(ApplicationDbContext context)
        : Repository<WorkRecord>(context), IWorkRecordRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<List<WorkRecord>> GetByEmployeeInRangeAsync(int empId, DateOnly from, DateOnly to)
        {
            return await _context.WorkRecords
                .Include(x => x.Employee)
                .Where(x => x.EmpId == empId && x.FromDate <= to && x.ToDate >= from)
                .OrderBy(x => x.FromDate)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> HasOverlapAsync(int empId, DateOnly from, DateOnly to, long? excludeId = null)
        {
            return await _context.WorkRecords.AnyAsync(x =>
                x.EmpId == empId &&
                x.FromDate <= to && x.ToDate >= from &&
                (excludeId == null || x.WorkRecordId != excludeId));
        }

        public async Task<WorkRecord?> GetByIdAsync(long id)
        {
            return await _context.WorkRecords
                .Include(x => x.Employee)
                .FirstOrDefaultAsync(x => x.WorkRecordId == id);
        }
    }
}
