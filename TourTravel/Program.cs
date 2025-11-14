using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TourTravel.Configuration;
using TourTravel.Models;
using TourTravel.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("Default");

builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));


//  Add Mailservices 
builder.Services.Configure<MailSetting>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddTransient<IMailService, MailService>();




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
    options.Tokens.PasswordResetTokenProvider = "PasswordReset";
})
.AddEntityFrameworkStores<MyDbContext>()
.AddDefaultTokenProviders()
.AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>("PasswordReset");

// Password Reset token lifespan
builder.Services.Configure<DataProtectionTokenProviderOptions>("PasswordReset", options =>
{
    options.TokenLifespan = TimeSpan.FromMinutes(2); // Customize as needed
});
// Default provider lifespan (used by most operations)
builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
{
    options.TokenLifespan = TimeSpan.FromMinutes(5); // expire after 2 minutes
});


// Add MVC
builder.Services.AddControllersWithViews();

// Add Authorization policies
builder.Services.AddAuthorization(options =>
{
  options.AddPolicy("CanEditRole", policy =>
      policy.RequireClaim("Permission", "EditRole"));
});

//  Add Session services
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
  options.IdleTimeout = TimeSpan.FromMinutes(30);
  options.Cookie.HttpOnly = true;
  options.Cookie.IsEssential = true;
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
app.UseSession();

app.UseRouting();

// ✅ Authentication must be BEFORE Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
