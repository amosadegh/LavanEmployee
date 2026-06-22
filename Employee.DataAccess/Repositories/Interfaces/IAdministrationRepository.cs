using Employee.DataAccess.Repositories.Interfaces;
using Employee.Entities.Models.Basic;

namespace Employee.DataAccess.Repositories.Implementations
{
    public interface IAdministrationRepository:IRepository<Administration>
    {
        bool AdministrationExists(int id);
    }
}