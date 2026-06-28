using Employee.DataAccess.Data;
using Employee.DataAccess.Repositories.Implementations;
using Employee.DataAccess.Repositories.Interfaces;
using Employee.Entities.Models.Personel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Employee.DataAccess.Repository
{
    public class EmployeeRepository : Repository<TblEmployee>, IEmployeeRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public EmployeeRepository(ApplicationDbContext db, UserManager<IdentityUser> userManager)
            : base(db)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<TblEmployee> AddEmployeeWithUserAsync(TblEmployee employee, string password)
        {
            // 1. ذخیره کارمند
            _db.Employees.Add(employee);
            await _db.SaveChangesAsync();

            // 2. ساخت User با UserName = EmployeeNumber
            var user = new IdentityUser
            {
                UserName = employee.EmployeeNumber,
                Email = employee.Mobile + "@company.local" // اگر Email اجباری است
            };

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                // اگر User نساخته شد، Employee را پاک کن
                _db.Employees.Remove(employee);
                await _db.SaveChangesAsync();

                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"خطا در ساخت کاربر: {errors}");
            }

            // 3. اضافه کردن به نقش User
            var roleResult = await _userManager.AddToRoleAsync(user, "User");

            if (!roleResult.Succeeded)
            {
                // حذف User و Employee
                await _userManager.DeleteAsync(user);
                _db.Employees.Remove(employee);
                await _db.SaveChangesAsync();

                var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                throw new Exception($"خطا در تخصیص نقش: {errors}");
            }

            return employee;
        }
public async Task<TblEmployee?> GetByIdAsync(int id)
        {
            return await _db.Employees
                .Include(e => e.Administration)
                .Include(e => e.Job)
                .Include(e => e.Organization)
                .Include(e => e.ContractType)
                .Include(e => e.PayBankNavigation)
                .Include(e => e.PayBackupBankNavigation)
                .Include(e => e.HomeCityNavigation)
                .Include(e => e.BirthCityNavigation)
                .Include(e => e.EntryCityNavigation)
                .Include(e => e.BirthProvince)
                .Include(e => e.HomeProvince)
                .Include(e => e.EntryProviance)
                .Include(e => e.StudyLevel)
                .Include(e => e.StudyField)
                .FirstOrDefaultAsync(e => e.EmpId == id);
        }

    }

}
