using Microsoft.AspNetCore.Mvc;
using TourTravel.Models;

namespace TourTravel.Admin.Controllers
{
  [Route("admin/[controller]")]
  public class TermsOfServiceController : Controller
  {
    private readonly MyDbContext _db;

    public TermsOfServiceController(MyDbContext db)
    {
      _db = db;
    }

    // ✅ List View
    [HttpGet]
    public IActionResult Index(string search = "")
    {
      var query = _db.TermsOfService.AsQueryable();

      if (!string.IsNullOrWhiteSpace(search))
        query = query.Where(x => x.Title.Contains(search));

      var model = query.OrderByDescending(x => x.Id).ToList();
      ViewBag.Search = search;

      return View("~/Views/Admin/TermsOfService/Index.cshtml", model);
    }

    // ✅ Create Page
    [HttpGet("create")]
    public IActionResult Create()
    {
      return View("~/Views/Admin/TermsOfService/Create.cshtml");
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public IActionResult Create(TermsOfService model)
    {

      _db.TermsOfService.Add(model);
      _db.SaveChanges();
      TempData["Success"] = "Term Of Service added successfully!";
      return Json(new { success = true, message = "Term Of Service added successfully!" });

    }

    // ✅ Edit Page
    [HttpGet("edit/{id}")]
    public IActionResult Edit(int id)
    {
      var termsOfService = _db.TermsOfService.Find(id);
      if (termsOfService == null)
        return NotFound();
      return View("~/Views/Admin/TermsOfService/Edit.cshtml", termsOfService);
    }

    [HttpPost("edit/{id}")]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, TermsOfService model)
    {
      if (ModelState.IsValid)
      {
        var existing = _db.TermsOfService.Find(id);
        if (existing == null)
          return NotFound();

        existing.Title = model.Title;
        existing.Description = model.Description;
        _db.SaveChanges();

        return Json(new { success = true, message = "Term Of Service Updated successfully!" });
      //  return RedirectToAction("Index");
      }

      TempData["Error"] = "Failed to update Term Of Service.";
      return View(model);
    }

    [HttpPost("delete/{id}")]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
      var termsOfService = _db.TermsOfService.Find(id);
      if (termsOfService != null)
      {
        _db.TermsOfService.Remove(termsOfService);
        _db.SaveChanges();
        TempData["Success"] = "TermsOfService deleted successfully!";
      }
      return RedirectToAction("Index");
    }
  }
}
