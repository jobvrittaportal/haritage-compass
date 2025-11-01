//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using TourTravel.Models;

//namespace TourTravel.Controllers
//{
//    public class CommonController : Controller
//    {
//        private readonly MyDbContext _db;
//        private readonly IWebHostEnvironment _env;

//        public CommonController(MyDbContext db, IWebHostEnvironment env)
//        {
//            _db = db;
//            _env = env;
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public IActionResult Subscribe([FromForm] NewsletterSubscribers model)
//        {
//            if (string.IsNullOrWhiteSpace(model.Email))
//            {
//                return Json(new { success = false, message = "Please enter a valid email address." });
//            }

//            var email = model.Email.Trim().ToLower();

//            var subscriber = _db.NewsletterSubscribers.FirstOrDefault(x => x.Email.ToLower() == email);

//            if (subscriber != null)
//            {
//                if (subscriber.IsSubscribed)
//                {
//                    return Json(new
//                    {
//                        success = true,
//                        message = $"The email '{model.Email}' is  subscribed to our newsletter."
//                    });
//                }

//                // Re-subscribe existing user
//                subscriber.IsSubscribed = true;
//                subscriber.UpdatedAt = DateTime.UtcNow;
//                _db.SaveChanges();

//                return Json(new
//                {
//                    success = true,
//                    message = $"The email '{model.Email}' has been re-subscribed successfully."
//                });
//            }

//            // New subscriber
//            var newSubscriber = new NewsletterSubscribers
//            {
//                Email = email,
//                IsSubscribed = true,
//                CreatedAt = DateTime.UtcNow,
//                UpdatedAt = DateTime.UtcNow
//            };

//            _db.NewsletterSubscribers.Add(newSubscriber);
//            _db.SaveChanges();

//            return Json(new
//            {
//                success = true,
//                message = $"Thank you for subscribing, {model.Email}!"
//            });
//        }
//    }
//}
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourTravel.Models;
using TourTravel.Services;


namespace TourTravel.Controllers
{

    public class CommonController(MyDbContext db, IWebHostEnvironment env, IMailService mailService) : Controller
    {
        private readonly MyDbContext _db = db;
        private readonly IWebHostEnvironment _env = env;
        private readonly IMailService _mailService = mailService;

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Subscribe([FromForm] NewsletterSubscribers model)
        {
            // ✅ 1. Basic validation
            var Email = model.Email.Trim().ToLower();
            if (string.IsNullOrWhiteSpace(Email))
            {
                return Json(new
                {
                    success = false,
                    message = "Please enter a valid email address."
                });
            }

            // ✅ 2. Case-insensitive lookup for existing subscription
            var existingSubscriber = _db.NewsletterSubscribers.FirstOrDefault(x => x.Email.Trim().ToLower() == Email);

            if (existingSubscriber is not null)
            {
                if (existingSubscriber.IsSubscribed)
                {
                    _ = SendSubscriptionEmailAsync(Email);
                    return Json(new
                    {
                        success = true,
                        message = $"The email '{model.Email}' is  subscribed to our newsletter."
                    });
                }

                // ✅ Re-subscribe previously unsubscribed user
                existingSubscriber.IsSubscribed = true;
                existingSubscriber.UpdatedAt = DateTime.Now;
                _db.SaveChanges();

                // Send confirmation email
                _ = SendSubscriptionEmailAsync(Email);

                return Json(new
                {
                    success = true,
                    message = $"Welcome back! '{model.Email}' has been re-subscribed."
                });
            }

            // ✅ 3. Create a new subscription
            var newSubscriber = new NewsletterSubscribers
            {
                Email = Email,
                IsSubscribed = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _db.NewsletterSubscribers.Add(newSubscriber);
            _db.SaveChanges();

            // Send confirmation email
            _ = SendSubscriptionEmailAsync(Email);

            return Json(new
            {
                success = true,
                message = $"Thank you for subscribing, {model.Email}!"
            });
        }

        // ✅ Asynchronous method to send subscription email
        private async Task SendSubscriptionEmailAsync(string email)
        {
            try
            {
                string unsubscribeUrl = Url.Action("Unsubscribe", "Common", new { email }, Request.Scheme);

                string emailBody = $@"
               <div style='font-family:Arial,Helvetica,sans-serif; max-width:600px; margin:0 auto; border:1px solid #ddd; border-radius:8px; overflow:hidden;'>

                 <!-- Header -->
                    <table role='presentation' width='100%' cellpadding='0' cellspacing='0' style='background-color:#1a73e8;'>
                       <tr>
                           <td style='padding:8px 8px; font-size:20px; font-weight:bold; color:white;'>
                               Tour Travel
                           </td>
                           <td style='padding:8px 8px; text-align:right;'>
                               <a href='{unsubscribeUrl}'
                                  style='background-color:#e53935; color:white; padding:8px 16px; border-radius:4px; text-decoration:none; font-size:14px; display:inline-block;'>
                                  Unsubscribe
                               </a>
                           </td>
                       </tr>
                   </table>


                    <!-- Email Body -->
                    <div style='padding:25px; background-color:#ffffff;'>
                        <p style='font-size:16px;'>Dear Subscriber,</p>
                        <p style='font-size:15px; line-height:1.6;'>
                            Thank you for subscribing to the <strong>Tour Travel Newsletter</strong>!
                            You’ll now receive the latest travel updates, exclusive offers, and destination inspiration.
                        </p>

                        <p style='font-size:15px; line-height:1.6;'>
                            Stay tuned for exciting travel content and deals tailored just for you.
                        </p>

                        <p style='text-align:center; margin-top:30px;'>
                            <a href='https://heritage-compass.jobvritta.com' 
                               style='background-color:#1a73e8; color:white; padding:12px 25px; border-radius:5px; text-decoration:none; font-size:16px;'>
                               Visit Our Website
                            </a>
                        </p>

                        <p style='font-size:13px; color:#777; margin-top:30px;'>
                            You are receiving this email because you subscribed to our newsletter.<br/>
                            If you no longer wish to receive these emails, click 
                            <a href='{unsubscribeUrl}' style='color:#e53935;'>here to unsubscribe</a>.
                        </p>

                        <p style='font-size:14px; color:#333; margin-top:25px;'>Best Regards,<br/>The Tour Travel Team</p>
                    </div>

                    <div style='background-color:#f5f5f5; text-align:center; padding:10px 0; font-size:12px; color:#888;'>
                        © {DateTime.UtcNow.Year} Tour Travel. All rights reserved.
                    </div>
                </div>";

                var mailData = new MailData
                {
                    EmailToId = new[] { email },
                    SenderName = "Tour Travel",
                    SenderEmail = "estaffing@tekinspirations.com",
                    EmailSubject = "Welcome to Tour Travel Newsletter!",
                    EmailBody = emailBody
                };

                await Task.Run(() => _mailService.SendMail(mailData));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Email Error] {ex.Message}");
            }
        }

        // ✅ Simple unsubscribe action (optional)
        [HttpGet]
        public IActionResult Unsubscribe(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return BadRequest("Invalid email address.");

            var subscriber = _db.NewsletterSubscribers.FirstOrDefault(x => x.Email.Trim().ToLower() == email.Trim().ToLower());

            if (subscriber is null)
                return NotFound("Subscriber not found.");

            subscriber.IsSubscribed = false;
            subscriber.UpdatedAt = DateTime.Now;
            _db.SaveChanges();

            return Content($"The email {email} has been unsubscribed successfully.");
        }
    }
}


