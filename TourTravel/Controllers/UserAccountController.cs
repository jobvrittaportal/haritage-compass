using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TourTravel.Models;
using TourTravel.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace TourTravel.Controllers
{
    public class UserAccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly MyDbContext _db;
        private readonly IMailService _mailService;
        public UserAccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager, MyDbContext db,IMailService mailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = db;
            _mailService = mailService;
        }
        public IActionResult Login()
        {
           
            var page = _db.SitePages.FirstOrDefault(f => f.Page == "Login");
            if (page != null)
            {
                ViewBag.Title = page.Title;
                ViewBag.Page = page.Page;
                ViewBag.Description = page.Description;
                ViewBag.Keywords = page.KeyWords;
                ViewBag.Image = page.Image;
                ViewBag.ImageHeight = page.ImgHeight;
                ViewBag.ImageWidth = page.ImgWidth;
            }
            return View();
        }
        public IActionResult Register()
        {
            var page = _db.SitePages.FirstOrDefault(f => f.Page == "Register");
            if (page != null)
            {
                ViewBag.Title = page.Title;
                ViewBag.Page = page.Page;
                ViewBag.Description = page.Description;
                ViewBag.Keywords = page.KeyWords;
                ViewBag.Image = page.Image;
                ViewBag.ImageHeight = page.ImgHeight;
                ViewBag.ImageWidth = page.ImgWidth;
            }
            return View();
        }
        public IActionResult ForgotPassword()
        {
            var page = _db.SitePages.FirstOrDefault(f => f.Page == "Forgot Password");
            if (page != null)
            {
                ViewBag.Title = page.Title;
                ViewBag.Page = page.Page;
                ViewBag.Description = page.Description;
                ViewBag.Keywords = page.KeyWords;
                ViewBag.Image = page.Image;
                ViewBag.ImageHeight = page.ImgHeight;
                ViewBag.ImageWidth = page.ImgWidth;
            }
            return View();
        }
       
        public IActionResult ResetPassword(string email, string token)
        {
            if (email == null || token == null)
            {
                return RedirectToAction("ForgotPassword");
            }
            var model = new ResetPasswordModel { Email = email, Token = token };
            var page = _db.SitePages.FirstOrDefault(f => f.Page == "ResetPassword");
            if (page != null)
            {
                ViewBag.Title = page.Title;
                ViewBag.Page = page.Page;
                ViewBag.Description = page.Description;
                ViewBag.Keywords = page.KeyWords;
                ViewBag.Image = page.Image;
                ViewBag.ImageHeight = page.ImgHeight;
                ViewBag.ImageWidth = page.ImgWidth;
            }
            return View(model);
        }

        public IActionResult ForgotPasswordConfirmation()
        {
            var page = _db.SitePages.FirstOrDefault(f => f.Page == "ForgotPasswordConfirmation");
            if (page != null)
            {
                ViewBag.Title = page.Title;
                ViewBag.Page = page.Page;
                ViewBag.Description = page.Description;
                ViewBag.Keywords = page.KeyWords;
                ViewBag.Image = page.Image;
                ViewBag.ImageHeight = page.ImgHeight;
                ViewBag.ImageWidth = page.ImgWidth;
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            email = email.Trim();
            password = password.Trim();

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Both Email and Password are required.";
                return View();
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                ViewBag.Error = "Invalid email or password.";
                return View();
            }

            if (!user.IsActive)
            {
                ViewBag.Error = "Your account is inactive.";
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(user, password, false, false);

            if (!result.Succeeded)
            {
                ViewBag.Error = "Incorrect email or password.";
                return View();
            }

            return RedirectToAction("Index", "Home");
        }



        //Regsiter New User
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(ApplicationUser model, string password)
        {
            if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Email and password are required.";
                return View(model);
            }

            // Check if email already exists
            var existingUser = await _userManager.FindByEmailAsync(model.Email);   
            if (existingUser != null)
            {
                ViewBag.Error = "A user with this email already exists.";
                return View(model);
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                Name = model.Name,
                IsActive = true // Default new accounts to active, adjust if needed
            };

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                
                TempData["SuccessMessage"] = "User created successfully. Please login.";
                return RedirectToAction("Register", "UserAccount");
            }

            // Show errors if creation failed
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }


    

        //Forget Password to send email
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(string email)
        {

            if (string.IsNullOrWhiteSpace(email))
            {
                ViewBag.Error = "Please enter your email.";
                return View();
            }

            var user = await _userManager.FindByEmailAsync(email.Trim());
            if (user == null)
            {
                // Do not reveal whether the email exists in the system
                TempData["Message"] = "If an account with this email exists, a reset link has been sent.";
                return RedirectToAction("ForgotPasswordConfirmation");
            }

            try
            {
                // Generate password reset token
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                // Construct password reset link
                var resetLink = Url.Action("ResetPassword", "UserAccount",
                    new { email = user.Email, token = token }, Request.Scheme);

                // Build HTML email body
                string emailBody = $@"
            <p>Dear {user.Name ?? "User"},</p>
            <p>We received a request to reset your password for your Travelox account.</p>
            <p>Click the button below to reset your password:</p>
            <p><a href='{resetLink}' 
                  style='display:inline-block;
                         background-color:#1a73e8;
                         color:white;
                         padding:10px 20px;
                         text-decoration:none;
                         border-radius:4px;'>Reset Password</a></p>
            <p>If you did not request this, please ignore this email.</p>
            <p>Best regards,<br/>Tour Travel Team</p>";

                // Send email using your MailService
                bool mailSent = _mailService.SendMail(new MailData
                {
                    EmailToId = new string[] { user.Email },
                    SenderName = "Travelox",
                    SenderEmail = "estaffing@tekinspirations.com",
                    EmailSubject = "Reset Your Password",
                    EmailBody = emailBody
                });

                if (!mailSent)
                {
                    ViewBag.Error = "Failed to send password reset email. Please try again later.";
                    return View();
                }

                //TempData["Message"] = "A password reset link has been sent to your email address.";
                return RedirectToAction("ForgotPasswordConfirmation");
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"An error occurred while processing your request: {ex.Message}";
                return View();

            }
        }

        // POST: ResetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.NewPassword != model.ConfirmPassword)
            {
                ViewBag.Error = "Passwords do not match.";
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ViewBag.Error = "Invalid email address.";
                return View(model);
            }

            // This updates the actual password in the database

            //var isValid = await _userManager.VerifyUserTokenAsync(user, "PasswordReset", "ResetPassword", model.Token);

            //if (!isValid)
            //{
            //    ViewBag.Error = "The password reset link has expired or is invalid. Please request a new one.";
            //    return View(model);
            //}
            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            


            if (result.Succeeded)
            {
                TempData["Message"] = "Your password has been reset successfully.";
                return RedirectToAction("ResetPassword", "UserAccount");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
            if (!result.Succeeded)
            {
                ViewBag.Error = "The password reset link has expired or is invalid. Please request a new one.";
                return View(model);
            }

            return View(model);

           
        }



    }
}
