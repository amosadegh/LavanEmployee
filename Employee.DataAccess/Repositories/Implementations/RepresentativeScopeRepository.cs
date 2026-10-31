using Employee.DataAccess.Data;
using Employee.DataAccess.Repositories.Interfaces;
using Employee.Entities.Models;
using Employee.Entities.Models.Personel;
using Microsoft.EntityFrameworkCore;

namespace Employee.DataAccess.Repositories.Implementations
{
    public class RepresentativeScopeRepository(ApplicationDbContext context)
        : Repository<TblEmployee>(context), IRepresentativeScopeRepository
    {
        private readonly ApplicationDbContext _context = context;

        private IQueryable<TblEmployee> Scope(string userId) =>
            _context.Set<TblEmployee>().AsNoTracking().Where(e =>
                e.SecondOrganizationId != null &&
                _context.Set<SecondOrganizationUser>().Any(s =>
                    s.UserId == userId &&
                    s.Role == "Representative" &&
                    s.SecondOrganizationId == e.SecondOrganizationId));

        public async Task<List<TblEmployee>> GetEmployeesAsync(string representativeUserId)
        {
            return await Scope(representativeUserId).OrderBy(e => e.EmpId).ToListAsync();
        }

        public async Task<TblEmployee?> GetAccessibleEmployeeAsync(string representativeUserId, int empId)
        {
            return await Scope(representativeUserId).FirstOrDefaultAsync(e => e.EmpId == empId);
        }
    }
}
