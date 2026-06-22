using Employee.Entities.Models.Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.DataAccess.Repositories.Interfaces
{
    public interface IOrganizationRepository:IRepository<Organization>
    {
        bool OrganizationExists(int id);
    }
}
