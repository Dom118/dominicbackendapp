using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ShoppingCartApi.Models;
using OrderItemApi.Models;


namespace OrdersApi.Models
{
 public class Order
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // e.g. "Pending", "Awaiting Payment", "Paid", "Failed"
        public string Status { get; set; } = "Awaiting Payment";

        // Store the Stripe session ID to match in the webhook
        public string? StripeSessionId { get; set; }

        public List<OrderItem> OrderItems { get; set; } = new();
    }
    
}