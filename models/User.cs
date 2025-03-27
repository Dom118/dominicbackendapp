using System.ComponentModel.DataAnnotations;
using OrdersApi.Models;
using ShoppingCartApi.Models;

namespace UserApi.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string? Username { get; set; }

        [Required]
        [StringLength(100)]
        public string? FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string? LastName { get; set; }
    
        [Required]
        [StringLength(100)]
        public string? Email { get; set; }

        [Required]
        [StringLength(100)]
        public string? Password { get; set; }

        [Required]
        [StringLength(20)]
        public string? Role { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 

    }
}