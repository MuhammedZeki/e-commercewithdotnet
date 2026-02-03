namespace dotnet_db.Models;


public class Card
{
    public int Id { get; set; }
    public string CustomerId { get; set; } = null!;

    public List<CardItem> CardItems { get; set; } = [];
}


public class CardItem
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public int CardId { get; set; }
    public Card Card { get; set; } = null!;
    public int Quantity { get; set; }
}