using Employee.DataAccess.Data;
using Employee.DataAccess.Repositories.Interfaces;
using Employee.Entities.Models.Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.DataAccess.Repositories.Implementations
{
    public class JobTypeRepository(ApplicationDbContext context)
        : Repository<JobType>(context), IJobTypeRepository
    {
        private readonly ApplicationDbContext _context = context;
        public bool JobTypeExists(int id)
        {
            return _context.JobTypes.Any(c => c.JobTypeId == id);
        }
    }
}
