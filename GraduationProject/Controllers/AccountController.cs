using GraduationProject.Models;
using GraduationProject.DAL;
using GraduationProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;

namespace GraduationProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration config;
        private readonly AppDbContext db;

        public AccountController(UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager, RoleManager<IdentityRole> _roleManager, IConfiguration _config, AppDbContext _db)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            roleManager = _roleManager;
            config = _config;
            db = _db;
        }


        public async Task<IActionResult> Index(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Register");
                }
                AppUser loggedUser = await db.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);
                ViewBag.loggerUser = loggedUser;
                AccountViewModels avm = new AccountViewModels()
                {
                    concertHisttory = await db.soldTickets.Include(x=>x.Concert).Where(x => x.Concert.Time < DateTime.Now && x.AppUserId==loggedUser.Id).ToListAsync(),
                    futureConcert = await db.soldTickets.Include(x=>x.Concert).Where(x => x.Concert.Time > DateTime.Now && x.AppUserId == loggedUser.Id).ToListAsync(),
                };
                return View(avm);
            }

            AppUser userToView = await db.Users.FirstOrDefaultAsync(x => x.Email == username);
            return View(userToView);
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel rvm)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser user = new AppUser
            {
                FullName = rvm.FullName,
                Email = rvm.Email,
                UserName = rvm.Email.Split('@')[0],
                IsActive=true
            };
            IdentityResult identityResult = await userManager.CreateAsync(user, rvm.Password);
            if (!identityResult.Succeeded)
            {
                foreach (IdentityError error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(rvm);
            }
            await userManager.AddToRoleAsync(user, "Member");
            return RedirectToAction("index", "Home");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel lvm)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser loggingUser = await userManager.FindByEmailAsync(lvm.Email);
            if (loggingUser == null)
            {
                ModelState.AddModelError("", "Email or password is wrong.");
                return View(lvm);
            };

            Microsoft.AspNetCore.Identity.SignInResult signInResult = await signInManager.PasswordSignInAsync(loggingUser, lvm.Password, lvm.KeepMeLoggedIn, true);
            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelError("", "You are locked out!");
                return View(lvm);

            }
            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("", "Email or password is wrong.");
                return View(lvm);
            }
            return RedirectToAction("index", "Home");
        }
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult ForgetPassword()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SendForgetPaswordMail(ForgetPassword forgetPassword)
        {
            AppUser user = await userManager.FindByEmailAsync(forgetPassword.User.Email);
            if (user == null) return NotFound();
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var link= Url.Action(nameof(ResetPassword), "Account", new { email = user.Email, token }, Request.Scheme);
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.Credentials = new NetworkCredential("javidkhi@code.edu.az", "Cavid1995");
            client.EnableSsl = true;

            MailAddress from = new MailAddress("javidkhi@code.edu.az");
            MailAddress to = new MailAddress(forgetPassword.User.Email);
            MailMessage message = new MailMessage(from, to);
            message.Subject = "Reset password";
            message.Body = $"<a href={link}>Go to reset password</a>";
            message.IsBodyHtml = true;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.SubjectEncoding = System.Text.Encoding.UTF8;

            client.Send(message);
            message.Dispose();
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> ResetPassword(string email, string token)
        {
            AppUser user = await userManager.FindByEmailAsync(email);

            if (user == null) return NotFound();

            ForgetPassword forgetPassword = new ForgetPassword
            {
                Token = token,
                User = user
            };
            return View(forgetPassword);
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ForgetPassword model)
        {
            AppUser user = await userManager.FindByEmailAsync(model.User.Email);
            if (user == null) return NotFound();


            IdentityResult result = await userManager.ResetPasswordAsync(user, model.Token, model.Password);

            foreach (var item in result.Errors)
            {
                ModelState.AddModelError("", item.Description);
            }
            await signInManager.PasswordSignInAsync(user, model.Password, true, true);

            TempData["ResponsResetPassword"] = "Your password has been successfully changed";
            return RedirectToAction("Index", "Home");
        }
        //public async Task<IActionResult> InitRoles()
        //{
        //    await roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
        //    await roleManager.CreateAsync(new IdentityRole("Admin"));
        //    await roleManager.CreateAsync(new IdentityRole("Member"));
        //    return Content("Ok");
        //}
    }
}
