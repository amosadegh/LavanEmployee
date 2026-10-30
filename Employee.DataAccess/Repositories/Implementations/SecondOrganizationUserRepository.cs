// Employee.DataAccess.Repositories.Implementations/SecondOrganizationUserRepository.cs
using Employee.DataAccess.Data;
using Employee.DataAccess.Repositories.Interfaces;
using Employee.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Employee.DataAccess.Repositories.Implementations
{
    public class SecondOrganizationUserRepository(ApplicationDbContext context)
        : Repository<SecondOrganizationUser>(context), ISecondOrganizationUserRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<List<SecondOrganizationUser>> GetByUserIdAsync(string userId)
        {
            return await _context.SecondOrganizationUsers
                .Include(x => x.SecondOrganization)
                .Where(x => x.UserId == userId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(string userId, int secondOrganizationId, string role)
        {
            return await _context.SecondOrganizationUsers.AnyAsync(x =>
                x.UserId == userId &&
                x.SecondOrganizationId == secondOrganizationId &&
                x.Role == role);
        }
    }
}
