using System.ComponentModel.DataAnnotations;

namespace dotnet_db.Models;

public class CategoryEditModel
{
    public int Id { get; set; }

    [Display(Name = "Category Name")]
    public string CategoryName { get; set; } = null!;

    [Display(Name = "Category Url")]
    public string CategoryUrl { get; set; } = null!;
}

