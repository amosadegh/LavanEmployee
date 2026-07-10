using Employee.Entities.ViewModels.User;
using Employee.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System;
using Microsoft.EntityFrameworkCore;
using Employee.Entities.Models.User;

namespace Employee.Web.Controllers
{
    public class AccountController(SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager): Controller
    {        
        private readonly SignInManager<ApplicationUser> signInManager = signInManager;
    private readonly UserManager<ApplicationUser> userManager = userManager;
    private readonly RoleManager<IdentityRole> roleManager = roleManager;


    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl)
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (!ModelState.IsValid)
            return View(model);

        var result = await signInManager.PasswordSignInAsync(
            model.EmployeeNumber!, model.Password!, model.RememberMe, lockoutOnFailure: true);

        var user = await userManager.Users.FirstOrDefaultAsync(u => u.UserName == model.EmployeeNumber);
        var roles = await userManager.GetRolesAsync(user!);

        if (user != null && await userManager.IsLockedOutAsync(user))
        {
            return RedirectToAction("LockOut", "Account");
        }
        if (result.Succeeded)
        {

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            if (roles.Contains("SuperAdmin"))
            {
                return RedirectToAction("Index", "Home", new { area = "SuperAdmin" });
            }
        }

        if (result.IsNotAllowed)
        {
            ModelState.AddModelError("", "حساب شما فعال نیست.");
        }
        else
        {
            ModelState.AddModelError("", "شماره پرسنلی یا رمز عبور اشتباه است.");
        }

        return View(model);
    }
    public async Task<IActionResult> LogOut()
    {
        await signInManager.SignOutAsync();
        return RedirectToAction("Login", "Account");
    }
    public ViewResult LockOut()
    {
        return View();
    }
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
}
