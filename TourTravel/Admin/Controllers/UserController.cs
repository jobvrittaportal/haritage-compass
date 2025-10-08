using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TourTravel.Models;

namespace TourTravel.Controllers
{
  public class UserController : Controller
  {
   
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
   
    public UserController(UserManager<ApplicationUser> userManager,
                          RoleManager<IdentityRole> roleManager)
    {
      _userManager = userManager;
      _roleManager = roleManager;
    }
    [Authorize]
    [Route("admin/User")]
    public IActionResult Index()
    {
      var users = _userManager.Users.ToList();
      return View("~/Views/Admin/User/Index.cshtml", users);
    }

    //[HttpGet]
    //public IActionResult Create() => View("~/Admin/Views/User/Index.cshtml");
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(ApplicationUser model, string password)
    {

      //if (!ModelState.IsValid)
      //{
      //  // Optional: log validation errors
      //  var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
      //  return BadRequest(new { message = "Validation failed", errors });
      //}
      // Check if email/username already exists
      var existingUser = await _userManager.FindByEmailAsync(model.Email);
      if (existingUser != null)
      {
        ModelState.AddModelError("", "A user with this email already exists.");
        return View("~/Admin/Views/User/Index.cshtml", model);
      }
      var user = new ApplicationUser
        {
          UserName = model.Email,
          Email = model.Email,
          Name = model.Name,
          IsActive = model.IsActive
        };

        var result = await _userManager.CreateAsync(user, password);

        if (result.Succeeded)
          return RedirectToAction("Index");

        foreach (var error in result.Errors)
          ModelState.AddModelError("", error.Description);
      

      return View("~/Admin/Views/User/Index.cshtml", model);
    }

    // 🔹 Edit User (GET)
    //[HttpGet]
    //public async Task<IActionResult> Edit(string id)
    //{
    //  var user = await _userManager.FindByIdAsync(id);
    //  if (user == null) return NotFound();
    //  return View("~/Admin/Views/User/Index.cshtml", user);
    //}

    // 🔹 Edit User (POST)
    //[Authorize]
    //[HttpPost]
    //public async Task<IActionResult> Edit(ApplicationUser model)
    //{
    //  var user = await _userManager.FindByIdAsync(model.Id);
    //  if (user == null) return NotFound();

    //  user.Name = model.Name;
    //  user.Email = model.Email;
    //  user.UserName = model.Email;
    //  user.IsActive = model.IsActive;

    //  var result = await _userManager.UpdateAsync(user);
    //  if (result.Succeeded)
    //    return RedirectToAction("Index");

    //  foreach (var error in result.Errors)
    //    ModelState.AddModelError("", error.Description);

    //  return View(model);
    //}

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Edit(ApplicationUser model, string? password)
    {
      var user = await _userManager.FindByIdAsync(model.Id);
      if (user == null) return NotFound();

      // Update user basic info
      user.Name = model.Name;
      user.Email = model.Email;
      user.UserName = model.Email;
      user.IsActive = model.IsActive;

      var result = await _userManager.UpdateAsync(user);

      if (!result.Succeeded)
      {
        foreach (var error in result.Errors)
          ModelState.AddModelError("", error.Description);

        return View(model);
      }

      // Update password if provided
      if (!string.IsNullOrEmpty(password))
      {
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var passwordResult = await _userManager.ResetPasswordAsync(user, token, password);

        if (!passwordResult.Succeeded)
        {
          foreach (var error in passwordResult.Errors)
            ModelState.AddModelError("", error.Description);

          return View(model);
        }
      }

      return RedirectToAction("Index");
    }


    // 🔹 Delete User

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Delete(string id)
    {
      var user = await _userManager.FindByIdAsync(id);
      if (user == null) return NotFound();

      await _userManager.DeleteAsync(user);
      return RedirectToAction("Index");
    }
  }
}
