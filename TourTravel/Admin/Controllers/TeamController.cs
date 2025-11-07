using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourTravel.Models;

namespace TourTravel.Controllers.Admin
{
    [Route("admin/[controller]")]
    public class TeamController : Controller
    {
        private readonly MyDbContext _db;
        private readonly IWebHostEnvironment _env;

        public TeamController(MyDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string? search, int page = 1, int pageSize = 5)
        {
            var query = _db.Team.AsQueryable();

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

            return View("~/Views/Admin/Team/Index.cshtml", list);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View("~/Views/Admin/Team/Create.cshtml");
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Team model, IFormFile? ImageFile)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Invalid data." });

            // ✅ Image validation
            if (ImageFile == null || ImageFile.Length == 0)
            {
                return Json(new { success = false, message = "Profile image is required." });
            }

            // ✅ Email duplicate check
            if (!string.IsNullOrEmpty(model.Email))
            {
                bool emailExists = await _db.Team
                    .AnyAsync(t => t.Email.Trim().ToLower() == model.Email.Trim().ToLower());

                if (emailExists)
                    return Json(new { success = false, message = "Email already exists." });
            }

            // ✅ Phone duplicate check
            if (!string.IsNullOrEmpty(model.Phone))
            {
                bool phoneExists = await _db.Team
                    .AnyAsync(t => t.Phone.Trim() == model.Phone.Trim());

                if (phoneExists)
                    return Json(new { success = false, message = "Phone number already exists." });
            }

            // ✅ Image upload
            try
            {
                string folderPath = Path.Combine(_env.WebRootPath, "uploads", "Team");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                string fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
                string filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                    await ImageFile.CopyToAsync(stream);

                model.ImageUrl = "/uploads/Team/" + fileName;
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Image upload failed: " + ex.Message });
            }

            // ✅ Save to DB
            try
            {
                _db.Team.Add(model);
                await _db.SaveChangesAsync();
                return Json(new { success = true, message = "Team member created successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error saving team: " + ex.Message });
            }
        }


        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var guide = await _db.Team.FindAsync(id);
            if (guide == null) return NotFound();

            return View("~/Views/Admin/Team/Edit.cshtml", guide);
        }

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TourGuideView model, IFormFile? ImageFile)
        {
            var existing = await _db.Team.FindAsync(id);
            if (existing == null) return NotFound();

            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(model.Email))
                {
                    bool emailExists = await _db.Team
                        .AnyAsync(t => t.Email == model.Email && t.Id != id);
                    if (emailExists)
                    {
                        ModelState.AddModelError("Email", "Email already exists.");
                        return View("~/Views/Admin/Team/Edit.cshtml", model);
                    }
                }

                if (!string.IsNullOrEmpty(model.Phone))
                {
                    bool phoneExists = await _db.Team
                        .AnyAsync(t => t.Phone == model.Phone && t.Id != id);
                    if (phoneExists)
                    {
                        ModelState.AddModelError("Phone", "Phone number already exists.");
                        return View("~/Views/Admin/Team/Edit.cshtml", model);
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
                    string folderPath = Path.Combine(_env.WebRootPath, "uploads", "Team");
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);

                    string fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
                    string filePath = Path.Combine(folderPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }

                    existing.ImageUrl = "/uploads/Team/" + fileName;
                }

                await _db.SaveChangesAsync();

                TempData["success"] = "Team updated successfully!";
                return RedirectToAction("Index");
            }

            return View("~/Views/Admin/Team/Edit.cshtml", model);
        }

        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var guide = await _db.Team.FindAsync(id);
            if (guide == null)
                return Json(new { success = false, message = "Team not found" });

            _db.Team.Remove(guide);
            await _db.SaveChangesAsync();

            return Json(new { success = true, message = " Team delete successfully"});
        }
    }
}
