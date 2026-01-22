namespace dotnet_db.Models;


public class ProductCreateModel
{
    public string Name { get; set; } = null!;
    public double Price { get; set; }
    public string Description { get; set; } = null!;
    public string? Img { get; set; }
    public bool IsHome { get; set; }
    public bool IsActive { get; set; }
    public int CategoryId { get; set; }
}