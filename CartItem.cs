namespace ShashaStyle.Models.Cart
{
    public class CartItem
    {
        public int ProductVariantId { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public string Size { get; set; } = string.Empty;

        public string Colour { get; set; } = string.Empty;

        public string SKU { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }

        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }

        public int AvailableStock { get; set; }

        public decimal Total =>
            UnitPrice * Quantity;
    }
}