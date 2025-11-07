using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            if (string.IsNullOrWhiteSpace(model.Title) || string.IsNullOrWhiteSpace(model.Description))
            {
                return Json(new { success = false, message = "Please fill all required fields." });
            }
            var existingTitle = _db.TermsOfService.FirstOrDefault(t => t.Title.Trim().ToLower() == model.Title.Trim().ToLower());
            if (existingTitle != null)
            {
                return Json(new { success = false, message = "This title already exists. Please use a different one." });
            }

            _db.TermsOfService.Add(model);
            _db.SaveChanges();
          
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
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "Please fill all required fields correctly." });
                }

                var existing = _db.TermsOfService.Find(id);
                if (existing == null)
                {
                    return Json(new { success = false, message = "Terms of Service not found." });
                }

                // ✅ Check for duplicate title (excluding current record)
                bool titleExists = _db.TermsOfService
                    .Any(b => b.Id != id && b.Title.Trim().ToLower() == model.Title.Trim().ToLower());

                if (titleExists)
                {
                    return Json(new { success = false, message = "This title already exists. Please use a different one." });
                }

                // ✅ Update fields
                existing.Title = model.Title;
                existing.Description = model.Description;

                _db.SaveChanges();

                return Json(new { success = true, message = "Terms of Service updated successfully!" });
            }
            catch (Exception ex)
            {
                // Optionally log exception
                return Json(new { success = false, message = "An unexpected error occurred while updating the record." });
            }
        }

        [HttpPost("delete/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var termsOfService = _db.TermsOfService.Find(id);
            if (termsOfService == null)
                return Json(new { success = false, message = "Terms of Service not found!" });

            _db.TermsOfService.Remove(termsOfService);
            _db.SaveChanges();

            return Json(new { success = true, message = "Terms of Service deleted successfully!" });
        }
    }
}
