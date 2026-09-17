using System.ComponentModel.DataAnnotations;

namespace ShashaStyle.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }

        public int OrderId { get; set; }

        public Order? Order { get; set; }

        public int ProductId { get; set; }

        public int ProductVariantId { get; set; }

        [Required]
        [StringLength(150)]
        public string ProductName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Size { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Colour { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string SKU { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Total { get; set; }
    }
}