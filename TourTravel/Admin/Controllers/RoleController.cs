using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TourTravel.Models;

public class RoleController : Controller
{
  private readonly RoleManager<IdentityRole> _roleManager;

  public RoleController(RoleManager<IdentityRole> roleManager)
  {
    _roleManager = roleManager;
  }
  [Route("admin/role")]
  [Authorize]
  [HttpGet]
  public IActionResult Index(string search)
  {
    var roles = _roleManager.Roles.ToList(); // Load all roles into memory

    if (!string.IsNullOrWhiteSpace(search))
    {
      roles = roles
          .Where(r => r.Name.Contains(search, StringComparison.OrdinalIgnoreCase))
          .ToList();
    }

    return View("~/Views/Admin/Role/Index.cshtml", roles);
  }



  [HttpPost]
  [Authorize]
  public async Task<IActionResult> Create(string Name)
  {
    if (string.IsNullOrWhiteSpace(Name))
      return BadRequest("Role name cannot be empty.");

    // Check if role already exists
    var existingRole = await _roleManager.FindByNameAsync(Name);
    if (existingRole != null)
      return Json(new { success = false, message = "A role with this name already exists.!" });



    var role = new IdentityRole(Name);
    var result = await _roleManager.CreateAsync(role);

    return Json(new { success = true, message = "Added successfully!" });
  }

  [HttpPost]
  [Authorize]
  public async Task<IActionResult> Edit(string Id, string Name)
  {
    var role = await _roleManager.FindByIdAsync(Id);
    if (role == null)
      return Json(new { success = false, message = "Role not found." });

    // Check if another role with the same name exists
    var existingRole = await _roleManager.FindByNameAsync(Name);
    if (existingRole != null && existingRole.Id != role.Id)
    {
      return Json(new { success = false, message = "A role with this name already exists. Please choose a different name." });
    }

    // Update role name
    role.Name = Name;
    role.NormalizedName = Name.ToUpper();

    var result = await _roleManager.UpdateAsync(role);
    if (!result.Succeeded)
    {
      var errors = string.Join(", ", result.Errors.Select(e => e.Description));
      return Json(new { success = false, message = errors });
    }

    return Json(new { success = true, message = "Role updated successfully!" });
  }

  [HttpPost]
  [Authorize]
  public async Task<IActionResult> Delete(string Id)
  {
    var role = await _roleManager.FindByIdAsync(Id);
    if (role == null) return View("~/Views/Admin/Role/Index.cshtml");
    var result = await _roleManager.DeleteAsync(role);
    return Json(new { success = true, message = "Delete successfully!" });
  }
}
