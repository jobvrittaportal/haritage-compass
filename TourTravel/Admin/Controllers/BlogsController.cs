using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourTravel.Models;
using System.Drawing; // For System.Drawing.Image


namespace TourTravel.Admin.Controllers
{
    [Authorize]
    [Route("admin/Blogs")]
    public class BlogsController : Controller
    {
        private readonly MyDbContext _db;
        private readonly IWebHostEnvironment _env;

        public BlogsController(MyDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        // ✅ Blog List + Search + Pagination
        [HttpGet]
        public async Task<IActionResult> Index(string? search, int page = 1, int pageSize = 5)
        {
            var query = _db.Blog.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(b => b.Title.Contains(search) || b.Author.Contains(search));
            }
            var result = query.ToList();

            if (result.Count == 0)
            {
                ViewBag.Message = "No records found.";
            }

            int totalItems = await query.CountAsync();
            var blogs = await query
                .OrderByDescending(b => b.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.Search = search;
            ViewBag.Page = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            return View("~/Views/Admin/Blogs/Index.cshtml", blogs);
        }

        // ✅ Render Create Form
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View("~/Views/Admin/Blogs/Create.cshtml");
        }

        // ✅ Create Blog (AJAX)
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Blog model, IFormFile? image)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.Title) || string.IsNullOrWhiteSpace(model.Content))
                    return Json(new { success = false, message = "Please fill all required fields." });

                if (image == null || image.Length == 0)
                    return Json(new { success = false, message = "Please upload a blog image." });

                bool slugExists = await _db.Blog.AnyAsync(b => b.SlugUrl.ToLower().Trim() == model.SlugUrl.ToLower().Trim());
                if (slugExists)
                    return Json(new { success = false, message = "Slug URL already exists." });

                bool titleexist = await _db.Blog.AnyAsync(b => b.Title.ToLower().Trim() == model.Title.ToLower().Trim());
                if (titleexist)
                    return Json(new { success = false, message = "Title already exists." });


                // ✅ Validate file size (max 5MB)
                if (image.Length > 5 * 1024 * 1024)
                    return Json(new { success = false, message = "Image size must be less than 5MB." });

                // ✅ Validate dimensions
                using (var stream = image.OpenReadStream())
                using (var img = Image.FromStream(stream))
                {
                    if (img.Width != 800 || img.Height != 600)
                        return Json(new { success = false, message = "Image dimensions must be 800x600." });
                }

                string folderPath = Path.Combine(_env.WebRootPath, "Blogs");
                Directory.CreateDirectory(folderPath);

                string uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
                string filePath = Path.Combine(folderPath, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                model.ImageUrl = "/Blogs/" + uniqueFileName;

                _db.Blog.Add(model);
                await _db.SaveChangesAsync();

                return Json(new { success = true, message = "Blog created successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        // ✅ Render Edit Form
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var blog = await _db.Blog.FindAsync(id);
            if (blog == null) return NotFound();

            return View("~/Views/Admin/Blogs/Edit.cshtml", blog);
        }

        // ✅ Edit Blog (AJAX)
        [HttpPost("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Blog model, IFormFile? image)
        {
            try
            {
                var existing = await _db.Blog.FindAsync(model.Id);
                if (existing == null)
                    return Json(new { success = false, message = "Record not found." });

                //if (image == null || image.Length == 0)
                //    return Json(new { success = false, message = "Please upload a blog image." });

                bool slugExists = await _db.Blog.AnyAsync(b => b.Id != model.Id && b.SlugUrl.ToLower().Trim() == model.SlugUrl.ToLower().Trim());

                if (slugExists)
                    return Json(new { success = false, message = "Slug URL already exists." });


                bool titleexist = await _db.Blog.AnyAsync(b => b.Id != model.Id && b.Title.ToLower().Trim() == model.Title.ToLower().Trim());

                if (titleexist)
                {
                    return Json(new { success = false, message = "This Title already exists. Please use a different one." });
                }

                existing.Title = model.Title;
                existing.Content = model.Content;
                existing.Author = model.Author;
                existing.MetaTitle = model.MetaTitle;
                existing.MetaDescription = model.MetaDescription;
                existing.SlugUrl = model.SlugUrl;

                if (image != null && image.Length > 0)
                {
                    string folderPath = Path.Combine(_env.WebRootPath, "Blogs");
                    Directory.CreateDirectory(folderPath);

                    string uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
                    string filePath = Path.Combine(folderPath, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    if (!string.IsNullOrEmpty(existing.ImageUrl))
                    {
                        string oldPath = Path.Combine(_env.WebRootPath, existing.ImageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(oldPath))
                            System.IO.File.Delete(oldPath);
                    }

                    existing.ImageUrl = "/Blogs/" + uniqueFileName;
                }

                _db.Update(existing);
                await _db.SaveChangesAsync();

                return Json(new { success = true, message = "Blog updated successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        // ✅ Delete Blog
        // ✅ Delete Blog
        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var blog = await _db.Blog.FindAsync(id);
            if (blog == null) return NotFound();

            if (!string.IsNullOrEmpty(blog.ImageUrl))
            {
                string filePath = Path.Combine(_env.WebRootPath, blog.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
            }

            _db.Blog.Remove(blog);
            await _db.SaveChangesAsync();

            return Json(new { success = true, message = "Blog deleted successfully!" });
        }
    }
}
