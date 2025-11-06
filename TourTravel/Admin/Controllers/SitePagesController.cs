using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourTravel.Models;

namespace TourTravel.Controllers.Admin
{
    [Route("admin/[controller]")]
    public class SitePagesController : Controller
    {
        private readonly MyDbContext _db;
        private readonly IWebHostEnvironment _env;

        public SitePagesController(MyDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? search, int page = 1, int pageSize =5)
        {
            var query = _db.SitePages.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.Page.Contains(search) || p.Title.Contains(search) || p.KeyWords.Contains(search));
            }

            int totalItems = await query.CountAsync();
            var list = await query
                .OrderByDescending(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.Search = search;
            ViewBag.Page = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            return View("~/Views/Admin/SitePages/Index.cshtml", list);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View("~/Views/Admin/SitePages/Create.cshtml");
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SitePages model, IFormFile? ImageFile)
        {
            if (ImageFile == null)
                ModelState.AddModelError("Image", "The Image field is required.");

            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(model.Title))
                {
                    bool TitleExists = await _db.SitePages.AnyAsync(t => t.Title == model.Title);
                    if (TitleExists)
                    {
                        ModelState.AddModelError("Title", "Title already exists.");
                        return View("~/Views/Admin/SitePages/Create.cshtml", model);
                    }
                }
                // Save image file
                string folderPath = Path.Combine(_env.WebRootPath, "uploads", "SitePages");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                string fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
                string filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                model.Image = "/uploads/SitePages/" + fileName;

                _db.SitePages.Add(model);
                await _db.SaveChangesAsync();

                TempData["success"] = "Site Page created successfully!";
                return RedirectToAction(nameof(Index));
            }

            return View("~/Views/Admin/SitePages/Create.cshtml", model);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var page = await _db.SitePages.FindAsync(id);
            if (page == null) return NotFound();

            return View("~/Views/Admin/SitePages/Edit.cshtml", page);
        }

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SitePages model, IFormFile? ImageFile)
        {
            var existing = await _db.SitePages.FindAsync(id);
            if (existing == null) return NotFound();

            if (ModelState.IsValid)
            {
                bool TitleExists = await _db.SitePages.AnyAsync(t => t.Title.Trim().ToLower() == model.Title.Trim().ToLower() && t.Id != model.Id);
                if (TitleExists)
                {
                    ModelState.AddModelError("Title", "Title already exists.");
                    return View("~/Views/Admin/SitePages/Edit.cshtml", model);
                }
                existing.Page = model.Page;
                existing.Title = model.Title;
                existing.Description = model.Description;
                existing.KeyWords = model.KeyWords;
                existing.ImgWidth = model.ImgWidth;
                existing.ImgHeight = model.ImgHeight;

                if (ImageFile != null)
                {
                    string folderPath = Path.Combine(_env.WebRootPath, "uploads", "SitePages");
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);

                    string fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
                    string filePath = Path.Combine(folderPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }

                    existing.Image = "/uploads/SitePages/" + fileName;
                }

                await _db.SaveChangesAsync();

                TempData["success"] = "Site Page updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            return View("~/Views/Admin/SitePages/Edit.cshtml", model);
        }

        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var page = await _db.SitePages.FindAsync(id);
            if (page == null)
                return Json(new { success = false, message = "Page not found" });

            _db.SitePages.Remove(page);
            await _db.SaveChangesAsync();

            return Json(new { success = true });
        }
    }
}
