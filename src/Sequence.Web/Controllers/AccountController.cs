using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sequence.Web.Domain.Entities;
using Sequence.Web.ViewModels.Account;
using System.Security.Claims;

namespace Sequence.Web.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public IActionResult Login(string? returnUrl = null)
    {
        if (_signInManager.IsSignedIn(User)) return RedirectToAction("Index", "Dashboard");
        return View(new LoginViewModel { ReturnUrl = returnUrl });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var result = await _signInManager.PasswordSignInAsync(
            model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                return Redirect(model.ReturnUrl);
            return RedirectToAction("Index", "Dashboard");
        }

        ModelState.AddModelError("", "ایمیل یا رمز عبور اشتباه است");
        return View(model);
    }

    public IActionResult Register()
    {
        if (_signInManager.IsSignedIn(User)) return RedirectToAction("Index", "Dashboard");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            DisplayName = model.DisplayName,
            JoinedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, isPersistent: false);
            TempData["Success"] = $"خوش آمدید، {user.DisplayName}!";
            return RedirectToAction("Index", "Dashboard");
        }

        foreach (var error in result.Errors)
            ModelState.AddModelError("", TranslateIdentityError(error.Code));

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    [Authorize]
    public async Task<IActionResult> Profile()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null) return NotFound();

        var vm = new ProfileViewModel
        {
            UserId = user.Id,
            DisplayName = user.DisplayName,
            AvatarUrl = user.AvatarUrl,
            Bio = user.Bio,
            JoinedAt = user.JoinedAt
        };

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> Profile(ProfileViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null) return NotFound();

        user.DisplayName = model.DisplayName;
        user.AvatarUrl = model.AvatarUrl;
        user.Bio = model.Bio;

        await _userManager.UpdateAsync(user);
        TempData["Success"] = "پروفایل با موفقیت به‌روزرسانی شد";
        return RedirectToAction(nameof(Profile));
    }

    private static string TranslateIdentityError(string code) => code switch
    {
        "PasswordTooShort" => "رمز عبور خیلی کوتاه است",
        "PasswordRequiresNonAlphanumeric" => "رمز عبور باید شامل کاراکتر خاص باشد",
        "PasswordRequiresDigit" => "رمز عبور باید شامل عدد باشد",
        "PasswordRequiresUpper" => "رمز عبور باید شامل حرف بزرگ باشد",
        "DuplicateEmail" => "این ایمیل قبلاً ثبت شده است",
        "DuplicateUserName" => "این نام کاربری قبلاً استفاده شده است",
        _ => "خطایی رخ داده است"
    };
}