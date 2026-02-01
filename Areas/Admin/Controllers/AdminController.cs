using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_db.Areas.Admin.Controllers;

[Authorize(Roles = "Admin")]
[Area("Admin")]
[Route("admin")]
public class AdminController : Controller
{

    [Route("")]
    public ActionResult Index()
    {
        return View();
    }
}