using Employee.Entities.Models.TimeSheet;

namespace Employee.DataAccess.Repositories.Interfaces
{
    public interface IWorkRecordRepository : IRepository<WorkRecord>
    {
        /// <summary>رکوردهایی از کارمند که با بازه (میلادی) تداخل دارند</summary>
        Task<List<WorkRecord>> GetByEmployeeInRangeAsync(int empId, DateOnly from, DateOnly to);

        Task<bool> HasOverlapAsync(int empId, DateOnly from, DateOnly to, long? excludeId = null);

        Task<WorkRecord?> GetByIdAsync(long id);
    }
}
