using dotnet_db.Models;
using dotnet_db.Services;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_db.Controllers;


[Route("cart")]
public class CartController : Controller
{
    private readonly DataContext _context;
    private readonly ICartService _cartService;


    public CartController(DataContext context, ICartService cartService)
    {
        _context = context;
        _cartService = cartService;

    }

    [HttpGet("")]
    public async Task<ActionResult> Index()
    {
        var customerId = _cartService.GetCustomerId();
        var cart = await _cartService.GetCart(customerId);
        return View(cart);
    }

    [HttpPost("add-to-cart")]
    public async Task<ActionResult> AddToCart(int productId, int quantity = 1)
    {

        await _cartService.AddToCart(productId, quantity);
        return RedirectToAction("Index");
    }

    [HttpPost("remove-item")]
    public async Task<ActionResult> Remove(int cartItemId)
    {

        await _cartService.RemoveFromCart(cartItemId);
        return RedirectToAction("Index");
    }

    [HttpPost("increase-item")]
    public async Task<ActionResult> Increase(int cartItemId)
    {
        await _cartService.InceareItem(cartItemId);
        return RedirectToAction("Index");

    }

    [HttpPost("decrease-item")]
    public async Task<ActionResult> Decrease(int cartItemId)
    {
        await _cartService.DecreaseItem(cartItemId);
        return RedirectToAction("Index");
    }



}