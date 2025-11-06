using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourTravel.Models;

namespace TourTravel.Controllers.Admin
{
    [Route("admin/[controller]")]
    public class TourGuideController : Controller
    {
        private readonly MyDbContext _db;
        private readonly IWebHostEnvironment _env;

        public TourGuideController(MyDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? search, int page = 1, int pageSize = 5)
        {
            var query = _db.TourGuideView.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(t => t.Name.Contains(search) || t.Email!.Contains(search));
            }

            int totalItems = await query.CountAsync();
            var list = await query
                .OrderByDescending(t => t.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.Search = search;
            ViewBag.Page = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            return View("~/Views/Admin/TourGuide/Index.cshtml", list);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View("~/Views/Admin/TourGuide/Create.cshtml");
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TourGuideView model, IFormFile? ImageFile)
        {
            if (ModelState.IsValid)
            {
                // Check email duplicate
                if (!string.IsNullOrEmpty(model.Email))
                {
                    bool emailExists = await _db.TourGuideView.AnyAsync(t => t.Email == model.Email);
                    if (emailExists)
                    {
                        ModelState.AddModelError("Email", "Email already exists.");
                        return View("~/Views/Admin/TourGuide/Create.cshtml", model);
                    }
                }

                // Check phone duplicate
                if (!string.IsNullOrEmpty(model.Phone))
                {
                    bool phoneExists = await _db.TourGuideView.AnyAsync(t => t.Phone == model.Phone);
                    if (phoneExists)
                    {
                        ModelState.AddModelError("Phone", "Phone number already exists.");
                        return View("~/Views/Admin/TourGuide/Create.cshtml", model);
                    }
                }

                // Image upload
                if (ImageFile != null)
                {
                    string folderPath = Path.Combine(_env.WebRootPath, "uploads", "TourGuide");
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);

                    string fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
                    string filePath = Path.Combine(folderPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }

                    model.ImageUrl = "/uploads/TourGuide/" + fileName;
                }

                _db.TourGuideView.Add(model);
                await _db.SaveChangesAsync();

                TempData["success"] = "Tour Guide created successfully!";
                return RedirectToAction(nameof(Index));
            }

            return View("~/Views/Admin/TourGuide/Create.cshtml", model);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var guide = await _db.TourGuideView.FindAsync(id);
            if (guide == null) return NotFound();

            return View("~/Views/Admin/TourGuide/Edit.cshtml", guide);
        }

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TourGuideView model, IFormFile? ImageFile)
        {
            var existing = await _db.TourGuideView.FindAsync(id);
            if (existing == null) return NotFound();

            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(model.Email))
                {
                    bool emailExists = await _db.TourGuideView
                        .AnyAsync(t => t.Email == model.Email && t.Id != id);
                    if (emailExists)
                    {
                        ModelState.AddModelError("Email", "Email already exists.");
                        return View("~/Views/Admin/TourGuide/Edit.cshtml", model);
                    }
                }

                if (!string.IsNullOrEmpty(model.Phone))
                {
                    bool phoneExists = await _db.TourGuideView
                        .AnyAsync(t => t.Phone == model.Phone && t.Id != id);
                    if (phoneExists)
                    {
                        ModelState.AddModelError("Phone", "Phone number already exists.");
                        return View("~/Views/Admin/TourGuide/Edit.cshtml", model);
                    }
                }

                existing.Name = model.Name;
                existing.Role = model.Role;
                existing.Email = model.Email;
                existing.Phone = model.Phone;
                existing.Facebook = model.Facebook;
                existing.Twitter = model.Twitter;
                existing.Instagram = model.Instagram;
                existing.LinkedIn = model.LinkedIn;
                existing.YouTube = model.YouTube;
                existing.Introduction = model.Introduction;
                existing.Biography = model.Biography;

                if (ImageFile != null)
                {
                    string folderPath = Path.Combine(_env.WebRootPath, "uploads", "TourGuide");
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);

                    string fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
                    string filePath = Path.Combine(folderPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }

                    existing.ImageUrl = "/uploads/TourGuide/" + fileName;
                }

                await _db.SaveChangesAsync();

                TempData["success"] = "Tour Guide updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            return View("~/Views/Admin/TourGuide/Edit.cshtml", model);
        }

        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var guide = await _db.TourGuideView.FindAsync(id);
            if (guide == null)
            {
                TempData["Error"] = "Tour guide not found.";
                return RedirectToAction("Index");
            }

            // Delete the image file if it exists
            if (!string.IsNullOrEmpty(guide.ImageUrl))
            {
                string filePath = Path.Combine(_env.WebRootPath, guide.ImageUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
            }

            _db.TourGuideView.Remove(guide);
            await _db.SaveChangesAsync();
            return Json(new { success = true, message = " Tour Guide Successfully Deleted" });
            //return RedirectToAction(nameof(Index));
        }

    }
}
