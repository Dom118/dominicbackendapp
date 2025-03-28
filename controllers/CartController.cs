using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using MinimalX.Data;
using OrderItemApi.Models;
using ShoppingCartApi.Models;
using ShoppingCartApi.Dtos;


namespace ProductApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<CartItem>>> GetCartItems()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var items = await _context.ShoppingCart
                .Where(ci => ci.UserId == userId)
                .ToListAsync();

            return Ok(items);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> AddToCart([FromBody] CartItemDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            // Build a new CartItem using the authenticated user's ID
            var newItem = new CartItem
            {
                UserId = userId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                UnitPrice = dto.UnitPrice
            };

            // Check if the user already has this product in their cart
            var existing = await _context.ShoppingCart
                .FirstOrDefaultAsync(ci => ci.UserId == userId && ci.ProductId == newItem.ProductId);

            if (existing == null)
            {
                // Add a new cart item
                _context.ShoppingCart.Add(newItem);
            }
            else
            {
                // Update existing cart item (e.g. increment quantity)
                existing.Quantity += newItem.Quantity;
                existing.UnitPrice = newItem.UnitPrice;
                _context.ShoppingCart.Update(existing);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }


        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> UpdateQuantity(int id, [FromBody] int newQuantity)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var item = await _context.ShoppingCart.FindAsync(id);
            if (item == null || item.UserId != userId)
                return NotFound();

            item.Quantity = newQuantity;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> RemoveItem(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var item = await _context.ShoppingCart.FindAsync(id);
            if (item == null || item.UserId != userId)
                return NotFound();

            _context.ShoppingCart.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}