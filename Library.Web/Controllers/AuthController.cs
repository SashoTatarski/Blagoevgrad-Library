using Library.Models.Models;
using Library.Models.Utils;
using Library.Services.Contracts;
using Library.Web.Models.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Library.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAccountManager _accountManager;

        public AuthController(IAccountManager accountManager)
        {
            _accountManager = accountManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return View(viewModel);
            }

            try
            {
                var user = _accountManager.Find(viewModel.Username, viewModel.Password);                
                
                _accountManager.CheckStatus(user);

                await SignInUserAsync(user);

                return BackToHome();
            }
            catch (Exception ex)
            {
                TempData["Status"] = ex.Message;                

                return View(viewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await this.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).ConfigureAwait(false);

            return BackToHome();
        }

        [HttpGet]
        public IActionResult Register()
        {
            var viewModel = new RegisterViewModel() { MembershipOption = new List<SelectListItem>() };
            viewModel.MembershipOption.Add(new SelectListItem { Text = Constants.SubscrOneMonth, Value = "1" });
            viewModel.MembershipOption.Add(new SelectListItem { Text = Constants.SubscrOneYear, Value = "12" });

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return View(viewModel);
            }                      

            try
            {
                viewModel.MembershipOption = new List<SelectListItem>();
                viewModel.MembershipOption.Add(new SelectListItem { Text = Constants.SubscrOneMonth, Value = "1" });
                viewModel.MembershipOption.Add(new SelectListItem { Text = Constants.SubscrOneYear, Value = "12" });

                User user = await _accountManager.CreateNewUserAsync(viewModel.Username, viewModel.Password, viewModel.MembershipType);

                await SignInUserAsync(user);

                return BackToHome();
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;

                return View(viewModel);
            }
        }


        private async Task SignInUserAsync(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.RoleName),
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(24),
                IssuedUtc = DateTimeOffset.UtcNow,
                IsPersistent = true
            };

            await this.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties).ConfigureAwait(false);
        }

        private RedirectToActionResult BackToHome()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}