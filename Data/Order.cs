namespace dotnet_db.Models;


public class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public string Username { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string PostalCode { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Email { get; set; } = null!;
    public double TotalPrice { get; set; }
    public List<OrderItem> OrderItems { get; set; } = [];
}

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public int Quantity { get; set; }
    public double Price { get; set; }

}