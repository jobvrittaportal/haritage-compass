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
      return BadRequest("A role with this name already exists.");

    var role = new IdentityRole(Name);
    var result = await _roleManager.CreateAsync(role);

    return result.Succeeded ? RedirectToAction("Index") : BadRequest(result.Errors);
  }


  [HttpPost]
  [Authorize]
  public async Task<IActionResult> Edit(string Id, string Name)
  {
    var role = await _roleManager.FindByIdAsync(Id);
    if (role == null) return View("~/Views/Admin/Role/Index.cshtml") ;
    role.Name = Name;
    role.NormalizedName = Name.ToUpper();
    var result = await _roleManager.UpdateAsync(role);
    return result.Succeeded ?     RedirectToAction("Index") : BadRequest(result.Errors);
  }

  [HttpPost]
  [Authorize]
  public async Task<IActionResult> Delete(string Id)
  {
    var role = await _roleManager.FindByIdAsync(Id);
    if (role == null) return View("~/Views/Admin/Role/Index.cshtml");
    var result = await _roleManager.DeleteAsync(role);
    return result.Succeeded ? RedirectToAction("Index") : BadRequest(result.Errors);
  }
}
