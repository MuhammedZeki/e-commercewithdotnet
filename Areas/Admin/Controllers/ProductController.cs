using System.Threading.Tasks;
using dotnet_db.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace dotnet_db.Areas.Admin.Controllers;

[Authorize(Roles = "Admin")]
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
    public ActionResult Index(int? kategori)
    {

        var query = _context.Products.AsQueryable();
        if (kategori != null)
        {
            query = query.Where(i => i.CategoryId == kategori);
        }
        var products = query.Select(i => new ProductGetModel
        {
            Id = i.Id,
            Name = i.Name,
            Price = i.Price,
            IsHome = i.IsHome,
            Img = i.Img,
            IsActive = i.IsActive,
            CategoryName = i.Category.CategoryName,
            Count = i.Category.Products.Count()
        }).ToList();

        ViewBag.Categories = new SelectList(_context.Categories.ToList(), "Id", "CategoryName", kategori);

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
            ImgName = i.Img,
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


        if (ModelState.IsValid)
        {

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
                product.Price = model.Price ?? 0;
                product.Description = model.Description;
                product.IsHome = model.IsHome;
                product.IsActive = model.IsActive;
                product.CategoryId = model.CategoryId;
                _context.SaveChanges();

                TempData["message"] = $"{product.Name} olarak başarıyla Güncellendi.";

                return RedirectToAction("Index", "Product");
            }
        }
        ViewBag.Categories = new SelectList(_context.Categories.ToList(), "Id", "CategoryName");
        return View(model);
    }


    [HttpGet("delete/{id}")]
    public ActionResult Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = _context.Products.FirstOrDefault(i => i.Id == id);

        if (product != null)
        {
            return View(product);
        }
        return RedirectToAction("Index", "Product");

    }

    [HttpPost("delete/{id}")]
    public IActionResult DeleteConfirmed(int id)
    {
        var product = _context.Products.Find(id);
        if (product == null)
        {
            return NotFound();
        }

        _context.Products.Remove(product);
        _context.SaveChanges();

        TempData["message"] = $"{product.Name} silindi.";
        return RedirectToAction("Index");
    }
}



