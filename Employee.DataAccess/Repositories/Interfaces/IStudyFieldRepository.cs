using Employee.Entities.Models.Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.DataAccess.Repositories.Interfaces
{
    public interface IStudyFieldRepository:IRepository<StudyField>
    {
        bool StudyFieldExists(int id);
    }
}
