using dotnet_db.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace dotnet_db.Controllers;

[Area("Admin")]
[Route("admin/roles")]
public class RoleController : Controller
{

    private RoleManager<AppRole> _roleManager;

    public RoleController(RoleManager<AppRole> roleManager)
    {
        _roleManager = roleManager;
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
}