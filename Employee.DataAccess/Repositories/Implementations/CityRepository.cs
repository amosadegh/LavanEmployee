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
    public class CityRepository(ApplicationDbContext context)
        :Repository<City>(context),ICityRepository
    {
        private readonly ApplicationDbContext _context=context;

        public bool CityExists(int Id)
        {
            return _context.Cities.Any(c => c.Id == Id);
        }
    }
}
