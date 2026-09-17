using System.ComponentModel.DataAnnotations;

namespace ShashaStyle.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        [Required]
        [StringLength(30)]
        public string OrderNumber { get; set; } = string.Empty;

        // Customer information
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Phone]
        [StringLength(30)]
        public string Phone { get; set; } = string.Empty;

        // Delivery address
        [Required]
        [StringLength(250)]
        public string Address { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string City { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Province { get; set; } = string.Empty;

        [Required]
        [StringLength(10)]
        public string PostalCode { get; set; } = string.Empty;

        // Order totals
        [Range(0, 999999999)]
        public decimal Subtotal { get; set; }

        [Range(0, 999999999)]
        public decimal Shipping { get; set; }

        [Range(0, 999999999)]
        public decimal Total { get; set; }

        // Status
        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Pending";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public ICollection<OrderItem> Items { get; set; }
            = new List<OrderItem>();
    }
}