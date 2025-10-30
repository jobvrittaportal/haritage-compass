using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TourTravel.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("Default");

builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));




// Register IHttpClientFactory
builder.Services.AddHttpClient();



// Add Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
  options.Password.RequireDigit = false;
  options.Password.RequireLowercase = false;
  options.Password.RequireUppercase = false;
  options.Password.RequireNonAlphanumeric = false;
  options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<MyDbContext>()
.AddDefaultTokenProviders();

// Add MVC
builder.Services.AddControllersWithViews();

// Add Authorization policies
builder.Services.AddAuthorization(options =>
{
  options.AddPolicy("CanEditRole", policy =>
      policy.RequireClaim("Permission", "EditRole"));
});

var app = builder.Build();



// Configure middleware
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Home/Error");
  app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ✅ Authentication must be BEFORE Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
