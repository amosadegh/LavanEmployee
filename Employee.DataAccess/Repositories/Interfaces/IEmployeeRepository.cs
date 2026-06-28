using Employee.Entities.Models.Personel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Employee.DataAccess.Repositories.Interfaces
{
    public interface IEmployeeRepository : IRepository<TblEmployee>
    {
        Task<TblEmployee> AddEmployeeWithUserAsync(TblEmployee employee, string password);
        Task<TblEmployee?> GetByIdAsync(int id);

    }
}

