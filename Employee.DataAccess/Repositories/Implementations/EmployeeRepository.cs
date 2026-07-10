using Employee.DataAccess.Data;
using Employee.DataAccess.Repositories.Implementations;
using Employee.DataAccess.Repositories.Interfaces;
using Employee.Entities.Models.Personel;
using Employee.Entities.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Employee.DataAccess.Repository
{
    public class EmployeeRepository : Repository<TblEmployee>, IEmployeeRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private const string DefaultRole = "User";

        public EmployeeRepository(
            ApplicationDbContext db,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
            : base(db)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<TblEmployee> AddEmployeeWithUserAsync(TblEmployee employee, string password)
        {
            await using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                // 1. چک وجود کاربر با همین EmployeeNumber
                var existingUser = await _userManager.FindByNameAsync(employee.EmployeeNumber);
                if (existingUser != null)
                {
                    // کاربر قبلاً وجود داره — فقط UserId رو ست کن
                    employee.UserId = existingUser.Id;
                }
                else
                {
                    // 2. ساخت کاربر جدید
                    var user = new ApplicationUser
                    {
                        UserName = employee.EmployeeNumber,
                        Email = $"{employee.EmployeeNumber}@local.com",
                        EmailConfirmed = true,
                        SecurityStamp = Guid.NewGuid().ToString()
                    };

                    var createResult = await _userManager.CreateAsync(user, password);
                    if (!createResult.Succeeded)
                    {
                        var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                        throw new Exception($"خطا در ساخت کاربر: {errors}");
                    }

                    // 3. اطمینان از وجود نقش و اختصاص به کاربر
                    if (!await _roleManager.RoleExistsAsync(DefaultRole))
                        await _roleManager.CreateAsync(new IdentityRole(DefaultRole));

                    var roleResult = await _userManager.AddToRoleAsync(user, DefaultRole);
                    if (!roleResult.Succeeded)
                    {
                        var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                        throw new Exception($"خطا در تخصیص نقش: {errors}");
                    }

                    employee.UserId = user.Id;
                }

                // 4. ذخیره کارمند (بعد از داشتن UserId)
                _db.Employees.Add(employee);
                await _db.SaveChangesAsync();

                await transaction.CommitAsync();
                return employee;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<TblEmployee?> GetByIdAsync(int id)
        {
            return await _db.Employees
                .Include(e => e.Administration)
                .Include(e => e.Job)
                .Include(e => e.JobType)
                .Include(e => e.JobLocation)
                .Include(e => e.Organization)
                .Include(e => e.SecondOrganization)
                .Include(e => e.ContractType)
                .Include(e => e.EduOrientation)
                .Include(e => e.StudyLevel)
                .Include(e => e.StudyField)
                .Include(e => e.PayBankNavigation)
                .Include(e => e.PayBackupBankNavigation)
                .Include(e => e.Camp)
                .Include(e => e.HomeCityNavigation)
                .Include(e => e.BirthCityNavigation)
                .Include(e => e.EntryCityNavigation)
                .Include(e => e.BirthProvinceNavigation)
                .Include(e => e.HomeProvinceNavigation)
                .Include(e => e.EntryProvinceNavigation)
                .FirstOrDefaultAsync(e => e.EmpId == id);
        }
    }
}
