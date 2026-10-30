using Employee.DataAccess.Repositories.Interfaces;
using Employee.Entities.Models;
using Employee.Entities.Models.Basic;
using Employee.Entities.Models.User;
using Employee.Entities.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Employee.Web.Areas.SuperAdmin.Controllers
{
    [Area("SuperAdmin")]
    [Authorize(Roles = "SuperAdmin")]

    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ISecondOrganizationUserRepository _secOrgUserRepo;
        private readonly ISecondOrganizationRepository _secOrgRepo;
        private readonly IRepository<Organization> _orgRepo;

        public UsersController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ISecondOrganizationUserRepository secOrgUserRepo,
            ISecondOrganizationRepository secOrgRepo,
            IRepository<Organization> orgRepo)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _secOrgUserRepo = secOrgUserRepo;
            _secOrgRepo = secOrgRepo;
            _orgRepo = orgRepo;
        }

        // ─── Index ────────────────────────────────────────────────
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users
                .AsNoTracking()
                .ToListAsync();

            var model = new List<UserViewModel>();
            foreach (var u in users)
            {
                var roles = await _userManager.GetRolesAsync(u);
                model.Add(new UserViewModel
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    UserName = u.UserName ?? string.Empty,
                    RoleName = roles.FirstOrDefault(),
                    IsActive = u.IsActive
                });
            }

            return View(model);
        }

        // ─── Create GET ───────────────────────────────────────────
        public async Task<IActionResult> Create()
        {
            await PopulateRoles();
            return View(new UserViewModel());
        }

        // ─── Create POST ──────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateRoles();
                return View(model);
            }

            var existing = await _userManager.FindByNameAsync(model.UserName);
            if (existing != null)
            {
                ModelState.AddModelError("Mobile", "این شماره موبایل قبلاً ثبت شده است");
                await PopulateRoles();
                return View(model);
            }

            var user = new ApplicationUser
            {
                UserName = model.UserName,                
                FullName = model.FullName,
                IsActive = model.IsActive
            };

            var result = await _userManager.CreateAsync(user, model.Password!);
            if (!result.Succeeded)
            {
                foreach (var err in result.Errors)
                    ModelState.AddModelError(string.Empty, err.Description);
                await PopulateRoles();
                return View(model);
            }

            if (!string.IsNullOrEmpty(model.RoleName))
                await _userManager.AddToRoleAsync(user, model.RoleName);

            TempData["Success"] = "کاربر با موفقیت ایجاد شد";
            return RedirectToAction(nameof(Index));
        }

        // ─── Edit GET ─────────────────────────────────────────────
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(user);
            var model = new UserViewModel
            {
                Id = user.Id,
                FullName = user.FullName,    
                UserName=user.UserName,
                RoleName = roles.FirstOrDefault(),
                IsActive = user.IsActive
            };

            await PopulateRoles(model.RoleName);
            return View(model);
        }

        // ─── Edit POST ────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserViewModel model)
        {
            // Password در Edit اختیاری است
            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");

            if (!ModelState.IsValid)
            {
                await PopulateRoles(model.RoleName);
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.Id!);
            if (user == null) return NotFound();

            user.FullName = model.FullName;
            user.IsActive = model.IsActive;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                foreach (var err in updateResult.Errors)
                    ModelState.AddModelError(string.Empty, err.Description);
                await PopulateRoles(model.RoleName);
                return View(model);
            }

            // به‌روزرسانی نقش
            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!string.IsNullOrEmpty(model.RoleName))
                await _userManager.AddToRoleAsync(user, model.RoleName);

            // تغییر رمز در صورت ورود
            if (!string.IsNullOrEmpty(model.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                await _userManager.ResetPasswordAsync(user, token, model.Password);
            }

            TempData["Success"] = "کاربر با موفقیت ویرایش شد";
            return RedirectToAction(nameof(Index));
        }

        // ─── Delete POST ──────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            await _userManager.DeleteAsync(user);
            TempData["Success"] = "کاربر حذف شد";
            return RedirectToAction(nameof(Index));
        }

        // ─── Private helper ───────────────────────────────────────
        private async Task PopulateRoles(string? selected = null)
        {
            var roles = await _roleManager.Roles
                .AsNoTracking()
                .Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Name,
                    Selected = r.Name == selected
                })
                .ToListAsync();

            ViewBag.Roles = roles;
        }

        // ── AssignOrganization GET ───────────────────────────────
        public async Task<IActionResult> AssignOrganization(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            ViewBag.UserId = id;
            ViewBag.UserFullName = user.FullName;

            await PopulateOrganizationDropDown();

            var assignments = await _secOrgUserRepo.GetByUserIdAsync(id);
            return View(assignments);
        }

        // ── AssignOrganization POST ──────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignOrganization(
            string userId,
            int secondOrganizationId,
            string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            bool exists = await _secOrgUserRepo.ExistsAsync(userId, secondOrganizationId, role);
            if (exists)
            {
                TempData["Error"] = "این کاربر با این نقش در این واحد سازمانی قبلاً ثبت شده است.";
                return RedirectToAction(nameof(AssignOrganization), new { id = userId });
            }

            var entity = new SecondOrganizationUser
            {
                UserId = userId,
                SecondOrganizationId = secondOrganizationId,
                Role = role
            };

            await _secOrgUserRepo.AddAsync(entity);
            await _secOrgUserRepo.SaveChangesAsync();

            TempData["Success"] = "انتساب با موفقیت ثبت شد.";
            return RedirectToAction(nameof(AssignOrganization), new { id = userId });
        }

        // ── RemoveAssignment POST ────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveAssignment(int assignmentId, string userId)
        {
            var entity = await _secOrgUserRepo.GetSingleAsync(x => x.Id == assignmentId);
            if (entity != null)
            {
                _secOrgUserRepo.Remove(entity);
                await _secOrgUserRepo.SaveChangesAsync();
                TempData["Success"] = "انتساب با موفقیت حذف شد.";
            }

            return RedirectToAction(nameof(AssignOrganization), new { id = userId });
        }

        // ── GetSecondOrganizations — AJAX ────────────────────────
        [HttpGet]
        public async Task<IActionResult> GetSecondOrganizations(int organizationId)
        {
            var items = await _secOrgRepo.GetAsync(
                filter: x => x.OrganizationId == organizationId);

            var result = items.Select(x => new
            {
                id = x.SecondOrganizationId,
                title = x.SecondOrganizationTitle
            });

            return Json(result);
        }

        // ── Private Helpers ──────────────────────────────────────
        private async Task PopulateOrganizationDropDown(int? selectedOrg = null)
        {
            var orgs = await _orgRepo.GetAsync();
            ViewBag.Organizations = new SelectList(
                orgs, "OrganizationId", "OrganizationTitle", selectedOrg);
        }
    }
}
