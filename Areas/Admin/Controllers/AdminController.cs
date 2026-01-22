using Microsoft.AspNetCore.Mvc;

namespace dotnet_db.Areas.Admin.Controllers;

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