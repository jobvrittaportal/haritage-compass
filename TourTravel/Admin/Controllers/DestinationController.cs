using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourTravel.Models;

namespace TourTravel.Admin.Controllers
{
    [Authorize]
    [Route("admin/Destination")]
    public class DestinationController : Controller
    {
        private readonly MyDbContext _db;
        private readonly IWebHostEnvironment _env;

        public DestinationController(MyDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? search, int page = 1, int pageSize = 5)
        {
            var query = _db.Destinations.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(d => d.DestinationName.Contains(search) || d.SlugUrl!.Contains(search));
            }

            int totalItems = await query.CountAsync();
            var list = await query
                .OrderByDescending(d => d.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.Search = search;
            ViewBag.Page = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            return View("~/Views/Admin/Destination/Index.cshtml", list);
        }

        // Render Create Form
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View("~/Views/Admin/Destination/Create.cshtml");
        }

        //Create Destination (AJAX)
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Destinations model, IFormFile? DestinationImage)
        {
            try
            {
                // Basic validation
                if (string.IsNullOrWhiteSpace(model.DestinationName) ||
                    string.IsNullOrWhiteSpace(model.MetaTitle) ||
                    string.IsNullOrWhiteSpace(model.MetaDescription) ||
                    DestinationImage == null || DestinationImage.Length == 0)
                {
                    return Json(new { success = false, message = "Please fill all required fields and upload an image." });
                }

                // Slug uniqueness (if provided)
                if (!string.IsNullOrWhiteSpace(model.SlugUrl))
                {
                    bool slugExists = await _db.Destinations
                        .AnyAsync(d => d.SlugUrl!.ToLower() == model.SlugUrl!.ToLower());
                    if (slugExists)
                        return Json(new { success = false, message = "Slug URL already exists. Please use a different one." });
                }

                // Save main image
                string folderPath = Path.Combine(_env.WebRootPath, "Destination");
                Directory.CreateDirectory(folderPath);

                string uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(DestinationImage.FileName)}";
                string filePath = Path.Combine(folderPath, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await DestinationImage.CopyToAsync(stream);
                }

                model.DestinationImgUrl = "/Destination/" + uniqueFileName;

                _db.Destinations.Add(model);
                await _db.SaveChangesAsync();

                return Json(new { success = true, message = "Destination created successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        // Render Edit Form (includes gallery)
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var dest = await _db.Destinations.FindAsync(id);
            if (dest == null)
                return NotFound();

            // Load gallery
            var gallery = await _db.DestinationGallery
                .Where(g => g.DestinationId == id)
                .OrderByDescending(g => g.Id)
                .ToListAsync();

            ViewBag.Gallery = gallery;

            return View("~/Views/Admin/Destination/Edit.cshtml", dest);
        }

        // Edit Destination (AJAX)
        [HttpPost("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Destinations model, IFormFile? DestinationImage)
        {
            try
            {
                var existing = await _db.Destinations.FindAsync(model.Id);
                if (existing == null)
                    return Json(new { success = false, message = "Record not found." });

                // Slug uniqueness for other records
                if (!string.IsNullOrWhiteSpace(model.SlugUrl))
                {
                    bool slugExists = await _db.Destinations
                        .AnyAsync(d => d.Id != model.Id && d.SlugUrl!.ToLower() == model.SlugUrl!.ToLower());
                    if (slugExists)
                        return Json(new { success = false, message = "Slug URL already exists. Please use a different one." });
                }

                existing.DestinationName = model.DestinationName;
                existing.DestinationInfo = model.DestinationInfo;
                existing.StartingPrice = model.StartingPrice;
                existing.MapUrl = model.MapUrl;
                existing.MetaTitle = model.MetaTitle;
                existing.MetaDescription = model.MetaDescription;
                existing.SlugUrl = model.SlugUrl;

                // Update image if new one uploaded
                if (DestinationImage != null && DestinationImage.Length > 0)
                {
                    string folderPath = Path.Combine(_env.WebRootPath, "Destination");
                    Directory.CreateDirectory(folderPath);

                    string uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(DestinationImage.FileName)}";
                    string filePath = Path.Combine(folderPath, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await DestinationImage.CopyToAsync(stream);
                    }

                    // Delete old image if exists
                    if (!string.IsNullOrEmpty(existing.DestinationImgUrl))
                    {
                        string oldPath = Path.Combine(_env.WebRootPath, existing.DestinationImgUrl.TrimStart('/'));
                        if (System.IO.File.Exists(oldPath))
                            System.IO.File.Delete(oldPath);
                    }

                    existing.DestinationImgUrl = "/Destination/" + uniqueFileName;
                }

                _db.Update(existing);
                await _db.SaveChangesAsync();

                return Json(new { success = true, message = "Destination updated successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        // Delete Destination (also delete main image and related gallery files)
        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var dest = await _db.Destinations.FindAsync(id);
            if (dest == null)
                return NotFound();

            // Delete main image
            if (!string.IsNullOrEmpty(dest.DestinationImgUrl))
            {
                string filePath = Path.Combine(_env.WebRootPath, dest.DestinationImgUrl.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
            }

            // Delete gallery files and records
            var galleries = await _db.DestinationGallery.Where(g => g.DestinationId == id).ToListAsync();
            foreach (var g in galleries)
            {
                if (!string.IsNullOrEmpty(g.GalleryImgUrl))
                {
                    string gPath = Path.Combine(_env.WebRootPath, g.GalleryImgUrl.TrimStart('/'));
                    if (System.IO.File.Exists(gPath))
                        System.IO.File.Delete(gPath);
                }
            }
            _db.DestinationGallery.RemoveRange(galleries);

            _db.Destinations.Remove(dest);
            await _db.SaveChangesAsync();

            TempData["Success"] = "Destination deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        // =========================
        // Gallery Endpoints
        // =========================

        // Upload one or multiple gallery images for a destination (AJAX)
        [HttpPost("UploadGallery/{destinationId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadGallery(int destinationId, List<IFormFile> galleryFiles)
        {
            try
            {
                var dest = await _db.Destinations.FindAsync(destinationId);
                if (dest == null)
                    return Json(new { success = false, message = "Destination not found." });

                if (galleryFiles == null || galleryFiles.Count == 0)
                    return Json(new { success = false, message = "No files selected." });

                string galleryFolder = Path.Combine(_env.WebRootPath, "Destination", "Gallery");
                Directory.CreateDirectory(galleryFolder);

                var saved = new List<object>();

                foreach (var file in galleryFiles)
                {
                    if (file.Length <= 0) continue;

                    string uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                    string filePath = Path.Combine(galleryFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    var gallery = new DestinationGallery
                    {
                        DestinationId = destinationId,
                        GalleryImgUrl = "/Destination/Gallery/" + uniqueFileName
                    };

                    _db.DestinationGallery.Add(gallery);
                    await _db.SaveChangesAsync();

                    saved.Add(new { id = gallery.Id, url = gallery.GalleryImgUrl });
                }

                return Json(new { success = true, message = "Gallery uploaded successfully.", data = saved });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        // Get gallery images (AJAX) for a destination when clicked + icon 
        [HttpGet("GetGallery/{destinationId}")]
        public async Task<IActionResult> GetGallery(int destinationId)
        {
            var gallery = await _db.DestinationGallery
                .Where(g => g.DestinationId == destinationId)
                .OrderByDescending(g => g.Id)
                .Select(g => new { g.Id, g.GalleryImgUrl })
                .ToListAsync();

            return Json(new { success = true, data = gallery });
        }

        //Delete a gallery image
        [HttpPost("DeleteGallery/{galleryId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteGallery(int galleryId)
        {
            var gallery = await _db.DestinationGallery.FindAsync(galleryId);
            if (gallery == null)
                return Json(new { success = false, message = "Gallery image not found." });

            // Delete file
            if (!string.IsNullOrEmpty(gallery.GalleryImgUrl))
            {
                string filePath = Path.Combine(_env.WebRootPath, gallery.GalleryImgUrl.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
            }

            _db.DestinationGallery.Remove(gallery);
            await _db.SaveChangesAsync();

            return Json(new { success = true, message = "Gallery image deleted." });
        }
    }
}
