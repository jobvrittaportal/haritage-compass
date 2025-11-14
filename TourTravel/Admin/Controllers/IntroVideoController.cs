using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourTravel.Models;

namespace TourTravel.Admin.Controllers
{
    [Route("admin/[controller]")]
    public class IntroVideoController : Controller
    {
        private readonly MyDbContext _db;
        private readonly IWebHostEnvironment _env;

        public IntroVideoController(MyDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;

        }

        [HttpGet]
        public async Task<IActionResult> Index(string? search, int page = 1, int pageSize = 5)
        {
            var query = _db.IntroVideo.AsQueryable();


            //if (!string.IsNullOrEmpty(search))
            //{
            //    query = query.Where(t => t.Title.Contains(search));
            //}

            int totalItems = await query.CountAsync();
            var list = await query
                .OrderByDescending(t => t.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.Search = search;
            ViewBag.Page = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            return View("~/Views/Admin/IntroVideo/Index.cshtml", list);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View("~/Views/Admin/IntroVideo/Create.cshtml");
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IntroVideo model, IFormFile? ImageFile)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Invalid form data." });

            if (ImageFile == null || ImageFile.Length == 0)
                return Json(new { success = false, message = "Please upload a thumbnail image." });

            if (string.IsNullOrWhiteSpace(model.VideoUrl))
                return Json(new { success = false, message = "Please provide a video URL." });

            try
            {
                // ✅ Create upload folder (wwwroot/uploads/introvideo)
                string folderPath = Path.Combine(_env.WebRootPath, "uploads", "introvideo");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                // ✅ Save image file
                string fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
                string filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

               
                model.ImageUrl = "/uploads/introvideo/" + fileName;

                
                _db.IntroVideo.Add(model);
                await _db.SaveChangesAsync();

                return Json(new { success = true, message = " Created successfully!" });
            }
            catch (Exception ex)
            {
                
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        [HttpGet("edit/{id}")]
        public IActionResult Edit(int id)
        {
            var item = _db.IntroVideo.Find(id);
            if (item == null)
                return NotFound();

            return View("~/Views/Admin/IntroVideo/Edit.cshtml", item);
        }

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IntroVideo model, IFormFile? newImage)
        {
            try
            {
                var item = await _db.IntroVideo.FindAsync(id);
                if (item == null)
                    return Json(new { success = false, message = "Record not found." });

                
                item.Title = model.Title;
                item.Description = model.Description;
                item.VideoUrl = model.VideoUrl;

                // ✅ Handle new image upload (optional)
                if (newImage != null && newImage.Length > 0)
                {
                    string folderPath = Path.Combine(_env.WebRootPath, "uploads", "IntroVideo");
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);

                    string fileName = $"{Guid.NewGuid()}{Path.GetExtension(newImage.FileName)}";
                    string filePath = Path.Combine(folderPath, fileName);

                   
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await newImage.CopyToAsync(stream);
                    }

                    // Delete old image if exists
                    if (!string.IsNullOrEmpty(item.ImageUrl))
                    {
                        string oldPath = Path.Combine(_env.WebRootPath, item.ImageUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                        if (System.IO.File.Exists(oldPath))
                            System.IO.File.Delete(oldPath);
                    }

                    item.ImageUrl = "/uploads/IntroVideo/" + fileName;
                }

                _db.IntroVideo.Update(item);
                await _db.SaveChangesAsync();

                return Json(new { success = true, message = "Updated successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }

        }

        [HttpPost("delete/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var video = _db.IntroVideo.Find(id);
            if (video == null)
                return Json(new { success = false, message = "Video not found!" });

            // ✅ Delete file from wwwroot if exists
            if (!string.IsNullOrEmpty(video.ImageUrl))
            {
                var filePath = Path.Combine(_env.WebRootPath, video.ImageUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            _db.IntroVideo.Remove(video);
            _db.SaveChanges();

            return Json(new { success = true, message = "deleted successfully!" });
        }

    }
}
