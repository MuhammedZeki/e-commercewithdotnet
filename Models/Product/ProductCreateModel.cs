using System.ComponentModel.DataAnnotations;

namespace dotnet_db.Models;

public class ProductCreateModel : ProductModel
{

    [Display(Name = "Ürün Resmi")]
    [Required(ErrorMessage = "{0} girmelisiniz.")]
    public IFormFile? Img { get; set; }

}