using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TourTravel.Models;

namespace TourTravel.Admin.Controllers
{
 
  [Authorize(Roles = "Admin")]
  public class PermissionController : Controller
  {
    private readonly MyDbContext _db;
    private readonly RoleManager<Role> _roleManager;

    public PermissionController(MyDbContext db, RoleManager<Role> roleManager)
    {
      _db = db;
      _roleManager = roleManager;
    }

    public IActionResult Manage(string roleId)
    {
      var role = _roleManager.FindByIdAsync(roleId).Result;
      var pages = _db.Pages.ToList();
      var permissions = _db.Permissions.Where(p => p.RoleId == roleId).Select(p => p.PageId).ToList();

      var model = pages.Select(p => new RolePermissionViewModel
      {
        PageId = p.Id,
        PageName = p.Name,
        IsAssigned = permissions.Contains(p.Id)
      }).ToList();

      ViewBag.RoleId = roleId;
      ViewBag.RoleName = role.Name;

      return View(model);
    }

    [HttpPost]
    public IActionResult Manage(string roleId, List<RolePermissionViewModel> model)
    {
      var existing = _db.Permissions.Where(p => p.RoleId == roleId).ToList();
      _db.Permissions.RemoveRange(existing);

      var newPermissions = model
          .Where(m => m.IsAssigned)
          .Select(m => new Permission
          {
            RoleId = roleId,
            PageId = m.PageId
          }).ToList();

      _db.Permissions.AddRange(newPermissions);
      _db.SaveChanges();

      return RedirectToAction("Index", "Role");
    }
  }



}
