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
    public class ContractTypeRepository(ApplicationDbContext context) 
        : Repository<ContractType>(context), IContractTypeRepository
    {
        private readonly ApplicationDbContext _context=context;

        public bool ContractTypeExists(int contractTypeID)
        {
           return _context.ContractTypes.Any(c=>c.ContractTypeId== contractTypeID);
        }
    }
    
}
