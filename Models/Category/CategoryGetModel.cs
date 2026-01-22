namespace dotnet_db.Models;


public class CategoryGetModel
{
    public int Id { get; set; }
    public string CategoryName { get; set; } = null!;
    public string? Url { get; set; }
    public int ProductCount { get; set; }
}