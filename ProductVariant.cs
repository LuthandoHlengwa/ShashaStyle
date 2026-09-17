using System.ComponentModel.DataAnnotations;

namespace ShashaStyle.Models
{
    public class ProductVariant
    {
        public int ProductVariantId { get; set; }

        public int ProductId { get; set; }

        public Product? Product { get; set; }

        [Required]
        [StringLength(50)]
        public string Size { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Colour { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string SKU { get; set; } = string.Empty;

        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}