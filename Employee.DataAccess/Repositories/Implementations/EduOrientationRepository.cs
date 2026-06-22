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
    public class EduOrientationRepository(ApplicationDbContext context)
        : Repository<EduOrientation>(context), IEduOrientationRepository
    {
        private readonly ApplicationDbContext _context=context;
        public bool EduOrientationExists(int id)
        {
            return _context.EduOrientations.Any(c => c.EduOrientationId == id);
        }
    }
}
