using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourTravel.Models;

namespace TourTravel.Admin.Controllers
{
  [Authorize]
  [Route("admin/WebsiteSetting")]
  public class WebsiteSettingController : Controller
  {
    private readonly MyDbContext _db;

    public WebsiteSettingController(MyDbContext db)
    {
      _db = db;
    }

    // ✅ List View
    [HttpGet]
    public IActionResult Index(string search = "")
    {
      var query = _db.WebsiteSetting.AsQueryable();

      if (!string.IsNullOrWhiteSpace(search))
        query = query.Where(x => x.Email.Contains(search));

      var model = query.OrderByDescending(x => x.Id).ToList();
      ViewBag.Search = search;

      return View("~/Views/Admin/WebsiteSetting/Index.cshtml", model);
    }

    // ✅ Create Page
    [HttpGet("create")]
    public IActionResult Create()
    {
      return View("~/Views/Admin/WebsiteSetting/Create.cshtml");
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public IActionResult Create(WebsiteSetting model)
    {

      // Check if email already exists
      var existing = _db.WebsiteSetting.FirstOrDefault(w => w.Email == model.Email);
      if (existing != null)
      {
        return Json(new { success = false, message = "Email already exists. Cannot add duplicate." });
      }

      _db.WebsiteSetting.Add(model);
      _db.SaveChanges();
      return Json(new { success = true, message = "Added successfully!" });
      //  return RedirectToAction("Index");

    }

    // ✅ Edit Page
    [HttpGet("edit/{id}")]
    public IActionResult Edit(int id)
    {
      var websiteSetting = _db.WebsiteSetting.Find(id);
      if (websiteSetting == null)
        return NotFound();
      return View("~/Views/Admin/WebsiteSetting/Edit.cshtml", websiteSetting);
    }

    [HttpPost("edit/{id}")]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, WebsiteSetting model)
    {
      if (ModelState.IsValid)
      {
        var existing = _db.WebsiteSetting.Find(id);
        if (existing == null)
          return NotFound();

        // Check if the new email is already used by another record
        var emailExists = _db.WebsiteSetting
                             .Any(w => w.Email == model.Email && w.Id != id);
        if (emailExists)
        {
          return Json(new { success = false, message = "Email already exists. Cannot update." });
        }

        existing.Email = model.Email;
        existing.Phone = model.Phone;
        existing.Facebook = model.Facebook;
        existing.Twitter = model.Twitter;
        existing.Instagram = model.Instagram;
        existing.LinkedIn = model.LinkedIn;
        existing.YouTube = model.YouTube;
        existing.Timing = model.Timing;
        existing.Address = model.Address;
        _db.SaveChanges();

        return Json(new { success = true, message = " Updated successfully!" });
        // return RedirectToAction("Index");
      }

      TempData["Error"] = "Failed to update Term Of Service.";
      return View(model);
    }

    [HttpPost("delete/{id}")]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
      var websiteSetting = _db.WebsiteSetting.Find(id);
      if (websiteSetting != null)
      {
        _db.WebsiteSetting.Remove(websiteSetting);
        _db.SaveChanges();
        TempData["Success"] = "  deleted successfully!";
      }
      return RedirectToAction("Index");
    }
  }
}
