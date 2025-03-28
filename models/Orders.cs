using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OrderItemApi.Models;
using UserApi.Models;


namespace OrdersApi.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(100)]
        public string? Status { get; set; } 

        [Column(TypeName ="decimal(10,2)")]
        public decimal TotalAmount { get; set; } 
        
        public string? ShippingAddress { get; set; }


        [ForeignKey("UserId")]
        public virtual UserProfile? UserProfile { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
    
}