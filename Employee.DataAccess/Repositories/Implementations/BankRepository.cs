using Employee.DataAccess.Data;
using Employee.DataAccess.Repositories.Interfaces;
using Employee.Entities.Models.Basic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.DataAccess.Repositories.Implementations
{
    public class BankRepository(ApplicationDbContext context)
        :Repository<Bank>(context),IBankRepository
    {
        private readonly ApplicationDbContext _context = context;
        public bool BankExists(int id)
        {
            return _context.Banks.Any(c => c.BankId == id);
        }
    }
   
}
