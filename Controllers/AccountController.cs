using System.Security.Claims;
using System.Threading.Tasks;
using dotnet_db.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_db.Controllers;

[Route("account")]
public class AccountController : Controller
{


    private UserManager<AppUser> _userManager;
    private SignInManager<AppUser> _signInManager;
    private IEmailService _emailService;


    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailService = emailService;
    }


    [HttpGet("login")]
    public ActionResult Login()
    {
        return View();
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(AccountLoginModel model, string? ReturnUrl)
    {

        if (ModelState.IsValid)
        {

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null)
            {

                await _signInManager.SignOutAsync();
                var results = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, true);

                if (results.Succeeded)
                {

                    await _userManager.ResetAccessFailedCountAsync(user);
                    await _userManager.SetLockoutEndDateAsync(user, null);

                    if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else if (results.IsLockedOut)
                {
                    var lockoutDate = await _userManager.GetLockoutEndDateAsync(user);
                    var timeLeft = lockoutDate.Value - DateTime.UtcNow;
                    ModelState.AddModelError("", $"Hesabınız kitlendi. Lütfen {timeLeft.Minutes + 1} dakika sonra tekrar deneyin");
                }
                else
                {
                    ModelState.AddModelError("", "Şifre hatalı");
                }
            }
            else
            {
                ModelState.AddModelError("", "Email Bulunamadı!");
            }
        }
        return View(model);
    }


    [HttpGet("create")]
    public ActionResult Create()
    {
        return View();
    }

    [HttpPost("create")]
    public async Task<ActionResult> Create(AccountCreateModel model)
    {
        if (ModelState.IsValid)
        {

            var user = new AppUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            foreach (var err in result.Errors)
            {
                ModelState.AddModelError("", err.Description);
            }
        }
        return View(model);
    }


    [Authorize]
    [HttpGet("logout")]
    public async Task<ActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    [HttpGet("access-denied")]
    public ActionResult AccessDenied()
    {
        return View();
    }

    [Authorize]
    [HttpGet("settings/user-edit")]
    public async Task<ActionResult> UserEdit()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(userId!);

        return View(
            new AccountEditUserModel
            {
                FullName = user.FullName,
                Email = user.Email!
            }
        );
    }


    [Authorize]
    [HttpPost("settings/user-edit")]
    public async Task<ActionResult> UserEdit(AccountEditUserModel model)
    {

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); //Giriş yapan kull. id'si
        var user = await _userManager.FindByIdAsync(userId!);

        if (ModelState.IsValid)
        {
            if (user != null)
            {
                user.FullName = model.FullName;
                user.Email = model.Email;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    TempData["message"] = "Bilgileriniz güncellendi!";
                }

                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
            }

        }
        return View(model);
    }


    [Authorize]
    [HttpGet("settings/change-password")]
    public ActionResult ChangePassword()
    {
        return View();
    }


    [Authorize]
    [HttpPost("settings/change-password")]
    public async Task<ActionResult> ChangePassword(AccountChangePasswordModel model)
    {

        if (ModelState.IsValid)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId!);

            if (user != null)
            {
                var res = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.Password);

                if (res.Succeeded)
                {
                    TempData["message"] = "Şifreniz başarıyla değiştirildi!";

                }

                foreach (var err in res.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }

            }
        }
        return View(model);
    }

    [HttpGet("forgot-password")]
    public ActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost("forgot-password")]
    public async Task<ActionResult> ForgotPassword(AccountForgotPasswordModel model)
    {

        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var url = Url.Action("ResetPassword", "Account", new { userId = user.Id, token });

                var link = $"<a href='http://localhost:5103{url}'>Şifreni yenilemek için bu linke tıklayınız</a>";
                await _emailService.SendEmailAsync(model.Email, "Şifre sıfırlama", link);

                TempData["message"] = "Mail gönderilen link ile şifreni sıfırlayabilirsin";

                return RedirectToAction("Login");
            }
            else
            {
                TempData["message"] = "Email bulunamadı!";
            }
        }
        return View();
    }


    [HttpGet("reset-password")]
    public async Task<ActionResult> ResetPassword(string userId, string token)
    {

        if (userId == null || token == null)
        {
            return RedirectToAction("Login");
        }

        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return RedirectToAction("Login");
        }

        return View(
            new AccountResetPasswordModel
            {
                Token = token,
                Email = user.Email!
            }
        );
    }

    [HttpPost("reset-password")]
    public async Task<ActionResult> ResetPassword(AccountResetPasswordModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return RedirectToAction("Login");
            }

            var res = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

            if (res.Succeeded)
            {
                TempData["message"] = "Şifreniz başarıyla değiştirildi!";
                return RedirectToAction("Login");
            }
            foreach (var err in res.Errors)
            {
                TempData["message"] = err.Description;
            }
        }
        return View(model);
    }

}