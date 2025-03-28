using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using OrdersApi.Models;
using OrderItemApi.Models;
using MinimalX.Data;

namespace MinimalX.OrderControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        // Convert cart items into an order
        [HttpPost("checkout")]
        [Authorize]
        public async Task<ActionResult> Checkout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // get the cart items
            var cartItems = await _context.ShoppingCart
                .Where(ci => ci.UserId == userId)
                .ToListAsync();

            if (!cartItems.Any())
                return BadRequest("Cart is empty.");

            // create an order
            var order = new Order
            {
                UserId = userId,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow,
                OrderItems = cartItems.Select(ci => new OrderItem
                {
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    UnitPrice = ci.UnitPrice
                }).ToList()
            };

            _context.Orders.Add(order);

            // clear the cart
            _context.ShoppingCart.RemoveRange(cartItems);

            await _context.SaveChangesAsync();
            return Ok(new { OrderId = order.Id, Message = "Checkout complete" });
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Order>>> GetMyOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            return Ok(orders);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);

            if (order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "administrator, vendor")]
        public async Task<ActionResult> UpdateOrderStatus(int id, [FromBody] string newStatus)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return NotFound();

            order.Status = newStatus;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("markpaid/{orderId}")]
        [Authorize]
        public async Task<IActionResult> MarkPaid(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                return NotFound($"Order {orderId} not found.");

            
            // If it's already paid, do nothing
            if (order.Status == "Paid")
                return Ok("Order is already paid.");

            // Mark order as paid
            order.Status = "Paid";

            // Remove the user's cart items (if any remain)
            var userCart = _context.ShoppingCart
                .Where(ci => ci.UserId == order.UserId)
                .ToList();
            _context.ShoppingCart.RemoveRange(userCart);

            // Reduce product stock for each item in this order
            foreach (var item in order.OrderItems)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product != null)
                {
                    product.StockQuantity = Math.Max(0, product.StockQuantity - item.Quantity);
                }
            }

            await _context.SaveChangesAsync();
            return Ok($"Order {orderId} marked as Paid. Cart cleared, stock reduced.");
        }

    }
}