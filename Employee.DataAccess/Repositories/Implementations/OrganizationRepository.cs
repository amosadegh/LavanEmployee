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
    public class OrganizationRepository(ApplicationDbContext context):
        Repository<Organization>(context),IOrganizationRepository
    {
        private readonly ApplicationDbContext _context=context;

        public bool OrganizationExists(int id)
        {
           return _context.Organizations.Any(o=>o.OrganizationId==id);
        }
    }
}
