using Employee.Entities.Models.User;
using Microsoft.AspNetCore.Identity;

namespace Employee.Web.Data
{
    public static class IdentitySeeder
    {
        public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = { "SuperAdmin", "UserAdmin", "User" };

            foreach (var role in roles)
            {
                // ✅ چک می‌کنیم قبلاً وجود دارد یا نه
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }


        public static async Task SeedSuperAdmin(UserManager<ApplicationUser> userManager)
        {
            const string superadminUsername = "SuperAdmin";
            const string adminEmail = "SuperAdmin@local";
            const string adminPassword = "SuperAdmin";

            // ✅ چک می‌کنیم کاربر از قبل وجود دارد یا نه
            var existingUser = await userManager.FindByNameAsync(superadminUsername);

            if (existingUser == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = superadminUsername,
                    Email = adminEmail,
                    EmailConfirmed = false
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "SuperAdmin");
                }
            }
        }

    }
}
