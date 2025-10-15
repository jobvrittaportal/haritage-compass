using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourTravel.Models;
using System.Drawing; // For System.Drawing.Image


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

    // ✅ About List + Search + Pagination
    [HttpGet]
    public async Task<IActionResult> Index(string? search, int page = 1, int pageSize = 5)
    {
      var query = _db.About.AsQueryable();

      // 🔍 Search filter by Title
      if (!string.IsNullOrEmpty(search))
      {
        query = query.Where(b => b.Title.Contains(search));
      }

      int totalItems = await query.CountAsync();
      var aboutList = await query
          .OrderByDescending(b => b.Id)
          .Skip((page - 1) * pageSize)
          .Take(pageSize)
          .ToListAsync();

      ViewBag.Search = search;
      ViewBag.Page = page;
      ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

      return View("~/Views/Admin/About/Index.cshtml", aboutList);
    }

    // ✅ Render Create Form
    [HttpGet("Create")]
    public IActionResult Create()
    {
      return View("~/Views/Admin/About/Create.cshtml");
    }

    // ✅ Create About (AJAX)
    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(About model, IFormFile? image)
    {
      try
      {
        // Validate
        if (string.IsNullOrWhiteSpace(model.Title) || string.IsNullOrWhiteSpace(model.Description))
        {
          return Json(new { success = false, message = "Please fill all required fields." });
        }

        if (image == null || image.Length == 0)
        {
          return Json(new { success = false, message = "Please upload an image for the About section." });
        }

        // ✅ Validate file size (max 5MB)
        //if (image.Length > 5 * 1024 * 1024)
        //  return Json(new { success = false, message = "Image size must be less than 5MB." });

        //// ✅ Validate dimensions
        //using (var stream = image.OpenReadStream())
        //using (var img = Image.FromStream(stream))
        //{
        //  if (img.Width != 800 || img.Height != 600)
        //    return Json(new { success = false, message = "Image dimensions must be 800x600." });
        //}

        // ✅ Check if SlugUrl already exists (case-insensitive)
        bool slugExists = await _db.About
            .AnyAsync(b => b.SlugUrl.ToLower() == model.SlugUrl.ToLower());

        if (slugExists)
        {
          return Json(new { success = false, message = "Slug URL already exists. Please use a different one." });
        }

        // ✅ Save image
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

        return Json(new { success = true, message = "About created successfully." });
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
      var about = await _db.About.FindAsync(id);
      if (about == null)
        return NotFound();

      return View("~/Views/Admin/About/Edit.cshtml", about);
    }

    // ✅ Edit About (AJAX)
    [HttpPost("Edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(About model, IFormFile? image)
    {
      try
      {
        var existing = await _db.About.FindAsync(model.Id);
        if (existing == null)
          return Json(new { success = false, message = "Record not found." });

        // ✅ Check if Slug already exists for another record
        bool slugExists = await _db.About
            .AnyAsync(b => b.Id != model.Id && b.SlugUrl.ToLower() == model.SlugUrl.ToLower());

        if (slugExists)
        {
          return Json(new { success = false, message = "Slug URL already exists. Please use a different one." });
        }

        // ✅ Validate file size (max 5MB)
        //if (image.Length > 5 * 1024 * 1024)
        //  return Json(new { success = false, message = "Image size must be less than 5MB." });

        //// ✅ Validate dimensions
        //using (var stream = image.OpenReadStream())
        //using (var img = Image.FromStream(stream))
        //{
        //  if (img.Width != 800 || img.Height != 600)
        //    return Json(new { success = false, message = "Image dimensions must be 800x600." });
        //}


        existing.Title = model.Title;
        existing.Description = model.Description;
        existing.KeyFeatures = model.KeyFeatures;
        existing.MetaTitle = model.MetaTitle;
        existing.MetaDescription = model.MetaDescription;
        existing.SlugUrl = model.SlugUrl;

        // ✅ Update image if new one uploaded
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

          // Delete old image if exists
          if (!string.IsNullOrEmpty(existing.Image))
          {
            string oldPath = Path.Combine(_env.WebRootPath, existing.Image.TrimStart('/'));
            if (System.IO.File.Exists(oldPath))
              System.IO.File.Delete(oldPath);
          }

          existing.Image = "/About/" + uniqueFileName;
        }

        _db.Update(existing);
        await _db.SaveChangesAsync();

        return Json(new { success = true, message = "About updated successfully." });
      }
      catch (Exception ex)
      {
        return Json(new { success = false, message = "Error: " + ex.Message });
      }
    }

    // ✅ Delete About
    [HttpPost("Delete/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
      var about = await _db.About.FindAsync(id);
      if (about == null)
        return NotFound();

      // Delete image file
      if (!string.IsNullOrEmpty(about.Image))
      {
        string filePath = Path.Combine(_env.WebRootPath, about.Image.TrimStart('/'));
        if (System.IO.File.Exists(filePath))
          System.IO.File.Delete(filePath);
      }

      _db.About.Remove(about);
      await _db.SaveChangesAsync();

      TempData["Success"] = "About deleted successfully.";
      return RedirectToAction(nameof(Index));
    }
  }
}
