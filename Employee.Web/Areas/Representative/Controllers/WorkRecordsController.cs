using System.Globalization;
using System.Security.Claims;
using Employee.DataAccess.Repositories.Interfaces;
using Employee.Entities.Models.Enums;
using Employee.Entities.Models.Personel;
using Employee.Entities.Models.TimeSheet;
using Employee.Models.TimeSheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Employee.Web.Helpers;

namespace Employee.Areas.Representative.Controllers
{
    /// نماینده، کارکرد کارمندان واحد سازمانی خودش را ثبت/ویرایش/ارسال می‌کند.
    [Area("Representative")]
    [Authorize(Roles = "Representative")]
    public class WorkRecordsController : Controller
    {
        private static readonly PersianCalendar Pc = new();

        private readonly IWorkRecordRepository _workRepo;
        private readonly IMonthlyTimeSheetRepository _sheetRepo;
        private readonly IRepresentativeScopeRepository _scope;

        public WorkRecordsController(IWorkRecordRepository workRepo,
                                     IMonthlyTimeSheetRepository sheetRepo,
                                     IRepresentativeScopeRepository scope)
        {
            _workRepo = workRepo;
            _sheetRepo = sheetRepo;
            _scope = scope;
        }

        private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        private Task<TblEmployee?> GetEmployeeAsync(int empId) =>
            _scope.GetAccessibleEmployeeAsync(UserId, empId);

        // ───────────── Index ─────────────
        public async Task<IActionResult> Index(int? empId, int? year, int? month)
        {
            var now = DateTime.Now;
            var (y, m) = TimeSheetHelper.NormalizeYearMonth(year, month, now);

            var employees = await _scope.GetEmployeesAsync(UserId);
            ViewBag.Employees = new SelectList(
                employees
                    .OrderBy(e => e.Family).ThenBy(e => e.Name)
                    .Select(e => new { e.EmpId, Title = $"{e.Name} {e.Family} - {e.EmployeeNumber}" }),
                "EmpId", "Title", empId);
            ViewBag.EmpId = empId;
            ViewBag.Year = y;
            ViewBag.Month = m;
            ViewBag.Years = new SelectList(Enumerable.Range(Pc.GetYear(now) - 5, 7).Reverse(), y);
            ViewBag.Months = new SelectList(
                TimeSheetHelper.MonthNames.Select((n, i) => new { Id = i + 1, Name = n }), "Id", "Name", m);

            var records = new List<WorkRecord>();
            ViewBag.Sheet = null;
            ViewBag.SheetEditable = false;
            ViewBag.DayTotals = new Dictionary<WorkStatusType, int>();

            if (empId != null)
            {
                var emp = await GetEmployeeAsync(empId.Value);
                if (emp == null) return Forbid();

                var (start, end) = TimeSheetHelper.GetMonthRange(y, m);
                records = (await _workRepo.GetByEmployeeInRangeAsync(emp.EmpId, start, end)).ToList();
                var sheet = await _sheetRepo.GetAsync(emp.EmpId, y, m);
                ViewBag.Sheet = sheet;
                ViewBag.SheetEditable = sheet?.IsEditable ?? true;
                ViewBag.DayTotals = records
                    .GroupBy(r => r.WorkStatus)
                    .ToDictionary(g => g.Key, g => g.Sum(r => r.GetDaysInRange(start, end)));
            }
            return View(records);
        }

        // ───────────── Create ─────────────
        [HttpGet]
        public async Task<IActionResult> Create(int empId, int? year, int? month)
        {
            var emp = await GetEmployeeAsync(empId);
            if (emp == null) return Forbid();

            var (y, m) = TimeSheetHelper.NormalizeYearMonth(year, month, DateTime.Now);
            var sheet = await _sheetRepo.GetAsync(empId, y, m);
            if (sheet != null && !sheet.IsEditable)
            {
                TempData["Error"] = "این ماه ارسال یا تأیید شده و قابل ثبت نیست";
                return RedirectToAction(nameof(Index), new { empId, year = y, month = m });
            }

            var vm = new WorkRecordCreateVm { EmpId = empId, Year = y, Month = m };
            SetupForm(vm, false);
            return View("Create", vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WorkRecordCreateVm vm)
        {
            var emp = await GetEmployeeAsync(vm.EmpId);
            if (emp == null) return Forbid();

            var sheet = await _sheetRepo.GetOrCreateAsync(emp.EmpId, vm.Year, vm.Month);
            if (!sheet.IsEditable)
            {
                TempData["Error"] = "این ماه ارسال یا تأیید شده و قابل ثبت نیست";
                return RedirectToAction(nameof(Index), new { empId = emp.EmpId, year = vm.Year, month = vm.Month });
            }

            var (ok, from, to) = await ValidateDatesAsync(vm, emp.EmpId, null);
            if (!ok)
            {
                SetupForm(vm, false);
                return View("Create", vm);
            }

            await _workRepo.AddAsync(new WorkRecord
            {
                EmpId = emp.EmpId,
                FromDate = from,
                ToDate = to,
                WorkStatus = vm.WorkStatus!.Value,
                Description = vm.Description?.Trim(),
                TimeSheetId = sheet.TimeSheetId,
                CreatedByUserId = UserId,
                CreatedAt = DateTime.UtcNow
            });
            await _workRepo.SaveChangesAsync();

            TempData["Success"] = "کارکرد با موفقیت ثبت شد";
            return RedirectToAction(nameof(Index), new { empId = emp.EmpId, year = vm.Year, month = vm.Month });
        }

        // ───────────── Edit ─────────────
        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            var rec = await LoadAccessibleAsync(id);
            if (rec == null) return NotFound();
            if (!await IsEditableAsync(rec))
            {
                TempData["Error"] = "این ماه ارسال یا تأیید شده و قابل ویرایش نیست";
                return RedirectToAction(nameof(Index), new { empId = rec.EmpId });
            }

            var dt = rec.FromDate.ToDateTime(TimeOnly.MinValue);
            var vm = new WorkRecordCreateVm
            {
                WorkRecordId = rec.WorkRecordId,
                EmpId = rec.EmpId,
                Year = Pc.GetYear(dt),
                Month = Pc.GetMonth(dt),
                FromDate = TimeSheetHelper.ToPersian(rec.FromDate),
                ToDate = TimeSheetHelper.ToPersian(rec.ToDate),
                WorkStatus = rec.WorkStatus,
                Description = rec.Description
            };
            SetupForm(vm, true);
            return View("Create", vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, WorkRecordCreateVm vm)
        {
            var rec = await LoadAccessibleAsync(id);
            if (rec == null) return NotFound();
            if (!await IsEditableAsync(rec))
            {
                TempData["Error"] = "این ماه ارسال یا تأیید شده و قابل ویرایش نیست";
                return RedirectToAction(nameof(Index), new { empId = rec.EmpId });
            }

            vm.EmpId = rec.EmpId; // مقدار فرم نادیده گرفته می‌شود
            var (ok, from, to) = await ValidateDatesAsync(vm, rec.EmpId, id);
            if (!ok)
            {
                vm.WorkRecordId = id;
                SetupForm(vm, true);
                return View("Create", vm);
            }

            rec.FromDate = from;
            rec.ToDate = to;
            rec.WorkStatus = vm.WorkStatus!.Value;
            rec.Description = vm.Description?.Trim();
            _workRepo.Update(rec);
            await _workRepo.SaveChangesAsync();

            TempData["Success"] = "کارکرد ویرایش شد";
            return RedirectToAction(nameof(Index), new { empId = rec.EmpId, year = vm.Year, month = vm.Month });
        }

        // ───────────── Delete ─────────────
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long id)
        {
            var rec = await LoadAccessibleAsync(id);
            if (rec == null) return NotFound();
            if (!await IsEditableAsync(rec))
            {
                TempData["Error"] = "این ماه ارسال یا تأیید شده و قابل حذف نیست";
                return RedirectToAction(nameof(Index), new { empId = rec.EmpId });
            }

            var dt = rec.FromDate.ToDateTime(TimeOnly.MinValue);
            var (y, m) = (Pc.GetYear(dt), Pc.GetMonth(dt));
            var empId = rec.EmpId;
            _workRepo.Remove(rec);
            await _workRepo.SaveChangesAsync();

            TempData["Success"] = "کارکرد حذف شد";
            return RedirectToAction(nameof(Index), new { empId, year = y, month = m });
        }

        // ───────────── Submit ─────────────
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(int empId, int year, int month)
        {
            var emp = await GetEmployeeAsync(empId);
            if (emp == null) return Forbid();

            var sheet = await _sheetRepo.GetAsync(empId, year, month);
            if (sheet == null || !sheet.IsEditable)
            {
                TempData["Error"] = "این ماه قابل ارسال نیست";
                return RedirectToAction(nameof(Index), new { empId, year, month });
            }

            var (s, e) = TimeSheetHelper.GetMonthRange(year, month);
            if ((await _workRepo.GetByEmployeeInRangeAsync(empId, s, e)).Count == 0)
            {
                TempData["Error"] = "برای ارسال حداقل یک کارکرد لازم است";
                return RedirectToAction(nameof(Index), new { empId, year, month });
            }

            sheet.Status = MonthlyStatus.Submitted;
            sheet.SubmittedAt = DateTime.UtcNow;
            sheet.RejectReason = null;
            _sheetRepo.Update(sheet);
            await _sheetRepo.SaveChangesAsync();

            TempData["Success"] = "کارکرد ماه برای تأیید ارسال شد";
            return RedirectToAction(nameof(Index), new { empId, year, month });
        }

        // ───────────── Helpers ─────────────
        /// رکورد را فقط اگر کارمندش در محدوده‌ی نماینده باشد برمی‌گرداند
        private async Task<WorkRecord?> LoadAccessibleAsync(long id)
        {
            var rec = await _workRepo.GetByIdAsync(id);
            if (rec == null) return null;
            return await GetEmployeeAsync(rec.EmpId) == null ? null : rec;
        }

        private async Task<bool> IsEditableAsync(WorkRecord r) =>
            (await _sheetRepo.GetByIdAsync(r.TimeSheetId))?.IsEditable == true;

        private void SetupForm(WorkRecordCreateVm vm, bool isEdit)
        {
            ViewBag.IsEdit = isEdit;
            ViewBag.Statuses = new SelectList(
                Enum.GetValues<WorkStatusType>().Select(s => new { Id = s, Name = TimeSheetHelper.StatusTitle(s) }),
                "Id", "Name", vm.WorkStatus);

            var (start, end) = TimeSheetHelper.GetMonthRange(vm.Year, vm.Month);
            ViewBag.MonthTitle = $"{TimeSheetHelper.MonthNames[vm.Month - 1]} {vm.Year}";
            ViewBag.MinText = TimeSheetHelper.ToPersian(start);
            ViewBag.MaxText = TimeSheetHelper.ToPersian(end);
        }

        private async Task<(bool Ok, DateOnly From, DateOnly To)> ValidateDatesAsync(
            WorkRecordCreateVm vm, int empId, long? excludeId)
        {
            var (ms, me) = TimeSheetHelper.GetMonthRange(vm.Year, vm.Month);

            if (!TimeSheetHelper.TryParsePersian(vm.FromDate, out var from) ||
                !TimeSheetHelper.TryParsePersian(vm.ToDate, out var to))
            {
                ModelState.AddModelError(string.Empty, "تاریخ‌ها معتبر نیستند");
                return (false, default, default);
            }
            if (to < from)
            {
                ModelState.AddModelError(nameof(vm.ToDate), "تاریخ پایان نباید قبل از شروع باشد");
                return (false, default, default);
            }
            if (from < ms || to > me)
            {
                ModelState.AddModelError(string.Empty,
                    $"بازه باید داخل {TimeSheetHelper.MonthNames[vm.Month - 1]} {vm.Year} باشد");
                return (false, default, default);
            }
            if (await _workRepo.HasOverlapAsync(empId, from, to, excludeId))
            {
                ModelState.AddModelError(string.Empty, "این بازه با یک کارکرد دیگر تداخل دارد");
                return (false, default, default);
            }
            return (ModelState.IsValid, from, to);
        }
    }
}
