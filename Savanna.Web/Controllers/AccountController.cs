using Common.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Savanna.Web.Models;

namespace Savanna.Web.Controllers;

[Route("[controller]/[action]")]
[AllowAnonymous]
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
    [ValidateAntiForgeryToken]
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
            return RedirectToAction(nameof(HomeController.Index), "Home");
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
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel loginViewModel, string? ReturnUrl)
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
            if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
            {
                return LocalRedirect(ReturnUrl);
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
        ModelState.AddModelError("Login", "Invalid email or password");
        return View();
    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction(nameof(AccountController.Login), "Login");
    }


    public async Task<IActionResult> IsUserAlreadyRegistered(string email)
    {
        ApplicationUser user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return Json(true);
        }
        else
        {
            return Json(false);
        }
    }
}
