using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVCPractice.Areas.Identity.Data;
using MVCPractice.Interfaces;
using MVCPractice.Models;
using MVCPractice.servers;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("MVCPracticeDBContextConnection") ?? throw new InvalidOperationException("Connection string 'MVCPracticeDBContextConnection' not found.");

builder.Services.AddDbContext<MVCPracticeDBContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddIdentity<MVCPracticeUser, IdentityRole>().AddEntityFrameworkStores<MVCPracticeDBContext>().AddDefaultTokenProviders();

//builder.Services.AddDefaultIdentity<MVCPracticeUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<MVCPracticeContext>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(300);
    options.Cookie.HttpOnly = true; 
    options.Cookie.IsEssential = true; 
});

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<IActivityService, ActivityService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    SeedData.Initialize(services);
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
