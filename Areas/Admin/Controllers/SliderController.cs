using dotnet_db.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace dotnet_db.Controllers;



[Area("Admin")]
[Route("admin/slider")]
public class SliderController : Controller
{

    private readonly DataContext _context;

    public SliderController(DataContext context)
    {
        _context = context;
    }


    [HttpGet("")]
    public ActionResult Index()
    {
        var sliders = _context.Sliders.Select(i => new SliderGetModel
        {
            Id = i.Id,
            Title = (string)i.Title!,
            Image = i.Image,
            IsActive = i.IsActive,
            Index = i.Index
        }).ToList();


        return View(sliders);
    }

    [HttpGet("create")]
    public ActionResult Create()
    {
        return View();
    }

    [HttpPost("create")]
    public async Task<ActionResult> Create(SliderCreateModel model)
    {
        if (model.Image == null || model.Image.Length == 0)
        {
            ModelState.AddModelError("Image", "Resim Seçiniz");
        }
        if (ModelState.IsValid)
        {
            var fileName = Path.GetRandomFileName() + ".jpg";
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await model.Image!.CopyToAsync(stream);
            }

            var slider = new Slider()
            {
                Title = model.Title,
                Description = model.Description,
                Image = fileName,
                IsActive = model.IsActive,
                Index = model.Index
            };
            _context.Sliders.Add(slider);
            _context.SaveChanges();

            return RedirectToAction("Index", "Slider");
        }
        return View(model);
    }


    [HttpGet("edit/{id}")]
    public ActionResult Edit(int id)
    {

        var slider = _context.Sliders.Select(i => new SliderEditModel
        {
            Id = i.Id,
            Title = (string)i.Title!,
            Description = i.Description,
            ImageName = i.Image,
            IsActive = i.IsActive,
            Index = i.Index
        }).FirstOrDefault(i => i.Id == id);

        return View(slider);
    }


    [HttpPost("edit/{id}")]
    public async Task<ActionResult> Edit(int id, SliderEditModel model)
    {

        if (id != model.Id)
        {
            return RedirectToAction("Index");
        }

        if (ModelState.IsValid)
        {
            var slider = _context.Sliders.FirstOrDefault(i => i.Id == model.Id);
            if (slider != null)
            {
                if (model.ImageFile != null)
                {
                    var fileName = Path.GetRandomFileName() + ".jpg";
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await model.ImageFile!.CopyToAsync(stream);
                    }
                    slider.Image = fileName;
                }
                slider.Title = model.Title;
                slider.Description = model.Description;
                slider.IsActive = model.IsActive;
                slider.Index = model.Index;
                _context.SaveChanges();

                TempData["message"] = $"{slider.Title} olarak başarıyla Güncellendi.";

                return RedirectToAction("Index", "Slider");
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

        var slider = _context.Sliders.FirstOrDefault(i => i.Id == id);

        if (slider != null)
        {
            return View(slider);
        }
        return RedirectToAction("Index", "Slider");

    }

    [HttpPost("delete/{id}")]
    public IActionResult DeleteConfirmed(int id)
    {
        var slider = _context.Sliders.Find(id);
        if (slider == null)
        {
            return NotFound();
        }

        _context.Sliders.Remove(slider);
        _context.SaveChanges();

        TempData["message"] = $"{slider.Title} silindi.";
        return RedirectToAction("Index");
    }
}