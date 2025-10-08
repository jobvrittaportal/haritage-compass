using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TourTravel.Models;
public class AccountController : Controller
{
  private readonly UserManager<ApplicationUser> _userManager;
  private readonly SignInManager<ApplicationUser> _signInManager;
  private readonly MyDbContext _db;

  public AccountController(UserManager<ApplicationUser> userManager,
                           SignInManager<ApplicationUser> signInManager,
                           MyDbContext db)
  {
    _userManager = userManager;
    _signInManager = signInManager;
    _db = db;
  }

  [HttpGet]
  public IActionResult Login() => View("~/Views/Admin/Account/Index.cshtml");

  [HttpPost]
  public async Task<IActionResult> Login(string email, string password)
  {
    var user = await _userManager.FindByEmailAsync(email);
    if (user == null || !user.IsActive)
    {
      ViewBag.Error = "Invalid login or inactive account!";
      View("~/Views/Admin/Account/Index.cshtml");
    }

    var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
    if (!result.Succeeded)
    {
      ViewBag.Error = "Invalid login credentials!";
      View("~/Views/Admin/Account/Index.cshtml");
    }

    // Assign claims dynamically
    var roles = await _userManager.GetRolesAsync(user);
    var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName ?? user.Email)
        };

    var rolePermissions = _db.Permissions
        .Where(p => roles.Contains(p.Role.Name))
        .Select(p => p.Page.Name)
        .Distinct()
        .ToList();

    foreach (var perm in rolePermissions)
      claims.Add(new Claim("Permission", perm));

    await _signInManager.SignInWithClaimsAsync(user, false, claims);

    return RedirectToAction("Index", "Dashboard");
  }

  [HttpPost]
  public async Task<IActionResult> Logout()
  {
    await _signInManager.SignOutAsync();
    return RedirectToAction("Login");
  }
}
