using Employee.Entities.Models.TimeSheet;

namespace Employee.DataAccess.Repositories.Interfaces
{
    public interface IMonthlyTimeSheetRepository : IRepository<MonthlyTimeSheet>
    {
        Task<MonthlyTimeSheet?> GetAsync(int empId, int year, int month);
        Task<MonthlyTimeSheet?> GetByIdAsync(long id);
        /// <summary>در صورت نبود، پیش‌نویس جدید می‌سازد و ذخیره می‌کند</summary>
        Task<MonthlyTimeSheet> GetOrCreateAsync(int empId, int year, int month);
    }
}
