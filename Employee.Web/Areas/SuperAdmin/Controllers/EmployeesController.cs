using Employee.DataAccess.Repositories.Implementations;
using Employee.DataAccess.Repositories.Interfaces;
using Employee.DataAccess.Repository;
using Employee.Entities.Models.Basic;
using Employee.Entities.Models.Personel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Employee.Areas.Admin.Controllers
{
    [Area("SuperAdmin")]
    //[Authorize(Roles = "Admin")]
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
        private readonly IRepository<Province> _provinceRepo;
        private readonly IRepository<StudyField> _studyFieldRepo;
        private readonly IRepository<StudyLevel> _studyLevelRepo;
        private readonly UserManager<IdentityUser> _userManager;


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
            IRepository<Province> provinceRepo,
            IRepository<StudyField> studyFieldRepo,
            IRepository<StudyLevel> studyLevelRepo,
            UserManager<IdentityUser> userManager)
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


        //// GET: GetAll (AJAX)

        //[HttpGet]
        //public async Task<IActionResult> GetAll()
        //{
        //    var employees = await _employeeRepo.GetAsync(
        //        include: query => query
        //            .Include(e => e.Administration)
        //            .Include(e => e.PayBankNavigation)
        //            .Include(e => e.Camp)
        //            .Include(e => e.HomeCityNavigation)
        //            .Include(e => e.ContractType)
        //            .Include(e => e.EduOrientation)
        //            .Include(e => e.JobLocation)
        //            .Include(e => e.Job)
        //            .Include(e => e.JobType)
        //            .Include(e => e.Organization)
        //            .Include(e => e.StudyLevel)
        //            .Include(e => e.StudyField)
        //            .Include(e => e.HomeProvinceNavigation)
        //    );

        //    return Json(new { data = employees });
        //}


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
                    string tempPassword = "Temp@123456";

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

        private async Task PopulateDropDowns(TblEmployee? employee = null)
        {
            ViewBag.Administrations = new SelectList(
                await _adminRepo.GetAsync(), "AdministrationId", "AdministrationTitle", employee?.AdministrationId);

            ViewBag.Banks = new SelectList(
                await _bankRepo.GetAsync(), "BankId", "BankName", employee?.PayBank);

            ViewBag.Camps = new SelectList(
                await _campRepo.GetAsync(), "CampId", "CampTitle", employee?.CampId);

            ViewBag.Cities = new SelectList(
                await _cityRepo.GetAsync(), "Id", "Name", employee?.HomeCity);

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

            ViewBag.Provinces = new SelectList(
                await _provinceRepo.GetAsync(), "Id", "Name", employee?.HomeProvince);

            ViewBag.StudyFields = new SelectList(
                await _studyFieldRepo.GetAsync(), "StudyFieldId", "StudyFieldTitle", employee?.StudyFieldId);

            ViewBag.StudyLevels = new SelectList(
                await _studyLevelRepo.GetAsync(), "StudyLevelId", "StudyLevelTitle", employee?.StudyLevelId);
        }
    }
}
