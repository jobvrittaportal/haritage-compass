using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourTravel.Models;

namespace TourTravel.Controllers.Admin
{
    [Route("admin/[controller]")]
    public class HeroSliderController : Controller
    {
        private readonly MyDbContext _db;
        private readonly IWebHostEnvironment _env;

        public HeroSliderController(MyDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? search, int page = 1, int pageSize = 2)
        {
            var query = _db.HeroSlider.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(h => h.Title.Contains(search) || h.SubTitle.Contains(search));
            }

            int totalItems = await query.CountAsync();
            var list = await query
                .OrderByDescending(h => h.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.Search = search;
            ViewBag.Page = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            return View("~/Views/Admin/HeroSlider/Index.cshtml", list);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View("~/Views/Admin/HeroSlider/Create.cshtml");
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HeroSlider model, IFormFile? ImageFile)
        {
            if (ImageFile == null)
                return Json(new { success = false, message = "The Image field is required." });

            if (!string.IsNullOrEmpty(model.Title))
            {
                bool titleExists = await _db.HeroSlider.AnyAsync(t => t.Title.Trim().ToLower() == model.Title.Trim().ToLower());
                if (titleExists)
                    return Json(new { success = false, message = "This title already exists!" });
            }

            string folderPath = Path.Combine(_env.WebRootPath, "uploads", "HeroSlider");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
            string filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await ImageFile.CopyToAsync(stream);
            }

            model.Image = "/uploads/HeroSlider/" + fileName;
            _db.HeroSlider.Add(model);
            await _db.SaveChangesAsync();

            return Json(new { success = true, message = "Hero Slider created successfully!" });
        }





        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var slider = await _db.HeroSlider.FindAsync(id);
            if (slider == null) return NotFound();

            return View("~/Views/Admin/HeroSlider/Edit.cshtml", slider);
        }

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, HeroSlider model, IFormFile? ImageFile)
        {
            try
            {
                var existing = await _db.HeroSlider.FindAsync(id);
                if (existing == null)
                    return Json(new { success = false, message = "Home Page Slider not found." });

                // ✅ Check for duplicate title (excluding current record)
                if (!string.IsNullOrEmpty(model.Title))
                {
                    bool titleExists = await _db.HeroSlider
                        .AnyAsync(t => t.Title.Trim().ToLower() == model.Title.Trim().ToLower() && t.Id != id);

                    if (titleExists)
                        return Json(new { success = false, message = "A Home Page Slider with this title already exists." });
                }

                // ✅ Update basic fields
                existing.Title = model.Title;
                existing.SubTitle = model.SubTitle;
                existing.Description = model.Description;
                existing.RotationTime = model.RotationTime;

                // ✅ Image upload (optional)
                if (ImageFile != null)
                {
                    string folderPath = Path.Combine(_env.WebRootPath, "uploads", "HeroSlider");
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);

                    string fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
                    string filePath = Path.Combine(folderPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }

                    // Delete old image if exists
                    if (!string.IsNullOrEmpty(existing.Image))
                    {
                        var oldImagePath = Path.Combine(_env.WebRootPath, existing.Image.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                            System.IO.File.Delete(oldImagePath);
                    }

                    existing.Image = "/uploads/HeroSlider/" + fileName;
                }

                await _db.SaveChangesAsync();

                return Json(new { success = true, message = "Home Page Slider updated successfully!" });
            }
            catch (Exception ex)
            {
                // log ex if needed
                return Json(new { success = false, message = "An unexpected error occurred. Please try again." });
            }
        }


        [HttpPost("delete/{id}")]

        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Delete(int id)

        {

            var slider = await _db.HeroSlider.FindAsync(id);

            if (slider == null)

                return Json(new { success = false, message = "Slider not found!" });

            // ✅ Delete slider image from folder

            if (!string.IsNullOrEmpty(slider.Image))

            {

                string filePath = Path.Combine(_env.WebRootPath, slider.Image.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

                if (System.IO.File.Exists(filePath))

                    System.IO.File.Delete(filePath);

            }

            _db.HeroSlider.Remove(slider);

            await _db.SaveChangesAsync();

            return Json(new { success = true, message = "Home Slider deleted successfully!" });

        }

    }
}
