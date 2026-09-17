using ShashaStyle.Models;

namespace ShashaStyle.Models.ViewModels
{
    public class StoreHomeViewModel
    {
        public List<Category> Categories { get; set; }
            = new List<Category>();

        public List<Product> FeaturedProducts { get; set; }
            = new List<Product>();

        public List<Product> LatestProducts { get; set; }
            = new List<Product>();
    }
}