using System.Threading.Tasks;
using dotnet_db.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dotnet_db.Controllers;



public class CartController : Controller
{
    private readonly DataContext _context;

    public CartController(DataContext context)
    {
        _context = context;
    }


    [Authorize]
    [HttpPost]
    public async Task<ActionResult> AddToCart(int productId, int quantity = 1)
    {

        var customerId = User.Identity?.Name;

        var cart = await _context.Cards
                                .Include(i => i.CardItems)
                                .Where(i => i.CustomerId == customerId)
                                .FirstOrDefaultAsync();

        if (cart == null)
        {
            cart = new Card { CustomerId = customerId! };
            _context.Cards.Add(cart);
        }


        var item = cart.CardItems.Where(i => i.ProductId == productId).FirstOrDefault();


        if (item != null)
        {
            item.Quantity += 1;

        }
        else
        {
            cart.CardItems.Add(new CardItem
            {
                ProductId = productId,
                Quantity = quantity
            });
        }

        await _context.SaveChangesAsync();


        return RedirectToAction("Index", "Home");
    }
}