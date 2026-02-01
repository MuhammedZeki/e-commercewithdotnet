using dotnet_db.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dotnet_db.Controllers;


[Authorize(Roles = "Admin")]
[Area("Admin")]
[Route("admin/users")]
public class UserController : Controller
{
    private UserManager<AppUser> _userManager;
    private RoleManager<AppRole> _roleManager;



    public UserController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [HttpGet("")]
    public ActionResult Index()
    {
        return View(_userManager.Users);
    }

    [HttpGet("create")]
    public ActionResult Create()
    {
        return View();
    }

    [HttpPost("create")]
    public async Task<ActionResult> Create(UserCreateModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new AppUser
            {
                FullName = model.FullName,
                Email = model.Email,
                UserName = model.Email
            };

            var result = await _userManager.CreateAsync(user);

            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            foreach (var err in result.Errors)
            {
                ModelState.AddModelError("", err.Description);
            }
        }
        return View(model);
    }

    [HttpGet("edit/{id}")]
    public async Task<ActionResult> Edit(int id)
    {
        if (id == 0)
        {
            return RedirectToAction("Index");
        }

        var user = await _userManager.FindByIdAsync(id.ToString());

        ViewBag.Roles = await _roleManager.Roles.Select(i => i.Name).ToListAsync();

        return View(
            new UserEditModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email!,
                SelectedRoles = await _userManager.GetRolesAsync(user)
            }
        );
    }

    [HttpPost("edit/{id}")]
    public async Task<ActionResult> Edit(int id, UserEditModel model)
    {

        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user != null)
            {
                user.FullName = model.FullName;
                user.Email = model.Email;


                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded && !string.IsNullOrEmpty(model.Password))
                {
                    await _userManager.RemovePasswordAsync(user);
                    await _userManager.AddPasswordAsync(user, model.Password);

                    TempData["message"] = $"{user.FullName} olarak başarıyla Güncellendi.";
                    return RedirectToAction("Index");
                }
                if (result.Succeeded)
                {
                    await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));

                    if (model.SelectedRoles != null)
                    {
                        await _userManager.AddToRolesAsync(user, model.SelectedRoles);
                    }
                    TempData["message"] = $"{user.FullName} olarak başarıyla Güncellendi.";
                    return RedirectToAction("Index");
                }

                foreach (var i in result.Errors)
                {
                    ModelState.AddModelError("", i.Description);
                }
            }
        }
        return View(model);
    }
}