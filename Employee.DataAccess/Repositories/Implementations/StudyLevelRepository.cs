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
    public class StudyLevelRepository(ApplicationDbContext context):
        Repository<StudyLevel>(context),IStudyLevelRepository
    {
            private readonly ApplicationDbContext _context=context;

        public bool StudyLevelExists(int id)
        {
            return _context.StudyLevels.Any(c => c.StudyLevelId == id);
        }
    }
}
