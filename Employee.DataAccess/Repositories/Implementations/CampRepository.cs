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
    public class CampRepository(ApplicationDbContext context)
       : Repository<Camp>(context), ICampRepository
    {
        private readonly ApplicationDbContext _context = context;
        public bool CampExists(int id)
        {
            return _context.Camps.Any(c => c.CampId == id);
        }
    }
}
