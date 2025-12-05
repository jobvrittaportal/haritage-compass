using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
      var existingUser = await _userManager.FindByEmailAsync(model.Email);
      if (existingUser != null)
        return Json(new { success = false, message = "A user with this email already exists." });

      if (!string.IsNullOrWhiteSpace(model.EmployeeCode))
      {
        string apiUrl = $"https://hrmsapi.jobvritta.com/api/Hrlense_Employee/employeeCodeCheck?empCode={model.EmployeeCode}";

        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("hrms-key", "Amar@Deep1Jobvritta#157");

        HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

        if (!response.IsSuccessStatusCode)
        {
          string errorResponse = await response.Content.ReadAsStringAsync();
          return Json(new { success = false, message = "Employee code does not exist in HRLense.", detail = errorResponse });
        }

        var json = await response.Content.ReadAsStringAsync();
        var hrEmployee = JsonConvert.DeserializeObject<EmployeeCheckResponse>(json);

        if (hrEmployee == null || hrEmployee.employee_Code != model.EmployeeCode)
        {
          return Json(new { success = false, message = "Invalid Employee Code. This code is not present in HRLense." });

        }

      }


      var user = new ApplicationUser
      {
        UserName = model.Email,
        Email = model.Email,
        Name = model.Name,
        IsActive = model.IsActive,
        EmployeeCode = model.EmployeeCode

      };

      var result = await _userManager.CreateAsync(user, password);
      if (result.Succeeded)
        return Json(new { success = true, message = "User added successfully!" });

      var errors = string.Join(", ", result.Errors.Select(e => e.Description));
      return Json(new { success = false, message = errors });
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Edit(ApplicationUser model, string? password)
    {
      var user = await _userManager.FindByIdAsync(model.Id);
      if (user == null)
        return Json(new { success = false, message = "User not found." });

      // Check for duplicate email (but ignore the same user)
      var existingUser = await _userManager.FindByEmailAsync(model.Email);
      if (existingUser != null && existingUser.Id != user.Id)
      {
        return Json(new { success = false, message = "Email already exists. Please use a different email." });
      }

      user.Name = model.Name;
      user.Email = model.Email;
      user.UserName = model.Email;
      user.IsActive = model.IsActive;
      user.EmployeeCode = model.EmployeeCode;


      var result = await _userManager.UpdateAsync(user);
      if (!result.Succeeded)
      {
        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
        return Json(new { success = false, message = errors });
      }

      // Update password only if provided
      if (!string.IsNullOrEmpty(password))
      {
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var passwordResult = await _userManager.ResetPasswordAsync(user, token, password);
        if (!passwordResult.Succeeded)
        {
          var errors = string.Join(", ", passwordResult.Errors.Select(e => e.Description));
          return Json(new { success = false, message = errors });
        }
      }

      return Json(new { success = true, message = "User updated successfully!" });
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
      var user = await _userManager.FindByIdAsync(id);
      if (user == null) return NotFound();
      return View("~/Admin/Views/User/Index.cshtml", user);
    }

    //  Edit User (POST)
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

    //[Authorize]
    //[HttpPost]
    //public async Task<IActionResult> Edit(ApplicationUser model, string? password)
    //{
    //  var user = await _userManager.FindByIdAsync(model.Id);
    //  if (user == null) return NotFound();

    //  // Update user basic info
    //  user.Name = model.Name;
    //  user.Email = model.Email;
    //  user.UserName = model.Email;
    //  user.IsActive = model.IsActive;

    //  var result = await _userManager.UpdateAsync(user);

    //  if (!result.Succeeded)
    //  {
    //    foreach (var error in result.Errors)
    //      ModelState.AddModelError("", error.Description);

    //    return View(model);
    //  }

    //  // Update password if provided
    //  if (!string.IsNullOrEmpty(password))
    //  {
    //    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
    //    var passwordResult = await _userManager.ResetPasswordAsync(user, token, password);

    //    if (!passwordResult.Succeeded)
    //    {
    //      foreach (var error in passwordResult.Errors)
    //        ModelState.AddModelError("", error.Description);

    //      return View(model);
    //    }
    //  }

    //  return RedirectToAction("Index");
    //}


    //  Delete User

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Delete(string id)
    {
      var user = await _userManager.FindByIdAsync(id);
      if (user == null) return NotFound();

      await _userManager.DeleteAsync(user);
      return Json(new { success = true, message = "Deleted successfully!" });
    }
  }

  public class EmployeeCheckResponse
  {
    public string employee_Name { get; set; }
    public string email { get; set; }
    public string employee_Code { get; set; }
  }
}
