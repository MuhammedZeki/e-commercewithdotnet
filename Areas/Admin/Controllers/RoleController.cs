using dotnet_db.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_db.Controllers;


[Authorize(Roles = "Admin")]
[Area("Admin")]
[Route("admin/roles")]
public class RoleController : Controller
{

    private RoleManager<AppRole> _roleManager;
    private UserManager<AppUser> _userManager;


    public RoleController(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    [HttpGet("")]
    public ActionResult Index()
    {
        return View(_roleManager.Roles);
    }


    [HttpGet("create")]
    public ActionResult Create()
    {
        return View();
    }

    [HttpPost("create")]
    public async Task<ActionResult> Create(RoleCreateModel model)
    {
        if (ModelState.IsValid)
        {
            var role = new AppRole
            {
                Name = model.RoleName
            };

            var result = await _roleManager.CreateAsync(role);

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

        var role = await _roleManager.FindByIdAsync(id.ToString());
        if (role != null)
        {
            return View(new RoleEditModel { Id = role.Id, RoleName = role.Name! });
        }
        return RedirectToAction("Index");
    }

    [HttpPost("edit/{id}")]
    public async Task<ActionResult> Edit(int id, RoleEditModel model)
    {

        if (id != model.Id)
        {
            return RedirectToAction("Index");
        }

        if (ModelState.IsValid)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString()); //string ister
            if (role != null)
            {
                role.Name = model.RoleName;

                var result = await _roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    TempData["message"] = $"{role.Name} olarak başarıyla Güncellendi.";
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



    [HttpGet("delete/{id}")]
    public async Task<ActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var role = await _roleManager.FindByIdAsync(id.ToString()!);

        if (role != null)
        {
            ViewBag.Users = await _userManager.GetUsersInRoleAsync(role.Name!);
            return View(role);
        }
        return RedirectToAction("Index");
    }


    [HttpPost("delete/{id}")]
    public async Task<ActionResult> DeleteConfirmed(int? id)
    {

        if (id == null)
        {
            return RedirectToAction("Index");
        }

        var role = await _roleManager.FindByIdAsync(id.ToString()!);

        if (role != null)
        {
            var result = await _roleManager.DeleteAsync(role);

            if (result.Succeeded)
            {
                TempData["message"] = $"{role.Name} rolü silindi.";
            }
        }
        return RedirectToAction("Index");
    }
}

