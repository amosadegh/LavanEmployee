using Employee.DataAccess.Data;
using Employee.DataAccess.Repositories.Interfaces;
using Employee.Entities.Models.Basic;

namespace Employee.DataAccess.Repositories.Implementations
{
    public class JobRepository(ApplicationDbContext context)
        : Repository<Job>(context), IJobRepository
    {
        private readonly ApplicationDbContext _context = context;
        public bool JobExists(int id)
        {
            return _context.Jobs.Any(c => c.JobId == id);
        }
    }
}
