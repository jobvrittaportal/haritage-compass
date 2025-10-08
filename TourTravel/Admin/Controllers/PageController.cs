//using Microsoft.AspNetCore.Mvc;
//using TourTravel.Data;
//using TourTravel.Models;

//namespace TourTravel.Admin.Controllers
//{
//  [Route("admin/[controller]")]
//  public class PageController : Controller
//  {
//    private readonly MyDbContext _db;
//  //  private readonly PermissionCheck _permission;

//    public PageController(MyDbContext db)
//    {
//      _db = db;
//    //  _permission = new PermissionCheck(_db);
//    }

//    // Index / List pages
//    [HttpGet("")]
//    public IActionResult Index()
//    {
//      //if (!_permission.HasPermission(User, "Page"))
//      //  return RedirectToAction("AccessDenied", "Home");

//      var pages = (from pm in _db.Page
//                   join spm in _db.Page on pm.ParentId equals spm.Id into tspm
//                   from spm in tspm.DefaultIfEmpty()
//                   select new RolePermission
//                   {
//                     Id = pm.Id,
//                     Name = pm.Name,
//                     Label = pm.Label,
//                     Url = pm.Url,
//                     Description = pm.Description,
//                     IsFeature = pm.IsFeature,
//                     ParentId = pm.ParentId,
//                     Parent = spm == null ? pm.Name : spm.Name
//                   })
//                   .OrderBy(p => p.Parent)
//                   .ThenBy(p => p.IsFeature)
//                   .ThenBy(p => p.Name)
//                   .ToList();

//      return View("Views/Admin/Page/Index.cshtml", pages);
//    }

//    // Create form (modal)
//    [HttpGet("create")]
//    public IActionResult Create()
//    {
//      // Convert Pages to SelectListItem
//      ViewBag.ParentPages = _db.Page
//          .Where(p => !p.IsFeature && p.ParentId == null)
//          .Select(p => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
//          {
//            Value = p.Id.ToString(),
//            Text = p.Name
//          })
//          .ToList();

//      return PartialView("~/Views/Admin/Page/_PageForm.cshtml", new Page());
//    }


//    [HttpPost("create")]
//    public IActionResult Create(Page page)
//    {
     

//      if (page.IsFeature && (page.ParentId == null || page.ParentId == 0))
//        return BadRequest("Select a parent page for this feature.");

//      if (!page.IsFeature)
//        page.ParentId = null;

//      _db.Page.Add(page);
//      _db.SaveChanges();
  

//      return Json(new { success = true, message = "Page added successfully!" });
//    }

//    // Edit form (modal)
//    [HttpGet("edit/{id}")]
//    public IActionResult Edit(int id)
//    {
//      var page = _db.Page.Find(id);
//      if (page == null) return NotFound();

//      ViewBag.ParentPages = _db.Page
//          .Where(p => !p.IsFeature && p.ParentId == null)
//          .Select(p => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
//          {
//            Value = p.Id.ToString(),
//            Text = p.Name
//          })
//          .ToList();

//      return PartialView("~/Views/Admin/Page/_PageForm.cshtml", page);
//    }

//    [HttpPost("edit/{id}")]
//    public IActionResult Edit(Page page)
//    {
//      var existing = _db.Page.Find(page.Id);
//      if (existing == null) return NotFound();

//      existing.Name = page.Name;
//      existing.Label = page.Label;
//      existing.Url = page.Url;
//      existing.Description = page.Description;
//      existing.IsFeature = page.IsFeature;
//      existing.ParentId = page.IsFeature ? page.ParentId : null;

//      _db.Page.Update(existing);
//      _db.SaveChanges();
//      //_permission.UpdatePage();

//      return Json(new { success = true, message = "Page updated successfully!" });
//    }

//    // Delete page
//    [HttpPost("delete/{id}")]
//    public IActionResult Delete(int id)
//    {
//      var page = _db.Page.Find(id);
//      if (page == null) return NotFound();

//      var childPages = _db.Page.Where(p => p.ParentId == id).ToList();
//      if (childPages.Any()) _db.Page.RemoveRange(childPages);

//      _db.Page.Remove(page);
//      _db.SaveChanges();
//      //_permission.UpdatePage();

//      return Json(new { success = true, message = "Page deleted successfully!" });
//    }
//  }

//  public class RolePermission
//  {
//    public int Id { get; set; }
//    public required string Name { get; set; }
//    public string? Label { get; set; }
//    public string? Url { get; set; }
//    public string? Description { get; set; }
//    public bool IsFeature { get; set; }
//    public int? ParentId { get; set; }
//    public string? Parent { get; set; }

//    // ✅ Role-based permissions
//    public bool CanView { get; set; }
//    public bool CanCreate { get; set; }
//    public bool CanEdit { get; set; }
//    public bool CanDelete { get; set; }
//    public bool CanExport { get; set; }
//  }

//}
