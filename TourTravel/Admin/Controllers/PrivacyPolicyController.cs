using Microsoft.AspNetCore.Mvc;
using TourTravel.Models;

namespace TourTravel.Admin.Controllers
{
  [Route("admin/[controller]")]
  public class PrivacyPolicyController : Controller
  {
    private readonly MyDbContext _db;

    public PrivacyPolicyController(MyDbContext db)
    {
      _db = db;
    }

    // ✅ List View
    [HttpGet]
    public IActionResult Index(string search = "")
    {
      var query = _db.PrivacyPolicy.AsQueryable();

      if (!string.IsNullOrWhiteSpace(search))
        query = query.Where(x => x.Title.Contains(search));

      var model = query.OrderByDescending(x => x.Id).ToList();
      ViewBag.Search = search;

      return View("~/Views/Admin/PrivacyPolicy/Index.cshtml", model);
    }

    // ✅ Create Page
    [HttpGet("create")]
    public IActionResult Create()
    {
      return View("~/Views/Admin/PrivacyPolicy/Create.cshtml");
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public IActionResult Create(PrivacyPolicy model)
    {

      _db.PrivacyPolicy.Add(model);
      _db.SaveChanges();
      return Json(new { success = true, message = "Privacy Policy Added successfully!" });
    //  return RedirectToAction("Index");

    }

    // ✅ Edit Page
    [HttpGet("edit/{id}")]
    public IActionResult Edit(int id)
    {
      var privacyPolicy = _db.PrivacyPolicy.Find(id);
      if (privacyPolicy == null)
        return NotFound();
      return View("~/Views/Admin/PrivacyPolicy/Edit.cshtml", privacyPolicy);
    }

    [HttpPost("edit/{id}")]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, PrivacyPolicy model)
    {
      if (ModelState.IsValid)
      {
        var existing = _db.PrivacyPolicy.Find(id);
        if (existing == null)
          return NotFound();

        existing.Title = model.Title;
        existing.Description = model.Description;
        _db.SaveChanges();

        return Json(new { success = true, message = "Privacy Policy Updated successfully!" });
       // return RedirectToAction("Index");
      }

      TempData["Error"] = "Failed to update Term Of Service.";
      return View(model);
    }

    [HttpPost("delete/{id}")]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
      var privacyPolicy = _db.PrivacyPolicy.Find(id);
      if (privacyPolicy != null)
      {
        _db.PrivacyPolicy.Remove(privacyPolicy);
        _db.SaveChanges();
        TempData["Success"] = "Privacy Policy deleted successfully!";
      }
      return RedirectToAction("Index");
    }
  }
}
