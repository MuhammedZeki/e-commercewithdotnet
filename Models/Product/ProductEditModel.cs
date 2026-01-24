using System.ComponentModel.DataAnnotations;

namespace dotnet_db.Models;


public class ProductEditModel : ProductModel
{
    public int Id { get; set; }

    public IFormFile? ImgFile { get; set; }

    public string? ImgName { get; set; }

}