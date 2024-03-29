using Common.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Savanna.Web.Models;

namespace Savanna.Web.Controllers
{
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            //Check for validation errors
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values
                    .SelectMany(temp => temp.Errors)
                    .Select(temp => temp.ErrorMessage);
                return View(registerViewModel);
            }

            ApplicationUser user = new ApplicationUser()
            {
                Email = registerViewModel.Email,
                UserName = registerViewModel.Email,
                NickName = registerViewModel.NickName
            };

            IdentityResult result = await _userManager.CreateAsync(user, registerViewModel.Password);
            if (result.Succeeded)
            {
                //Sign In
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction(nameof(GameController.Index), "Game");
            }
            else
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("Register", error.Description);
                }   
                return View(registerViewModel);
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values
                    .SelectMany(temp => temp.Errors)
                    .Select(temp => temp.ErrorMessage);
                return View(loginViewModel);
            }
            var result = await _signInManager.PasswordSignInAsync
                (loginViewModel.Email, loginViewModel.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return RedirectToAction(nameof(GameController.Index), "Game");
            }
            ModelState.AddModelError("Login", "Invalid email or password");
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(AccountController.Login), "Login");
        }
    }
}
