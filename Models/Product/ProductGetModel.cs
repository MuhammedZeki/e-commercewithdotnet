namespace dotnet_db.Models;

public class ProductGetModel
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public double Price { get; set; }
    public bool IsHome { get; set; }
    public bool IsActive { get; set; }
    public string? Img { get; set; }
    public string CategoryName { get; set; } = null!;
}