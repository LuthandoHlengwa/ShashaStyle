using ShashaStyle.Models;

namespace ShashaStyle.Models.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalProducts { get; set; }

        public int TotalCategories { get; set; }

        public int TotalStock { get; set; }

        public int LowStockCount { get; set; }

        public int OutOfStockCount { get; set; }

        public int ActiveProducts { get; set; }

        public int ActiveVariants { get; set; }

        public List<Product> RecentProducts { get; set; }
            = new List<Product>();

        public List<ProductVariant> LowStockVariants { get; set; }
            = new List<ProductVariant>();
    }
}