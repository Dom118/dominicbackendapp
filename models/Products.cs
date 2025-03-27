using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OrderItemApi.Models;
using ShoppingCartApi.Models;


namespace ProductApi.Models
{
    public class Product
    {
        [Key]
        public int Id {get; set;}
        [Required]
        [StringLength(100)]
        public string? Name {get; set;}
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price {get; set;}
        public int StockQuantity {get; set;}
        
        public string? ImageUrl { get; set; }
        public string? ImageFileName { get; set; }

    }
}
