namespace dotnet_db.Models;


public class Slider
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string Image { get; set; } = null!;
    public bool IsActive { get; set; }
    public int Index { get; set; }
}


