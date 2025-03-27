using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ProductApi.Models;

namespace ReviewApi.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string? ReviewerName { get; set; }

        [Required]
        [StringLength(500)]
        public string? Content { get; set; }

        [Required]
        public int Rating { get; set; } // 

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        // Foreign key for Product
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product? Product { get; set; }
    }
}