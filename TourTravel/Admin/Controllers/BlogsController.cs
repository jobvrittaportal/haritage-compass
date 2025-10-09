using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TourTravel.Models;

namespace TourTravel.Admin.Controllers
{
  [Authorize]
  [Route("admin/blogs")]
  public class BlogsController : Controller
  {
    private readonly MyDbContext _db;
    private readonly IWebHostEnvironment _env;

    public BlogsController(MyDbContext db, IWebHostEnvironment env)
    {
      _db = db;
      _env = env;
    }

    //  Blog List + Search + Pagination
    [HttpGet]
    public IActionResult Index(string? search, int page = 1, int pageSize = 5)
    {
      var query = _db.Blog.AsQueryable();

      // 🔍 Search filter by Title or Author
      if (!string.IsNullOrEmpty(search))
      {
        query = query.Where(b => b.Title.Contains(search) || b.Author.Contains(search));
      }

      int totalItems = query.Count();
      var blogs = query
          .OrderByDescending(b => b.Id)
          .Skip((page - 1) * pageSize)
          .Take(pageSize)
          .ToList();

      ViewBag.Search = search;
      ViewBag.Page = page;
      ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

      return View("~/Views/Admin/Blogs/Index.cshtml", blogs);
    }

    // ✅ Get single blog for editing (AJAX)
    [HttpGet("GetBlogById")]
    public IActionResult GetBlogById(int id)
    {
      var blog = _db.Blog.Find(id);
      if (blog == null)
        return Json(new { success = false, message = "Blog not found" });

      return Json(new
      {
        success = true,
        id = blog.Id,
        title = blog.Title,
        author = blog.Author,
        content = blog.Content,
        imageUrl = blog.ImageUrl
      });
    }

    // ✅ Create blog (AJAX)
    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Blog model, IFormFile? imageUrl)
    {
      try
      {

        if (imageUrl == null || imageUrl.Length == 0)
        {
          return Json(new { success = false, message = "Please upload an image for the blog." });
        }


        string folderPath = Path.Combine(_env.WebRootPath, "Blogs");
        Directory.CreateDirectory(folderPath);

        string uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(imageUrl.FileName)}";
        string filePath = Path.Combine(folderPath, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
          await imageUrl.CopyToAsync(stream);
        }

        model.ImageUrl = "/Blogs/" + uniqueFileName;


        _db.Blog.Add(model);
        await _db.SaveChangesAsync();

        return Json(new { success = true, message = "Blog created successfully" });
      }
      catch (Exception ex)
      {
        return Json(new { success = false, message = "Error: " + ex.Message });
      }
    }

    // ✅ Edit blog (AJAX)
    [HttpPost("Edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Blog model, IFormFile? imageUrl)
    {
      try
      {
        var existing = _db.Blog.Find(model.Id);
        if (existing == null)
          return Json(new { success = false, message = "Blog not found" });

        existing.Title = model.Title;
        existing.Author = model.Author;
        existing.Content = model.Content;

        // ✅ Only update image if a new file is uploaded
        if (imageUrl != null && imageUrl.Length > 0)
        {
          string folderPath = Path.Combine(_env.WebRootPath, "Blogs");
          Directory.CreateDirectory(folderPath);

          string uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(imageUrl.FileName)}";
          string filePath = Path.Combine(folderPath, uniqueFileName);

          using (var stream = new FileStream(filePath, FileMode.Create))
          {
            await imageUrl.CopyToAsync(stream);
          }

          // Optional: delete old image if it exists
          if (!string.IsNullOrEmpty(existing.ImageUrl))
          {
            string oldFilePath = Path.Combine(_env.WebRootPath, existing.ImageUrl.TrimStart('/'));
            if (System.IO.File.Exists(oldFilePath))
              System.IO.File.Delete(oldFilePath);
          }

          existing.ImageUrl = "/Blogs/" + uniqueFileName;
        }

        _db.Update(existing);
        await _db.SaveChangesAsync();

        return Json(new { success = true, message = "Blog updated successfully" });
      }
      catch (Exception ex)
      {
        return Json(new { success = false, message = "Error: " + ex.Message });
      }
    }


    // ✅ Delete blog (non-AJAX)
    [HttpPost("Delete/{id}")]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
      var blog = _db.Blog.Find(id);
      if (blog == null)
        return NotFound();

      _db.Blog.Remove(blog);
      _db.SaveChanges();

      return RedirectToAction(nameof(Index));
    }
  }
}
