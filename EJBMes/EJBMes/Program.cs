using Microsoft.EntityFrameworkCore;
using EJBMes.Models;

using EJBMes.Services.Contract;
using EJBMes.Services.Implementation;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<EjbproductionReportContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("EJBConnSQL"));
});

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IKineticService, KineticService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Start/StartSession";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    });

builder.Services.AddControllersWithViews(options => {
    options.Filters.Add(
        new ResponseCacheAttribute { NoStore = true, Location = ResponseCacheLocation.None }
        );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Start}/{action=StartSession}/{id?}");

app.Run();
