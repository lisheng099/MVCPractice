using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using MVCPractice.Interfaces;
using MVCPractice.Models;
using MVCPractice.Models.Account;
using MVCPractice.services;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("MVCPracticeDBContextConnection") ?? throw new InvalidOperationException("Connection string 'MVCPracticeDBContextConnection' not found.");

builder.Services.AddDbContext<MVCPracticeDBContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<MVCPracticeDBContext>().AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();

builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<IActivityService, ActivityService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Home/Index";
});


builder.Services.AddMvc(options =>
{
    options.Filters.Add(new AuthorizeFilter());
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedData.Initialize(services);
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();