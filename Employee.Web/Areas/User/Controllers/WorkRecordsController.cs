using System.Globalization;
using System.Security.Claims;
using Employee.DataAccess.Repositories.Interfaces;
using Employee.Entities.Models.Enums;
using Employee.Entities.Models.Personel;
using Employee.Entities.Models.TimeSheet;
using Employee.Models.TimeSheet;
using Employee.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Employee.Areas.User.Controllers
{
    /// فقط کاربر با نقش User (کارمند) به کارکرد خودش دسترسی دارد.
    [Area("User")]
    [Authorize(Roles = "User")]
    public class WorkRecordsController : Controller
    {
        private static readonly PersianCalendar Pc = new();

        private readonly IWorkRecordRepository _workRepo;
        private readonly IEmployeeRepository _employeeRepo;
        private readonly IMonthlyTimeSheetRepository _sheetRepo;

        public WorkRecordsController(IWorkRecordRepository workRepo,
                                     IEmployeeRepository employeeRepo,
                                     IMonthlyTimeSheetRepository sheetRepo)
        {
            _workRepo = workRepo;
            _employeeRepo = employeeRepo;
            _sheetRepo = sheetRepo;
        }

        private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        private Task<TblEmployee?> GetMeAsync() => _employeeRepo.GetSingleAsync(e => e.UserId == UserId);

        // ───────────── Index ─────────────
        public async Task<IActionResult> Index(int? year, int? month)
        {
            var now = DateTime.Now;
            var (y, m) = TimeSheetHelper.NormalizeYearMonth(year, month, now);

            var me = await GetMeAsync();
            if (me == null) return Forbid();

            var (start, end) = TimeSheetHelper.GetMonthRange(y, m);
            var records = await _workRepo.GetByEmployeeInRangeAsync(me.EmpId, start, end);
            var sheet = await _sheetRepo.GetAsync(me.EmpId, y, m);

            ViewBag.Sheet = sheet;
            ViewBag.SheetStatus = sheet?.Status ?? MonthlyStatus.Draft;
            ViewBag.SheetEditable = sheet?.IsEditable ?? true;
            ViewBag.Year = y;
            ViewBag.Month = m;
            ViewBag.Years = new SelectList(Enumerable.Range(Pc.GetYear(now) - 5, 7).Reverse(), y);
            ViewBag.Months = new SelectList(
                TimeSheetHelper.MonthNames.Select((n, i) => new { Id = i + 1, Name = n }), "Id", "Name", m);
            ViewBag.DayTotals = records
                .GroupBy(r => r.WorkStatus)
                .ToDictionary(g => g.Key, g => g.Sum(r => r.GetDaysInRange(start, end)));
            return View(records);
        }

        // ───────────── Create ─────────────
        [HttpGet]
        public async Task<IActionResult> Create(int? year, int? month)
        {
            var me = await GetMeAsync();
            if (me == null) return Forbid();

            var (y, m) = TimeSheetHelper.NormalizeYearMonth(year, month, DateTime.Now);
            var sheet = await _sheetRepo.GetAsync(me.EmpId, y, m);
            if (sheet != null && !sheet.IsEditable)
            {
                TempData["Error"] = "این ماه ارسال یا تأیید شده و قابل ثبت نیست";
                return RedirectToAction(nameof(Index), new { year = y, month = m });
            }

            var vm = new WorkRecordCreateVm { Year = y, Month = m };
            SetupForm(vm, isEdit: false);
            return View("Create", vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WorkRecordCreateVm vm)
        {
            var me = await GetMeAsync();
            if (me == null) return Forbid();

            var sheet = await _sheetRepo.GetOrCreateAsync(me.EmpId, vm.Year, vm.Month);
            if (!sheet.IsEditable)
            {
                TempData["Error"] = "این ماه ارسال یا تأیید شده و قابل ثبت نیست";
                return RedirectToAction(nameof(Index), new { year = vm.Year, month = vm.Month });
            }

            var (ok, from, to) = await ValidateDatesAsync(vm, me.EmpId, null);
            if (!ok)
            {
                SetupForm(vm, isEdit: false);
                return View("Create", vm);
            }

            await _workRepo.AddAsync(new WorkRecord
            {
                EmpId = me.EmpId,
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
            return RedirectToAction(nameof(Index), new { year = vm.Year, month = vm.Month });
        }

        // ───────────── Edit ─────────────
        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            var me = await GetMeAsync();
            if (me == null) return Forbid();

            var rec = await _workRepo.GetByIdAsync(id);
            if (rec == null || rec.EmpId != me.EmpId) return NotFound();
            if (!await IsEditableAsync(rec))
            {
                TempData["Error"] = "این ماه ارسال یا تأیید شده و قابل ویرایش نیست";
                return RedirectToAction(nameof(Index));
            }

            var (y, m) = (Pc.GetYear(rec.FromDate.ToDateTime(TimeOnly.MinValue)),
                          Pc.GetMonth(rec.FromDate.ToDateTime(TimeOnly.MinValue)));
            var vm = new WorkRecordCreateVm
            {
                WorkRecordId = rec.WorkRecordId,
                Year = y,
                Month = m,
                FromDate = TimeSheetHelper.ToPersian(rec.FromDate),
                ToDate = TimeSheetHelper.ToPersian(rec.ToDate),
                WorkStatus = rec.WorkStatus,
                Description = rec.Description
            };
            SetupForm(vm, isEdit: true);
            return View("Create", vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, WorkRecordCreateVm vm)
        {
            var me = await GetMeAsync();
            if (me == null) return Forbid();

            var rec = await _workRepo.GetByIdAsync(id);
            if (rec == null || rec.EmpId != me.EmpId) return NotFound();
            if (!await IsEditableAsync(rec))
            {
                TempData["Error"] = "این ماه ارسال یا تأیید شده و قابل ویرایش نیست";
                return RedirectToAction(nameof(Index));
            }

            var (ok, from, to) = await ValidateDatesAsync(vm, me.EmpId, id);
            if (!ok)
            {
                vm.WorkRecordId = id;
                SetupForm(vm, isEdit: true);
                return View("Create", vm);
            }

            rec.FromDate = from;
            rec.ToDate = to;
            rec.WorkStatus = vm.WorkStatus!.Value;
            rec.Description = vm.Description?.Trim();
            _workRepo.Update(rec);
            await _workRepo.SaveChangesAsync();

            TempData["Success"] = "کارکرد ویرایش شد";
            return RedirectToAction(nameof(Index), new { year = vm.Year, month = vm.Month });
        }

        // ───────────── Delete ─────────────
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long id)
        {
            var me = await GetMeAsync();
            if (me == null) return Forbid();

            var rec = await _workRepo.GetByIdAsync(id);
            if (rec == null || rec.EmpId != me.EmpId) return NotFound();
            if (!await IsEditableAsync(rec))
            {
                TempData["Error"] = "این ماه ارسال یا تأیید شده و قابل حذف نیست";
                return RedirectToAction(nameof(Index));
            }

            var (y, m) = (Pc.GetYear(rec.FromDate.ToDateTime(TimeOnly.MinValue)),
                          Pc.GetMonth(rec.FromDate.ToDateTime(TimeOnly.MinValue)));
            _workRepo.Remove(rec);
            await _workRepo.SaveChangesAsync();

            TempData["Success"] = "کارکرد حذف شد";
            return RedirectToAction(nameof(Index), new { year = y, month = m });
        }

        // ───────────── Submit (ارسال ماه برای تأیید) ─────────────
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(int year, int month)
        {
            var me = await GetMeAsync();
            if (me == null) return Forbid();

            var sheet = await _sheetRepo.GetAsync(me.EmpId, year, month);
            if (sheet == null || !sheet.IsEditable)
            {
                TempData["Error"] = "این ماه قابل ارسال نیست";
                return RedirectToAction(nameof(Index), new { year, month });
            }

            var (s, e) = TimeSheetHelper.GetMonthRange(year, month);
            if ((await _workRepo.GetByEmployeeInRangeAsync(me.EmpId, s, e)).Count == 0)
            {
                TempData["Error"] = "برای ارسال حداقل یک کارکرد لازم است";
                return RedirectToAction(nameof(Index), new { year, month });
            }

            sheet.Status = MonthlyStatus.Submitted;
            sheet.SubmittedAt = DateTime.UtcNow;
            sheet.RejectReason = null;
            _sheetRepo.Update(sheet);
            await _sheetRepo.SaveChangesAsync();

            TempData["Success"] = "کارکرد ماه برای تأیید ارسال شد";
            return RedirectToAction(nameof(Index), new { year, month });
        }

        // ───────────── Helpers ─────────────
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
            ViewBag.MinIso = start.ToString("yyyy-MM-dd");
            ViewBag.MaxIso = end.ToString("yyyy-MM-dd");
        }

        /// اعتبارسنجی تاریخ‌ها؛ خطاها در ModelState ثبت می‌شوند
        private async Task<(bool Ok, DateOnly From, DateOnly To)> ValidateDatesAsync(
            WorkRecordCreateVm vm, int empId, long? excludeId)
        {
            var (ms, me) = TimeSheetHelper.GetMonthRange(vm.Year, vm.Month);

            if (!TimeSheetHelper.TryParsePersian(vm.FromDate, out var from) || !TimeSheetHelper.TryParsePersian(vm.ToDate, out var to))
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
