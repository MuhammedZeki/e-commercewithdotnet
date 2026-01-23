using System.ComponentModel.DataAnnotations;

namespace dotnet_db.Models;


public class ProductCreateModel
{
    [Display(Name = "Ürün Adı")]
    [Required(ErrorMessage = "{0} girmelisiniz.")]
    [StringLength(20, ErrorMessage = "{0} için {2}-{1} Karakter aralığında giriniz.", MinimumLength = 3)]
    public string Name { get; set; } = null!;
    [Display(Name = "Ürün Fiyat")]
    [Required(ErrorMessage = "{0} girmelisiniz.")]
    public double? Price { get; set; }
    public string Description { get; set; } = null!;
    [Display(Name = "Ürün Resmi")]
    [Required(ErrorMessage = "{0} girmelisiniz.")]
    public IFormFile? Img { get; set; }
    public bool IsHome { get; set; }
    public bool IsActive { get; set; }
    [Display(Name = "Kategori")]
    [Required(ErrorMessage = "Lütfen bir {0} seçmelisiniz.")]
    public int? CategoryId { get; set; }
}