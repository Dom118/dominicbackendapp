// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using OrdersApi.Models;
// using MinimalX.Data;

// namespace MinimalX.OrderControllers
// {
//     [Route("api/[controller]")]
//     [ApiController]
//     public class OrderController : ControllerBase
//     {   
//         // connection to database
//         private readonly AppDbContext _context;
//         public OrderController(AppDbContext context)
//         {
//             _context = context;
//         }

//         // retrieve all orders
//         [HttpGet]
//         public async Task<IEnumerable<Order>> GetOrders() => await _context.Orders.ToListAsync();

//         // retrieve single order
//         [HttpGet("{id}")]
//         public async Task<ActionResult<Order>> GetOrder(int id)
//         {
//             var order = await _context.Orders.FindAsync(id);
//             return order == null ? NotFound() : order;
//         }

//     }
// }
