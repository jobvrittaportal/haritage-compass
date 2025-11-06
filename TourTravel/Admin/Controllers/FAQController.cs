using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourTravel.Models;

namespace TourTravel.Admin.Controllers
{
  [Route("admin/[controller]")]
  public class FAQController : Controller
  {
    private readonly MyDbContext _db;

    public FAQController(MyDbContext db)
    {
      _db = db;
    }

    // ✅ List View
    [HttpGet]
    public IActionResult Index(string search = "")
    {
      var query = _db.FAQ.AsQueryable();

      if (!string.IsNullOrWhiteSpace(search))
        query = query.Where(x => x.Question.Contains(search));

      var model = query.OrderByDescending(x => x.Id).ToList();
      ViewBag.Search = search;

      return View("~/Views/Admin/FAQ/Index.cshtml",model);
    }

    // ✅ Create Page
    [HttpGet("create")]
    public IActionResult Create()
    {
      return View("~/Views/Admin/FAQ/Create.cshtml");
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public IActionResult Create(FAQ model)
    {
            var existingFAQ = _db.FAQ
             .FirstOrDefault(f => f.Question.Trim().ToLower() == model.Question.Trim().ToLower());

            if (existingFAQ != null)
            {
                return Json(new { success = false, message = " already exists!" });
                
            }


         _db.FAQ.Add(model);
        _db.SaveChanges();
        TempData["Success"] = "FAQ added successfully!";
        return RedirectToAction("Index");
    
    }

    // ✅ Edit Page
    [HttpGet("edit/{id}")]
    public IActionResult Edit(int id)
    {
      var faq = _db.FAQ.Find(id);
      if (faq == null)
        return NotFound();
      return View("~/Views/Admin/FAQ/Edit.cshtml",faq);
    }

    [HttpPost("edit/{id}")]
    [ValidateAntiForgeryToken]
    public IActionResult Edit( FAQ model)
    {
      if (ModelState.IsValid)
      {
        var existing = _db.FAQ.Find(model.Id);
        if (existing == null)
          return NotFound();

                var duplicate = _db.FAQ
                 .FirstOrDefault(f => f.Question.Trim().ToLower() == model.Question.Trim().ToLower()
                                      && f.Id != model.Id);

                if (duplicate != null)
                {
                    return Json(new { success = false, message = " already exists!" });
                    //   return RedirectToAction("Index");
                }

                existing.Question = model.Question;
        existing.Answer = model.Answer;
        _db.SaveChanges();

        TempData["Success"] = "FAQ updated successfully!";
        return RedirectToAction("Index");
      }

      TempData["Error"] = "Failed to update FAQ.";
      return View(model);
    }

        // ✅ Delete
        [HttpPost("delete/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var faq = _db.FAQ.Find(id);
            if (faq == null)
                return Json(new { success = false, message = "FAQ not found!" });

            _db.FAQ.Remove(faq);
            _db.SaveChanges();

            return Json(new { success = true, message = "FAQ deleted successfully!" });
        }
    }
}
