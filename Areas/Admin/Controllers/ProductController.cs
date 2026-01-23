using System.Threading.Tasks;
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
    public async Task<ActionResult> Create(ProductCreateModel model)
    {

        if (model.Img == null || model.Img.Length == 0)
        {
            ModelState.AddModelError("Img", "Resim Seçiniz");
        }
        if (ModelState.IsValid)
        {

            var fileName = Path.GetRandomFileName() + ".jpg";
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await model.Img!.CopyToAsync(stream);
            }

            var product = new Product
            {
                Name = model.Name,
                Price = model.Price ?? 0,
                Description = model.Description,
                Img = fileName, //upload file bu
                IsHome = model.IsHome,
                IsActive = model.IsActive,
                CategoryId = (int)model.CategoryId!
            };
            _context.Products.Add(product);
            _context.SaveChanges();

            return RedirectToAction("Index", "Product");
        }
        ViewBag.Categories = new SelectList(_context.Categories.ToList(), "Id", "CategoryName");
        return View(model);
    }

    [HttpGet("edit/{id}")]
    public ActionResult Edit(int id)
    {

        var product = _context.Products.Select(i => new ProductEditModel
        {
            Id = i.Id,
            Name = i.Name,
            Price = i.Price,
            Description = i.Description,
            Img = i.Img,
            IsHome = i.IsHome,
            IsActive = i.IsActive,
            CategoryId = i.CategoryId
        }).FirstOrDefault(i => i.Id == id);

        ViewBag.Categories = new SelectList(_context.Categories.ToList(), "Id", "CategoryName");

        return View(product);
    }

    [HttpPost("edit/{id}")]
    public async Task<ActionResult> Edit(int id, ProductEditModel model)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        var product = _context.Products.FirstOrDefault(i => i.Id == model.Id);
        if (product != null)
        {

            if (model.ImgFile != null)
            {
                var fileName = Path.GetRandomFileName() + ".jpg";
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await model.ImgFile!.CopyToAsync(stream);
                }
                product.Img = fileName;
            }

            product.Name = model.Name;
            product.Price = model.Price;
            product.Description = model.Description;
            product.IsHome = model.IsHome;
            product.IsActive = model.IsActive;
            product.CategoryId = model.CategoryId;
            _context.SaveChanges();

            TempData["message"] = $"{product.Name} olarak başarıyla Güncellendi.";

            return RedirectToAction("Index", "Product");
        }

        return View(model);
    }
}



