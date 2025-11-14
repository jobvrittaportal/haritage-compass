using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TourTravel.Models;

namespace TourTravel.Admin.Controllers
{
    [Route("admin/[controller]")]
    public class WhyChooseUsController : Controller
    {
        private readonly MyDbContext _db;
        private readonly IWebHostEnvironment _env;

        public WhyChooseUsController(MyDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
            
        }
        [HttpGet]
        public async Task<IActionResult> Index(string? search, int page = 1, int pageSize = 5)
        {
            var query = _db.WhyChooseUs.AsQueryable();

           
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

            return View("~/Views/Admin/WhyChooseUs/Index.cshtml", list);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View("~/Views/Admin/WhyChooseUs/Create.cshtml");
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WhyChooseUs model, IFormFile? ImageFile)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Invalid form data." });

            if (ImageFile == null || ImageFile.Length == 0)
                return Json(new { success = false, message = "Please upload an image." });

            try
            {
              
                string folderPath = Path.Combine(_env.WebRootPath, "uploads", "WhyChooseUs");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                
                string fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
                string filePath = Path.Combine(folderPath, fileName);

               
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                
                model.ImageUrl = "/uploads/WhyChooseUs/" + fileName;

                _db.WhyChooseUs.Add(model);
                await _db.SaveChangesAsync();

                return Json(new { success = true, message = "Create successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }


        [HttpGet("edit/{id}")]
        public IActionResult Edit(int id)
        {
            var item = _db.WhyChooseUs.Find(id);
            if (item == null)
                return NotFound();

            return View("~/Views/Admin/WhyChooseUs/Edit.cshtml", item);
        }

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, WhyChooseUs model, IFormFile? newImage)
        {
            try
            {
                var item = await _db.WhyChooseUs.FindAsync(id);
                if (item == null)
                    return Json(new { success = false, message = "Record not found." });

                
                item.Title = model.Title;
                item.Description = model.Description;


                // ✅ Handle new image upload (optional)
                if (newImage != null && newImage.Length > 0)
                {
                    string folderPath = Path.Combine(_env.WebRootPath, "uploads", "WhyChooseUs");
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

                    item.ImageUrl = "/uploads/WhyChooseUs/" + fileName;
                }

                _db.WhyChooseUs.Update(item);
                await _db.SaveChangesAsync();

                return Json(new { success = true, message = "Updated successfully!" });
            }
            catch (Exception ex)
            {
               
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }

        // ✅ Delete
        [HttpPost("delete/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var image = _db.WhyChooseUs.Find(id);
            if (image == null)
                return Json(new { success = false, message = "not found!" });

            if (!string.IsNullOrEmpty(image.ImageUrl))
            {

                var filePath = Path.Combine(_env.WebRootPath, image.ImageUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            _db.WhyChooseUs.Remove(image);
            _db.SaveChanges();

            return Json(new { success = true, message = " deleted successfully!" });
        }
    }
}
