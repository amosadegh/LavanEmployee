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
    public class ProvinceRepository(ApplicationDbContext context)
        :Repository<Province>(context),IProvinceRepository
    {
        private readonly ApplicationDbContext _context=context;

        public bool ProvinceExists(int id)
        {
           return _context.Provinces.Any(p=>p.Id == id);
        }
    }
}
