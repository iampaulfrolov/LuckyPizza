using System.Security.Claims;
using System.Threading.Tasks;
using CourseProject.Data;
using CourseProject.Models.DataModels;
using CourseProject.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CourseProject.Controllers;

[AllowAnonymous]
public class UserController : Controller
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public UserController(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(UserRegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new User
                { UserName = model.UserName, Name = model.Name, Surname = model.SurName, PhoneNumber = "new" };
            await _userManager.AddToRoleAsync(user, RoleConst.User);
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return RedirectToAction("Index", "Product");
            }

            foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(UserLoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, true, false);
            if (result.Succeeded) return RedirectToAction("Index", "Product");

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View();
        }

        return View();
    }

    [AllowAnonymous]
    public IActionResult GoogleLogin()
    {
        var redirectUrl = Url.Action("GoogleResponse", "User");
        var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
        return new ChallengeResult("Google", properties);
    }

    [AllowAnonymous]
    public async Task<IActionResult> GoogleResponse()
    {
        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
            return RedirectToAction(nameof(Login));

        var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
        string[] userInfo =
            { info.Principal.FindFirst(ClaimTypes.Name)?.Value, info.Principal.FindFirst(ClaimTypes.Email)?.Value };
        if (result.Succeeded) return RedirectToAction("Index", "Product");

        var user = new User
        {
            Email = info.Principal.FindFirst(ClaimTypes.Email).Value,
            UserName = info.Principal.FindFirst(ClaimTypes.Email).Value
        };

        var identResult = await _userManager.CreateAsync(user);
        if (identResult.Succeeded)
        {
            identResult = await _userManager.AddLoginAsync(user, info);
            if (identResult.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return RedirectToAction("Index", "Product");
            }
        }

        return RedirectToAction("Index", "Product");
    }
}