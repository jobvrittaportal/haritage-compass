using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            try
            {
                if (string.IsNullOrWhiteSpace(model.Title) || string.IsNullOrWhiteSpace(model.Description))
                {
                    return Json(new { success = false, message = "Please fill all required fields." });
                }

                var existingTitle = _db.PrivacyPolicy.FirstOrDefault(t => t.Title.Trim().ToLower() == model.Title.Trim().ToLower());

                if (existingTitle != null)
                {
                    return Json(new { success = false, message = " already exists!" });
                }

                _db.PrivacyPolicy.Add(model);
                _db.SaveChanges();
                return Json(new { success = true, message = "Privacy Policy Added successfully!" });
            }

            
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }

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

         bool titleexist =  _db.PrivacyPolicy.Any(b => b.Id != model.Id && b.Title.ToLower() == model.Title.ToLower());

          if (titleexist)
            {
              return Json(new { success = false, message = "This Title already exists. Please use a different one." });
            }

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
            if (privacyPolicy == null)
                return Json(new { success = false, message = "Privacy Policy not found!" });
            _db.PrivacyPolicy.Remove(privacyPolicy);
            _db.SaveChanges();

            return Json(new { success = true, message = "Privacy Policy Delete Successfully!" });
        }
    }
}
