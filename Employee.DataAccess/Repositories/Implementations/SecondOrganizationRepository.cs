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
    public class SecondOrganizationRepository(ApplicationDbContext context)
        :Repository<SecondOrganization>(context),ISecondOrganizationRepository
    {
        private readonly ApplicationDbContext _context=context;

        public bool SecondOrganizationExists(int id)
        {
            return _context.SecondOrganizations.Any(s=>s.OrganizationId==id);
        }
    }
}
