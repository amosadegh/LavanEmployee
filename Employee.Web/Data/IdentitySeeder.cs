using Microsoft.AspNetCore.Identity;

namespace Employee.Web.Data
{
    public static class IdentitySeeder
    {
        public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
            if (!await roleManager.RoleExistsAsync("User"))
                await roleManager.CreateAsync(new IdentityRole("User"));

            if (!await roleManager.RoleExistsAsync("SuperAdmin"))

                if (!await roleManager.RoleExistsAsync("UserAdmin"))
                    await roleManager.CreateAsync(new IdentityRole("UserAdmin"));
        }
        public static async Task SeedSuperAdmin(UserManager<IdentityUser> userManager)
        {
            string username = "801348";
            string password = "801348"; // رمز دلخواه

            // بررسی اینکه آیا کاربر وجود دارد یا نه
            var user = await userManager.FindByNameAsync(username);
            if (user == null)
            {
                var newUser = new IdentityUser
                {
                    UserName = username,
                    Email = "superadmin@system.com",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(newUser, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newUser, "SuperAdmin");
                }
            }
        }


    }
}
