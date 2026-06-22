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
    public class AdministrationRepository(ApplicationDbContext context)
    : Repository<Administration>(context), IAdministrationRepository
    {
        private readonly ApplicationDbContext _context = context;

        public bool AdministrationExists(int id)
        {
            return _context.Administrations.Any(a => a.AdministrationId == id);
        }
    }

}
