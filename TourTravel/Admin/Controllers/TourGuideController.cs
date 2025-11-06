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
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Invalid data." });

            if (ImageFile == null || ImageFile.Length == 0)
            {
                return Json(new { success = false, message = "Profile image is required." });
            }

            // Email duplicate check
            if (!string.IsNullOrEmpty(model.Email))
            {
                bool emailExists = await _db.TourGuideView.AnyAsync(t => t.Email == model.Email);
                if (emailExists)
                    return Json(new { success = false, message = "Email already exists." });
            }

            // Phone duplicate check
            if (!string.IsNullOrEmpty(model.Phone))
            {
                bool phoneExists = await _db.TourGuideView.AnyAsync(t => t.Phone == model.Phone);
                if (phoneExists)
                    return Json(new { success = false, message = "Phone number already exists." });
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
                    await ImageFile.CopyToAsync(stream);

                model.ImageUrl = "/uploads/TourGuide/" + fileName;
            }

            _db.TourGuideView.Add(model);
            await _db.SaveChangesAsync();

            return Json(new { success = true, message = "Tour Guide created successfully!" });
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
            try
            {
                var existing = await _db.TourGuideView.FindAsync(id);
                if (existing == null)
                    return Json(new { success = false, message = "Tour Guide not found." });

                if (!ModelState.IsValid)
                    return Json(new { success = false, message = "Invalid data submitted." });

                // ✅ Duplicate checks (using model.Id instead of id)
                if (!string.IsNullOrEmpty(model.Email))
                {
                    bool emailExists = await _db.TourGuideView
                        .AnyAsync(t => t.Email.Trim().ToLower() == model.Email.Trim().ToLower()
                                    && t.Id != model.Id);
                    if (emailExists)
                        return Json(new { success = false, message = "Email already exists!" });
                }

                if (!string.IsNullOrEmpty(model.Phone))
                {
                    bool phoneExists = await _db.TourGuideView
                        .AnyAsync(t => t.Phone.Trim() == model.Phone.Trim()
                                    && t.Id != model.Id);
                    if (phoneExists)
                        return Json(new { success = false, message = "Phone number already exists!" });
                }

                // ✅ Update basic fields
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

                // ✅ Handle image upload
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    string uploadDir = Path.Combine(_env.WebRootPath, "uploads", "TourGuide");
                    if (!Directory.Exists(uploadDir))
                        Directory.CreateDirectory(uploadDir);

                    string fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
                    string filePath = Path.Combine(uploadDir, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                        await ImageFile.CopyToAsync(stream);

                    if (!string.IsNullOrEmpty(existing.ImageUrl))
                    {
                        var oldPath = Path.Combine(_env.WebRootPath, existing.ImageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(oldPath))
                            System.IO.File.Delete(oldPath);
                    }

                    existing.ImageUrl = "/uploads/TourGuide/" + fileName;
                }

                await _db.SaveChangesAsync();
                return Json(new { success = true, message = "Tour Guide updated successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }




        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var guide = await _db.TourGuideView.FindAsync(id);
                if (guide == null)
                    return Json(new { success = false, message = "Tour guide not found." });

                // ✅ Delete the image file if it exists
                if (!string.IsNullOrEmpty(guide.ImageUrl))
                {
                    string filePath = Path.Combine(_env.WebRootPath, guide.ImageUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);
                }

                _db.TourGuideView.Remove(guide);
                await _db.SaveChangesAsync();

                return Json(new { success = true, message = "Tour guide successfully deleted." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error deleting tour guide: " + ex.Message });
            }
        }

    }
}
