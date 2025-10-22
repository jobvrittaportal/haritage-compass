using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourTravel.Models;

namespace TourTravel.Controllers
{
    public class CommonController : Controller
    {
        private readonly MyDbContext _db;
        private readonly IWebHostEnvironment _env;

        public CommonController(MyDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Subscribe([FromForm] NewsletterSubscribers model)
        {
            if (string.IsNullOrEmpty(model.Email))
            {
                return Json(new { success = false, message = "Email is required." });
            }

            var exists = _db.NewsletterSubscribers.FirstOrDefault(x => x.Email == model.Email);
            if (exists != null)
            {
                return Json(new { success = false, message = "You are already subscribed." });
            }

            _db.NewsletterSubscribers.Add(new NewsletterSubscribers
            {
                Email = model.Email
            });
            _db.SaveChanges();

            return Json(new { success = true, message = "Thank you for subscribing!" });
        }

    }
}
