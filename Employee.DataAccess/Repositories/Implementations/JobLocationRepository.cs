using Employee.DataAccess.Data;
using Employee.DataAccess.Repositories.Interfaces;
using Employee.Entities.Models.Basic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.DataAccess.Repositories.Implementations
{
    public class JobLocationRepository(ApplicationDbContext context)
        : Repository<JobLocation>(context), IJobLocationRepository
    {
        private readonly ApplicationDbContext _context=context;
        public bool JobLocationEists(int jobLocId)
        {
            return _context.JobLocations.Any(c => c.JobLocId == jobLocId);
        }
    }
}
