using dotnet_db.Models;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_db.Areas.Admin.Controllers;


[Area("Admin")]
[Route("admin/categories")]
public class CategoryController : Controller
{

    private readonly DataContext _context;

    public CategoryController(DataContext context)
    {
        _context = context;
    }


    [HttpGet("")]
    public ActionResult Index()
    {
        var categories = _context.Categories.Select(i => new CategoryGetModel
        {
            Id = i.Id,
            CategoryName = i.CategoryName,
            Url = i.Url,
            ProductCount = i.Products.Count()
        }).ToList();
        return View(categories);
    }

    [HttpGet("create")]
    public ActionResult Create()
    {
        return View();
    }

    [HttpPost("create")]
    public ActionResult Create(CategoryCreateModel model)
    {

        var category = new Category
        {
            CategoryName = model.CategoryName,
            Url = model.CategoryUrl
        };
        _context.Categories.Add(category);
        _context.SaveChanges();

        return RedirectToAction("Index", "Category");
    }
}