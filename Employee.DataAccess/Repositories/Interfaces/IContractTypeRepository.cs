using Employee.Entities.Models.Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.DataAccess.Repositories.Interfaces
{
    public interface IContractTypeRepository:IRepository<ContractType>
    {
        bool ContractTypeExists(int contractTypeID);
    }
}
