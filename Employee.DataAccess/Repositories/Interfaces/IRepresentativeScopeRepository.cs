using Employee.Entities.Models.Personel;

namespace Employee.DataAccess.Repositories.Interfaces
{
    public interface IRepresentativeScopeRepository : IRepository<TblEmployee>
    {
        /// <summary>کارمندان واحدهای سازمانی‌ای که کاربر در آن‌ها نماینده است</summary>
        Task<List<TblEmployee>> GetEmployeesAsync(string representativeUserId);

        /// <summary>کارمند در محدوده‌ی نماینده (برای اعتبارسنجی سمت سرور)؛ در غیر این صورت null</summary>
        Task<TblEmployee?> GetAccessibleEmployeeAsync(string representativeUserId, int empId);
    }
}
