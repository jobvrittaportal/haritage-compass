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

            return View("~/Views/Admin/FAQ/Index.cshtml", model);
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
            try
            {
                var existingFAQ = _db.FAQ
                 .FirstOrDefault(f => f.Question.Trim().ToLower() == model.Question.Trim().ToLower());

                if (existingFAQ != null)
                {
                    return Json(new { success = false, message = "FAQ  already exists!" });

                }


                _db.FAQ.Add(model);
                _db.SaveChanges();
                return Json(new { success = true, message = "FAQ created successfully." });
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
            var faq = _db.FAQ.Find(id);
            if (faq == null)
                return NotFound();
            return View("~/Views/Admin/FAQ/Edit.cshtml", faq);
        }

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(FAQ model)
        {
            try
            {
                // 🧩 Check if record exists
                var existing = await _db.FAQ.FindAsync(model.Id);
                if (existing == null)
                    return Json(new { success = false, message = "FAQ record not found." });

                // 🧠 Duplicate question check (case-insensitive)
                bool duplicateExists = await _db.FAQ
                    .AnyAsync(f => f.Id != model.Id && f.Question.Trim().ToLower() == model.Question.Trim().ToLower());

                if (duplicateExists)
                    return Json(new { success = false, message = "This question already exists. Please use a different one." });

                // ✅ Update fields
                existing.Question = model.Question.Trim();
                existing.Answer = model.Answer.Trim();

                _db.Update(existing);
                await _db.SaveChangesAsync();

                return Json(new { success = true, message = "FAQ updated successfully." });
            }
            catch (Exception ex)
            {
                // ⚠️ Error handling
                return Json(new { success = false, message = "An error occurred: " + ex.Message });
            }
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
