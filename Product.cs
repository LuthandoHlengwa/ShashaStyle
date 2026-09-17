using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShashaStyle.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, 9999999)]
        public decimal Price { get; set; }

        [StringLength(500)]
        public string? ImageUrl { get; set; }

        public int CategoryId { get; set; }

        public Category? Category { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public ICollection<ProductVariant> Variants { get; set; }
            = new List<ProductVariant>();
    }
}