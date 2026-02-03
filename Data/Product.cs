namespace dotnet_db.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public double Price { get; set; }
    public string Description { get; set; } = null!;
    public bool IsHome { get; set; }
    public string? Img { get; set; }
    public bool IsActive { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

}