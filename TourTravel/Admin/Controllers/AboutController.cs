using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourTravel.Models;

namespace TourTravel.Admin.Controllers
{
  [Authorize]
  [Route("admin/About")]
  public class AboutController : Controller
  {
    private readonly MyDbContext _db;
    private readonly IWebHostEnvironment _env;

    public AboutController(MyDbContext db, IWebHostEnvironment env)
    {
      _db = db;
      _env = env;
    }

    //  Blog List + Search + Pagination
    [HttpGet]
    [Authorize]
    public IActionResult Index(string? search, int page = 1, int pageSize = 5)
    {
      var query = _db.About.AsQueryable();

      // 🔍 Search filter by Title or Author
      if (!string.IsNullOrEmpty(search))
      {
        query = query.Where(b => b.Title.Contains(search) );
      }

      int totalItems = query.Count();
      var about = query
          .OrderByDescending(b => b.Id)
          .Skip((page - 1) * pageSize)
          .Take(pageSize)
          .ToList();

      ViewBag.Search = search;
      ViewBag.Page = page;
      ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

      return View("~/Views/Admin/About/Index.cshtml", about);
    }

    // ✅ Get single blog for editing (AJAX)
    [HttpGet("GetAboutById")]
    [Authorize]
    public IActionResult GetAboutById(int id)
    {
      var about = _db.About.Find(id);
      if (about == null)
        return Json(new { success = false, message = "About not found" });

      return Json(new
      {
        success = true,
        id = about.Id,
        title = about.Title,
        description = about.Description,
        KeyFeatures = about.KeyFeatures,
        MetaTitle = about.MetaTitle,
        MetaDescription = about.MetaDescription,
        SlugUrl = about.SlugUrl,
        image = about.Image
      });
    }

    // ✅ Create blog (AJAX)
    [HttpPost("Create")]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(About model, IFormFile? image)
    {
      try
      {

        // ✅ Check if SlugUrl already exists (case-insensitive)
        bool slugExists = await _db.About
            .AnyAsync(b => b.SlugUrl.ToLower() == model.SlugUrl.ToLower());

        if (slugExists)
        {
          return Json(new { success = false, message = "Slug URL already exists. Please use a different one." });
        }
        if (image == null || image.Length == 0)
        {
          return Json(new { success = false, message = "Please upload an image for the About." });
        }


        string folderPath = Path.Combine(_env.WebRootPath, "About");
        Directory.CreateDirectory(folderPath);

        string uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
        string filePath = Path.Combine(folderPath, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
          await image.CopyToAsync(stream);
        }

        model.Image = "/About/" + uniqueFileName;


        _db.About.Add(model);
        await _db.SaveChangesAsync();

        return Json(new { success = true, message = "About created successfully" });
      }
      catch (Exception ex)
      {
        return Json(new { success = false, message = "Error: " + ex.Message });
      }
    }

    // ✅ Edit blog (AJAX)
    [HttpPost("Edit")]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(About model, IFormFile? image)
    {
      try
      {
        var existing = _db.About.Find(model.Id);
        if (existing == null)
          return Json(new { success = false, message = "Blog not found" });

        bool slugExists = await _db.About
         .AnyAsync(b => b.Id != model.Id && b.SlugUrl.ToLower() == model.SlugUrl.ToLower());

        if (slugExists)
        {
          return Json(new { success = false, message = "Slug URL already exists. Please use a different one." });
        }


        existing.Title = model.Title;
        existing.Description = model.Description;
        existing.MetaTitle = model.MetaTitle;
        existing.MetaDescription = model.MetaDescription;
        existing.SlugUrl = model.SlugUrl;


        // ✅ Only update image if a new file is uploaded
        if (image != null && image.Length > 0)
        {
          string folderPath = Path.Combine(_env.WebRootPath, "About");
          Directory.CreateDirectory(folderPath);

          string uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
          string filePath = Path.Combine(folderPath, uniqueFileName);

          using (var stream = new FileStream(filePath, FileMode.Create))
          {
            await image.CopyToAsync(stream);
          }

          // Optional: delete old image if it exists
          if (!string.IsNullOrEmpty(existing.Image))
          {
            string oldFilePath = Path.Combine(_env.WebRootPath, existing.Image.TrimStart('/'));
            if (System.IO.File.Exists(oldFilePath))
              System.IO.File.Delete(oldFilePath);
          }

          existing.Image = "/About/" + uniqueFileName;
        }

        _db.Update(existing);
        await _db.SaveChangesAsync();

        return Json(new { success = true, message = "About updated successfully" });
      }
      catch (Exception ex)
      {
        return Json(new { success = false, message = "Error: " + ex.Message });
      }
    }


    // ✅ Delete blog (non-AJAX)
    [HttpPost("Delete/{id}")]
    [Authorize]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
      var about = _db.About.Find(id);
      if (about == null)
        return NotFound();

      _db.About.Remove(about);
      _db.SaveChanges();

      return RedirectToAction(nameof(Index));
    }
  }
}
