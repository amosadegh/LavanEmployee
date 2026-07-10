using Employee.DataAccess.Repositories.Implementations;
using Employee.DataAccess.Repositories.Interfaces;
using Employee.DataAccess.Repository;
using Employee.Entities.Models.Basic;
using Employee.Entities.Models.Personel;
using Employee.Entities.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Employee.Areas.Admin.Controllers
{
    [Area("SuperAdmin")]
    [Authorize(Roles = "SuperAdmin")]
    public class EmployeesController : Controller
    {
        private readonly IEmployeeRepository _employeeRepo;
        private readonly IRepository<Administration> _adminRepo;
        private readonly IRepository<Bank> _bankRepo;
        private readonly IRepository<Camp> _campRepo;
        private readonly IRepository<City> _cityRepo;
        private readonly IRepository<ContractType> _contractRepo;
        private readonly IRepository<EduOrientation> _eduOrientationRepo;
        private readonly IRepository<Job> _jobRepo;
        private readonly IRepository<JobLocation> _jobLocRepo;
        private readonly IRepository<JobType> _jobTypeRepo;
        private readonly IRepository<Organization> _orgRepo;
        private readonly IRepository<SecondOrganization> _secOrgRepo;
        private readonly IRepository<Province> _provinceRepo;
        private readonly IRepository<StudyField> _studyFieldRepo;
        private readonly IRepository<StudyLevel> _studyLevelRepo;
        private readonly UserManager<ApplicationUser> _userManager;


        public EmployeesController(
            IEmployeeRepository employeeRepo,
            IRepository<Administration> adminRepo,
            IRepository<Bank> bankRepo,
            IRepository<Camp> campRepo,
            IRepository<City> cityRepo,
            IRepository<ContractType> contractRepo,
            IRepository<EduOrientation> eduOrientationRepo,
            IRepository<Job> jobRepo,
            IRepository<JobLocation> jobLocRepo,
            IRepository<JobType> jobTypeRepo,
            IRepository<Organization> orgRepo,
            IRepository<SecondOrganization> secOrgRepo,
            IRepository<Province> provinceRepo,
            IRepository<StudyField> studyFieldRepo,
            IRepository<StudyLevel> studyLevelRepo,
            UserManager<ApplicationUser> userManager)
        {
            _employeeRepo = employeeRepo;
            _adminRepo = adminRepo;
            _bankRepo = bankRepo;
            _campRepo = campRepo;
            _cityRepo = cityRepo;
            _contractRepo = contractRepo;
            _eduOrientationRepo = eduOrientationRepo;
            _jobRepo = jobRepo;
            _jobLocRepo = jobLocRepo;
            _jobTypeRepo = jobTypeRepo;
            _orgRepo = orgRepo;
            _secOrgRepo = secOrgRepo;
            _provinceRepo = provinceRepo;
            _studyFieldRepo = studyFieldRepo;
            _studyLevelRepo = studyLevelRepo;
            _userManager = userManager;
        }

        // GET: Index
        public async Task<IActionResult> Index()
        {
            var employees = await _employeeRepo.GetAsync(
                include: query => query
                    .Include(e => e.ApplicationUser)
                    .Include(e => e.Administration)
                    .Include(e => e.PayBankNavigation)
                    .Include(e => e.PayBackupBankNavigation)
                    .Include(e => e.Camp)
                    .Include(e => e.HomeCityNavigation)
                    .Include(e => e.BirthCityNavigation)
                    .Include(e => e.EntryCityNavigation)
                    .Include(e => e.ContractType)
                    .Include(e => e.EduOrientation)
                    .Include(e => e.JobLocation)
                    .Include(e => e.Job)
                    .Include(e => e.JobType)
                    .Include(e => e.Organization)
            );

            return View(employees ?? Enumerable.Empty<TblEmployee>());
        }


        // GET: Create
        public async Task<IActionResult> Create()
        {
            await PopulateDropDowns();
            return View();
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TblEmployee employee)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // پسورد پیش‌فرض را بعد از ذخیره Employee تنظیم می‌کنیم
                    // پس اینجا یک پسورد موقت بدهید
                    string tempPassword = employee.EmployeeNumber;

                    var result = await _employeeRepo.AddEmployeeWithUserAsync(employee, tempPassword);

                    // حالا EmpId موجود است، پسورد را آپدیت کنید
                    var user = await _userManager.FindByNameAsync(result.EmployeeNumber);
                    if (user != null)
                    {
                        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                        await _userManager.ResetPasswordAsync(user, token, result.EmpId.ToString());
                    }

                    TempData["success"] = "کارمند با موفقیت اضافه شد";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["error"] = $"خطا: {ex.Message}";
                }
            }

            // بارگذاری DropDowns در صورت خطا
            await PopulateDropDowns(employee);
            return View(employee);
        }

        // GET: Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _employeeRepo.GetSingleAsync(c => c.EmpId == id);
            if (employee == null)
            {
                return NotFound();
            }
            await PopulateDropDowns();
            return View(employee);

        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TblEmployee employee)
        {
            if (ModelState.IsValid)
            {
                _employeeRepo.Update(employee);
                await _employeeRepo.SaveChangesAsync();
                TempData["success"] = "کارمند با موفقیت ویرایش شد";
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropDowns(employee);
            return View(employee);
        }

        // POST: Delete (AJAX)
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _employeeRepo.GetByIdAsync(id);
            if (employee == null)
                return Json(new { success = false, message = "کارمند یافت نشد" });

            _employeeRepo.Remove(employee);
            await _employeeRepo.SaveChangesAsync();
            return Json(new { success = true, message = "کارمند حذف شد" });
        }

        [HttpGet]
        public async Task<IActionResult> GetSecondOrganizations(int organizationId)
        {
            if (organizationId <= 0)
                return Json(Array.Empty<object>());

            var secondOrganizations = await _secOrgRepo.GetAsync(
                x => x.OrganizationId == organizationId);

            var result = secondOrganizations.Select(x => new
            {
                id = x.SecondOrganizationId,
                title = x.SecondOrganizationTitle
            });

            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetCities(int provinceId)
        {
            if (provinceId <= 0)
                return Json(Array.Empty<object>());

            var cities = await _cityRepo.GetAsync(x => x.ProvinceId == provinceId);
            var result = cities.Select(x => new { id = x.Id, text = x.Name });
            return Json(result);
        }


        [HttpGet]
        public async Task<IActionResult> GetEduOrientations(int? studyFieldId)
        {
            if (studyFieldId == null)
                return Json(Array.Empty<object>());

            var orientations = await _eduOrientationRepo.GetAsync(
                x => x.StudyFieldId == studyFieldId);

            return Json(orientations.Select(o => new
            {
                id = o.EduOrientationId,
                title = o.EduOrientationTitle
            }));
        }

        private async Task PopulateDropDowns(TblEmployee? employee = null)
        {
            ViewBag.Administrations = new SelectList(
                await _adminRepo.GetAsync(), "AdministrationId", "AdministrationTitle", employee?.AdministrationId);

            ViewBag.Banks = new SelectList(
                await _bankRepo.GetAsync(), "BankId", "BankTitle");

            ViewBag.Camps = new SelectList(
                await _campRepo.GetAsync(), "CampId", "CampTitle", employee?.CampId);

            ViewBag.Provinces = new SelectList(
               await _provinceRepo.GetAsync(), "Id", "Name");

            ViewBag.Cities = new SelectList(
                await _cityRepo.GetAsync(), "Id", "Name");

            ViewBag.ContractTypes = new SelectList(
                await _contractRepo.GetAsync(), "ContractTypeId", "ContractTypeTitle", employee?.ContractTypeId);

            ViewBag.EduOrientations = new SelectList(
                await _eduOrientationRepo.GetAsync(), "EduOrientationId", "EduOrientationTitle", employee?.EduOrientationId);

            ViewBag.Jobs = new SelectList(
                await _jobRepo.GetAsync(), "JobId", "JobTitle", employee?.JobId);

            ViewBag.JobLocations = new SelectList(
                await _jobLocRepo.GetAsync(), "JobLocId", "JobLocTitle", employee?.JobLocId);

            ViewBag.JobTypes = new SelectList(
                await _jobTypeRepo.GetAsync(), "JobTypeId", "JobTypeTitle", employee?.JobTypeId);

            ViewBag.Organizations = new SelectList(
                await _orgRepo.GetAsync(), "OrganizationId", "OrganizationTitle", employee?.OrganizationId);

            var secondOrgs = employee?.OrganizationId is int orgId
                ? await _secOrgRepo.GetAsync(x => x.OrganizationId == orgId)
                : Enumerable.Empty<SecondOrganization>();

            ViewBag.SecondOrganizations = new SelectList(
                secondOrgs,
                "SecondOrganizationId",
                "SecondOrganizationTitle",
                employee?.SecondOrganizationId);

            ViewBag.StudyFields = new SelectList(
                await _studyFieldRepo.GetAsync(), "StudyFieldId", "StudyFieldTitle", employee?.StudyFieldId);

            ViewBag.StudyLevels = new SelectList(
                await _studyLevelRepo.GetAsync(), "StudyLevelId", "StudyLevelTitle", employee?.StudyLevelId);
        }

    }
}
