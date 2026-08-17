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

        public UsersController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
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
    }
}
