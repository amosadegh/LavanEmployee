using Employee.DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Employee.Web.Data;
using Employee.DataAccess.Repositories.Interfaces;
using Employee.DataAccess.Repositories.Implementations;
using Employee.DataAccess.Repository;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
   options.Password.RequireNonAlphanumeric = false;
   options.Password.RequiredLength = 4;
   options.Password.RequireLowercase = false;
   options.Password.RequireUppercase = false;
   options.User.RequireUniqueEmail = true;
   options.SignIn.RequireConfirmedEmail = false;
   options.SignIn.RequireConfirmedAccount = false;
   options.SignIn.RequireConfirmedPhoneNumber = false;
   options.Lockout.MaxFailedAccessAttempts = 3;
   options.Lockout.AllowedForNewUsers = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromDays(14); // تنظیم کوکی برای ۱۴ روز
    options.SlidingExpiration = true; // تمدید خودکار اعتبار کوکی در هر درخواست
});
//#region User
//builder.Services.AddScoped<IUserRepository, UserRepository>();


// Add services to the container.
builder.Services.AddControllersWithViews();
#region BasicDataServices
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IAdministrationRepository, AdministrationRepository>();
builder.Services.AddScoped<IContractTypeRepository, ContractTypeRepository>();
builder.Services.AddScoped<IJobRepository, JobRepository>();
builder.Services.AddScoped<IStudyLevelRepository,StudyLevelRepository>();
builder.Services.AddScoped<IStudyFieldRepository,StudyFieldRepository>();
builder.Services.AddScoped<IEduOrientationRepository,EduOrientationRepository>();
builder.Services.AddScoped<ICampRepository, CampRepository>();
builder.Services.AddScoped<IBankRepository, BankRepository>();
builder.Services.AddScoped<IJobLocationRepository, JobLocationRepository>();
builder.Services.AddScoped<IJobTypeRepository, JobTypeRepository>();
builder.Services.AddScoped<IProvinceRepository, ProvinceRepository>();
builder.Services.AddScoped<ICityRepository,CityRepository>();
builder.Services.AddScoped<IOrganizationRepository,OrganizationRepository>();
builder.Services.AddScoped<ISecondOrganizationRepository,SecondOrganizationRepository>();

#endregion
#region Employee
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
#endregion


var app = builder.Build();

// فراخوانی SeedRoles
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    await IdentitySeeder.SeedRoles(roleManager);
    await IdentitySeeder.SeedSuperAdmin(userManager);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
// Add HttpContextAccessor for session access
builder.Services.AddHttpContextAccessor();

// Add after app.UseRouting(); and before app.UseAuthentication();
app.UseSession();

#pragma warning disable ASP0014 // Suggest using top level route registrations
app.UseEndpoints(endpoints =>
{
    // مسیر مربوط به Areas
    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
    // مسیر پیش‌فرض
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Account}/{action=Login}/{id?}");
});
#pragma warning restore ASP0014 // Suggest using top level route registrations

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
