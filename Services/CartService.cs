using dotnet_db.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnet_db.Services;

public interface ICartService
{
    string GetCustomerId();
    Task<Card> GetCart(string customerId);
    Task AddToCart(int productId, int quantity = 1);
    Task RemoveFromCart(int cartItemId);
    Task InceareItem(int cartItemId);
    Task DecreaseItem(int cartItemId);
    Task TransferCartToUser(string username);
}

public class CartService : ICartService
{
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;


    public CartService(DataContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task AddToCart(int productId, int quantity = 1)
    {
        var cart = await GetCart(GetCustomerId());

        var product = await _context.Products.FirstOrDefaultAsync(i => i.Id == productId);

        if (product != null)
        {
            cart.AddItemCart(product, quantity);
            await _context.SaveChangesAsync();
        }

        await _context.SaveChangesAsync();
    }

    public async Task DecreaseItem(int cartItemId)
    {
        var cart = await GetCart(GetCustomerId());
        cart.DecreaseItem(cartItemId);
        await _context.SaveChangesAsync();
    }

    public async Task<Card> GetCart(string cusId)
    {
        var customerId =
         _httpContextAccessor.HttpContext?.User.Identity?.Name ??
         _httpContextAccessor.HttpContext?.Request.Cookies["customerId"];

        var cart = await _context.Cards
                                .Include(i => i.CardItems)
                                .ThenInclude(i => i.Product)
                                .Where(i => i.CustomerId == cusId)
                                .FirstOrDefaultAsync();

        if (cart == null)
        {
            customerId = _httpContextAccessor.HttpContext?.User.Identity?.Name;

            if (string.IsNullOrEmpty(customerId))
            {
                customerId = Guid.NewGuid().ToString();
                var options = new CookieOptions
                {
                    Expires = DateTime.Now.AddMonths(1),
                    IsEssential = true
                };
                _httpContextAccessor.HttpContext?.Response.Cookies.Append("customerId", customerId, options);
            }
            cart = new Card { CustomerId = customerId };
            _context.Cards.Add(cart);
        }
        return cart;
    }

    public string GetCustomerId()
    {
        var context = _httpContextAccessor.HttpContext;
        return context?.User.Identity?.Name ?? context?.Request.Cookies["customerId"]!;
    }

    public async Task InceareItem(int cartItemId)
    {
        var cart = await GetCart(GetCustomerId());
        cart.InceareItem(cartItemId);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveFromCart(int cartItemId)
    {
        var cart = await GetCart(GetCustomerId());

        cart.RemoveItem(cartItemId);

        await _context.SaveChangesAsync();

    }

    public async Task TransferCartToUser(string username)
    {
        var userCart = await GetCart(username);

        var cookieCart = await GetCart(_httpContextAccessor.HttpContext?.Request.Cookies["customerId"]!);

        foreach (var item in cookieCart?.CardItems!)
        {
            var cartItem = userCart?.CardItems.Where(i => i.ProductId == item.ProductId).FirstOrDefault();
            if (cartItem != null)
            {
                cartItem.Quantity += item.Quantity;
            }
            else
            {
                userCart?.CardItems.Add(new CardItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                });
            }
        }

        _context.Cards.Remove(cookieCart);
        await _context.SaveChangesAsync();
    }
}