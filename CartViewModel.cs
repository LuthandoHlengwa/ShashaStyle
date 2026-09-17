using ShashaStyle.Models.Cart;

namespace ShashaStyle.Models.ViewModels
{
    public class CartViewModel
    {
        public List<CartItem> Items { get; set; }
            = new List<CartItem>();

        public decimal Subtotal =>
            Items.Sum(item => item.Total);

        public decimal Shipping
        {
            get
            {
                if (!Items.Any())
                {
                    return 0;
                }

                return Subtotal >= 1000 ? 0 : 99;
            }
        }

        public decimal Total =>
            Subtotal + Shipping;

        public int TotalItems =>
            Items.Sum(item => item.Quantity);
    }
}