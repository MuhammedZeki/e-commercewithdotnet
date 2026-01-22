using dotnet_db.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace dotnet_db.Areas.Admin.Controllers;

[Area("Admin")]
[Route("admin/products")]
public class ProductController : Controller
{

    private readonly DataContext _context;

    public ProductController(DataContext context)
    {
        _context = context;
    }

    [HttpGet("")]
    public ActionResult Index()
    {
        var products = _context.Products.Select(i => new ProductGetModel
        {
            Id = i.Id,
            Name = i.Name,
            Price = i.Price,
            IsHome = i.IsHome,
            Img = i.Img,
            IsActive = i.IsActive,
            CategoryName = i.Category.CategoryName
        }).ToList();
        return View(products);
    }

    [HttpGet("create")]
    public ActionResult Create()
    {
        ViewBag.Categories = new SelectList(_context.Categories.ToList(), "Id", "CategoryName");
        return View();
    }

    [HttpPost("create")]
    public ActionResult Create(ProductCreateModel model)
    {
        var product = new Product
        {
            Name = model.Name,
            Price = model.Price,
            Description = model.Description,
            Img = "1.jpeg", //upload
            IsHome = model.IsHome,
            IsActive = model.IsActive,
            CategoryId = model.CategoryId
        };
        _context.Products.Add(product);
        _context.SaveChanges();

        return RedirectToAction("Index", "Product");
    }

}