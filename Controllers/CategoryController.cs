using dotnet_db.Models;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_db.Controllers;


[Route("category")]
public class CategoryController : Controller
{

    private readonly DataContext _context;

    public CategoryController(DataContext context)
    {
        _context = context;
    }


    [Route("")]
    public ActionResult Index()
    {
        var categories = _context.Categories.ToList();
        return View(categories);
    }
}