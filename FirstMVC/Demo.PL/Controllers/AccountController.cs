using AutoMapper;
using Demo.DAL.Entities;
using Demo.PL.Models;
using Demo.PL.Utitly;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Demo.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var user = new AppUser
            {
                FName = model.FName,
                LName = model.LName,
                Email = model.Email,
                Agree = model.Agree,
                UserName = model.FName + model.LName
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
                return Redirect(nameof(Login));
            else
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            return View(model);
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                if (await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.KeepMeIn, false);
                    if (result.Succeeded) return RedirectToAction("Index", "Home");
                }
            }
            ModelState.AddModelError("", "Incorrect E-mail Or Password");
            return View(model);
        }

        public new async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return Redirect(nameof(Login));
        }

        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordVM model)
        {
            if (!ModelState.IsValid) return View(model);
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is not null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var url = Url.Action("ResetPassword", "Account", new { email = model.Email, token = token }, Request.Scheme);
                Email email = new Email
                {
                    Subject = "Reset Your Password",
                    Recipient = model.Email,
                    Body = url
                };
                EmailSetting.SendEmail(email);
                return RedirectToAction(nameof(CheckYourInBox));
            }
            ModelState.AddModelError("", "This Email Not Found!");
            return View(model);
        }

        public IActionResult CheckYourInBox()
        {
            return View();
        }

        public IActionResult ResetPassword(string email, string token)
        {
            TempData["email"] = email;
            TempData["token"] = token;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
        {
            if (!ModelState.IsValid) return View();
            var email = TempData["email"] as string;
            var token = TempData["token"] as string;
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var result = await _userManager.ResetPasswordAsync(user, token, model.Password);
                if (result.Succeeded) return RedirectToAction(nameof(Login));
                foreach (var item in result.Errors)
                    ModelState.AddModelError("", item.Description);
            }
            return View();
        }

    }
}
