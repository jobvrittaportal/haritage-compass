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
        public async Task<IActionResult> Index(string? search, int page = 1, int pageSize = 5)
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
            // Server-side validation for image
            if (ImageFile == null)
            {
                ModelState.AddModelError("Image", "The Image field is required.");
            }

            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(model.Title))
                {
                    bool titleExists = await _db.HeroSlider.AnyAsync(t => t.Title == model.Title);
                    if (titleExists)
                    {
                        ModelState.AddModelError("Title", "Title already exists.");
                        return View("~/Views/Admin/HeroSlider/Create.cshtml", model);
                    }
                }

                // Save image file
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

                TempData["success"] = "Hero Slider created successfully!";
                return RedirectToAction(nameof(Index));
            }

            return View("~/Views/Admin/HeroSlider/Create.cshtml", model);
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
            var existing = await _db.HeroSlider.FindAsync(id);
            if (existing == null) return NotFound();

            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(model.Title))
                {
                    bool titleExists = await _db.HeroSlider.AnyAsync(t => t.Title == model.Title);
                    if (titleExists)
                    {
                        ModelState.AddModelError("Title", "Title already exists.");
                        return View("~/Views/Admin/HeroSlider/Edit.cshtml", model);
                    }
                }
                existing.Title = model.Title;
                existing.SubTitle = model.SubTitle;
                existing.Description = model.Description;
                existing.RotationTime = model.RotationTime;

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

                    existing.Image = "/uploads/HeroSlider/" + fileName;
                }

                await _db.SaveChangesAsync();

                TempData["success"] = "Hero Slider updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            return View("~/Views/Admin/HeroSlider/Edit.cshtml", model);
        }

        [HttpPost("delete/{id}")]

        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Delete(int id)

        {

            var slider = await _db.HeroSlider.FindAsync(id);

            if (slider == null)

                return Json(new { success = false, message = "❌ Slider not found!" });

            // ✅ Delete slider image from folder

            if (!string.IsNullOrEmpty(slider.Image))

            {

                string filePath = Path.Combine(_env.WebRootPath, slider.Image.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

                if (System.IO.File.Exists(filePath))

                    System.IO.File.Delete(filePath);

            }

            _db.HeroSlider.Remove(slider);

            await _db.SaveChangesAsync();

            return Json(new { success = true, message = "✅ Slider deleted successfully!" });

        }

    }
}
