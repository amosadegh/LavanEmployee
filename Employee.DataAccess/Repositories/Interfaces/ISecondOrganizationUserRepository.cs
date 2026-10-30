// Employee.DataAccess.Repositories.Interfaces/ISecondOrganizationUserRepository.cs
using Employee.Entities.Models;

namespace Employee.DataAccess.Repositories.Interfaces
{
    public interface ISecondOrganizationUserRepository : IRepository<SecondOrganizationUser>
    {
        Task<List<SecondOrganizationUser>> GetByUserIdAsync(string userId);
        Task<bool> ExistsAsync(string userId, int secondOrganizationId, string role);
    }
}
