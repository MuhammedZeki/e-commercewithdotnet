using dotnet_db.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_db.Controllers;



[Authorize]
[Route("order")]
public class OrderController : Controller
{
    private ICartService _cartService;
    public OrderController(ICartService cartService)
    {
        _cartService = cartService;
    }

    public async Task<ActionResult> CheckOut()
    {
        ViewBag.Cart = await _cartService.GetCart(User.Identity?.Name!);
        return View();
    }

}

