// using System.ComponentModel.DataAnnotations;
// using System.ComponentModel.DataAnnotations.Schema;
// using ProductApi.Models;


// namespace InventoryApi.Models
// {
//     public class InventoryLog
//     {
//         [Key]
//         public int Id { get; set; }

//         [Required]
//         [StringLength(100)]
//         public string? Name { get; set; }

//         [Required]
//         public int Quantity { get; set; }

//          // Foreign key for Product
//         [ForeignKey("Product")]
//         public int ProductId { get; set; }
//         public Product? Product { get; set; }

//     }
// }