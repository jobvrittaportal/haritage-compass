using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourTravel.Models;

namespace TourTravel.Admin.Controllers
{
    [Route("admin/[controller]")]
    public class GalleryController : Controller
    {
        private readonly MyDbContext _db;
        private readonly IWebHostEnvironment _env;

        public GalleryController(MyDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        // ✅ List all images
        [HttpGet]
        public async Task<IActionResult> Index(string? search, int page = 1, int pageSize = 5)
        {
            var query = _db.Gallery.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(b => b.Title.Contains(search));
            }

            int totalItems = await query.CountAsync();

            var images = await query
                .OrderByDescending(b => b.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.Search = search;
            ViewBag.Page = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            return View("~/Views/Admin/Gallery/Index.cshtml", images);
        }


        // ✅ Create Page
        [HttpGet("create")]
        public IActionResult Create()
        {
            return View("~/Views/Admin/Gallery/Create.cshtml");
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Gallery model, IFormFile? imageUrl)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.Title))
                    return Json(new { success = false, message = "Title is required." });

                if (imageUrl == null || imageUrl.Length == 0)
                    return Json(new { success = false, message = "Please select an image to upload." });

                // Validate folder
                string folderPath = Path.Combine(_env.WebRootPath, "Gallery");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                // Save image
                string uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(imageUrl.FileName)}";
                string filePath = Path.Combine(folderPath, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    imageUrl.CopyTo(stream);
                }

                model.ImageUrl = "/Gallery/" + uniqueFileName;
                _db.Gallery.Add(model);
                _db.SaveChanges();

                return Json(new { success = true, message = "Image uploaded successfully!" });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "An error occurred while uploading the image." });
            }
        }


        // ✅ Edit Page
        [HttpGet("edit/{id}")]
        public IActionResult Edit(int id)
        {
            var image = _db.Gallery.Find(id);
            if (image == null) return NotFound();
            return View("~/Views/Admin/Gallery/Edit.cshtml", image);
        }

        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Gallery model, IFormFile? newImage)
        {
            try
            {
                var existing = _db.Gallery.Find(id);
                if (existing == null)
                    return Json(new { success = false, message = "Image not found!" });

                // ✅ Validate title duplication
                bool titleExists = _db.Gallery.Any(g => g.Id != id && g.Title.Trim().ToLower() == model.Title.Trim().ToLower());
                if (titleExists)
                    return Json(new { success = false, message = "This title already exists. Please choose a different one." });

                // ✅ Update title
                existing.Title = model.Title;

                // ✅ Handle new image upload (if provided)
                if (newImage != null && newImage.Length > 0)
                {
                    string folderPath = Path.Combine(_env.WebRootPath, "Gallery");
                    Directory.CreateDirectory(folderPath);

                    string uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(newImage.FileName)}";
                    string filePath = Path.Combine(folderPath, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        newImage.CopyTo(stream);
                    }

                    // ✅ Delete old image safely
                    if (!string.IsNullOrEmpty(existing.ImageUrl))
                    {
                        string oldPath = Path.Combine(_env.WebRootPath, existing.ImageUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                        if (System.IO.File.Exists(oldPath))
                            System.IO.File.Delete(oldPath);
                    }

                    existing.ImageUrl = "/Gallery/" + uniqueFileName;
                }

                _db.SaveChanges();
                return Json(new { success = true, message = "Gallery image updated successfully!" });
            }
            catch (Exception ex)
            {
                // Optional: log ex.Message
                return Json(new { success = false, message = "An error occurred while updating the image. Please try again." });
            }
        }

        // ✅ Delete
        // ✅ Delete
        [HttpPost("delete/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var image = _db.Gallery.Find(id);
            if (image == null)
                return Json(new { success = false, message = "Image not found!" });

            // Delete file from wwwroot
            if (!string.IsNullOrEmpty(image.ImageUrl))
            {
                string filePath = Path.Combine(_env.WebRootPath, image.ImageUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
            }

            _db.Gallery.Remove(image);
            _db.SaveChanges();

            return Json(new { success = true, message = "Image deleted successfully!" });
        }
    }
}
