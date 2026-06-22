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
    public class StudyFieldRepository(ApplicationDbContext context)
       :Repository<StudyField>(context),IStudyFieldRepository
    {
        private readonly ApplicationDbContext _context=context;

        public bool StudyFieldExists(int id)
        {
            return _context.StudyFields.Any(c => c.StudyFieldId == id);
        }
    }
}
