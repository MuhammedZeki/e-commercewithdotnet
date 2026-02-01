using dotnet_db.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_db.Areas.Admin.Controllers;

[Authorize(Roles = "Admin")]
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
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var category = new Category
        {
            CategoryName = model.CategoryName,
            Url = model.CategoryUrl
        };
        _context.Categories.Add(category);
        _context.SaveChanges();

        return RedirectToAction("Index", "Category");
    }

    [HttpGet("edit/{id}")]
    public ActionResult Edit(int id)
    {

        var category = _context.Categories.Select(i => new CategoryEditModel
        {
            Id = i.Id,
            CategoryName = i.CategoryName,
            CategoryUrl = i.Url

        }).FirstOrDefault(i => i.Id == id);

        return View(category);
    }

    [HttpPost("edit/{id}")]
    public ActionResult Edit(int id, CategoryEditModel model)
    {
        if (id != model.Id)
        {
            return NotFound();
        }
        if (ModelState.IsValid)
        {
            var category = _context.Categories.FirstOrDefault(i => i.Id == model.Id);
            if (category != null)
            {
                category.CategoryName = model.CategoryName;
                category.Url = model.CategoryUrl;
                _context.SaveChanges();

                TempData["message"] = $"{category.CategoryName} olarak başarıyla Güncellendi.";

                return RedirectToAction("Index", "Category");
            }
        }
        return View(model);
    }

    [HttpGet("delete/{id}")]
    public ActionResult Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var category = _context.Categories.FirstOrDefault(i => i.Id == id);

        if (category != null)
        {
            return View(category);
        }
        return RedirectToAction("Index", "Category");

    }

    [HttpPost("delete/{id}")]
    public IActionResult DeleteConfirmed(int id)
    {
        var category = _context.Categories.Find(id);
        if (category == null)
        {
            return NotFound();
        }

        _context.Categories.Remove(category);
        _context.SaveChanges();

        TempData["message"] = $"{category.CategoryName} silindi.";
        return RedirectToAction("Index");
    }
}