using System.ComponentModel.DataAnnotations;

namespace dotnet_db.Models;

public class CategoryCreateModel
{
    [Display(Name = "Category Name")]
    public string CategoryName { get; set; } = null!;

    [Display(Name = "Category Url")]
    public string CategoryUrl { get; set; } = null!;
}

