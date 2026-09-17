using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using ShashaStyle.Data;
using ShashaStyle.Models.Cart;
using ShashaStyle.Models.ViewModels;

namespace ShashaStyle.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        private const string CartSessionKey = "ShashaStyleCart";

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }


        // ========================================
        // CART
        // ========================================

        public IActionResult Index()
        {
            var cart = GetCart();

            var model = new CartViewModel
            {
                Items = cart
            };

            return View(model);
        }


        // ========================================
        // ADD TO CART
        // ========================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(
            int productVariantId,
            int quantity = 1)
        {
            if (quantity <= 0)
            {
                quantity = 1;
            }

            var variant = await _context.ProductVariants
                .Include(v => v.Product)
                .FirstOrDefaultAsync(v =>
                    v.ProductVariantId == productVariantId &&
                    v.IsActive &&
                    v.Product != null &&
                    v.Product.IsActive);

            if (variant == null)
            {
                TempData["ErrorMessage"] =
                    "This product variant is no longer available.";

                return RedirectToAction(
                    "Index",
                    "Store");
            }

            if (variant.StockQuantity <= 0)
            {
                TempData["ErrorMessage"] =
                    "This item is currently out of stock.";

                return RedirectToAction(
                    "Product",
                    "Store",
                    new { id = variant.ProductId });
            }


            var cart = GetCart();

            var existingItem = cart.FirstOrDefault(item =>
                item.ProductVariantId == productVariantId);


            if (existingItem != null)
            {
                var newQuantity =
                    existingItem.Quantity + quantity;

                if (newQuantity > variant.StockQuantity)
                {
                    existingItem.Quantity =
                        variant.StockQuantity;

                    TempData["ErrorMessage"] =
                        "The requested quantity exceeds available stock.";
                }
                else
                {
                    existingItem.Quantity =
                        newQuantity;

                    TempData["SuccessMessage"] =
                        "Cart updated successfully.";
                }
            }
            else
            {
                if (quantity > variant.StockQuantity)
                {
                    quantity = variant.StockQuantity;
                }

                cart.Add(new CartItem
                {
                    ProductVariantId =
                        variant.ProductVariantId,

                    ProductId =
                        variant.ProductId,

                    ProductName =
                        variant.Product!.Name,

                    Size =
                        variant.Size,

                    Colour =
                        variant.Colour,

                    SKU =
                        variant.SKU,

                    ImageUrl =
                        variant.Product.ImageUrl,

                    UnitPrice =
                        variant.Product.Price,

                    Quantity =
                        quantity,

                    AvailableStock =
                        variant.StockQuantity
                });

                TempData["SuccessMessage"] =
                    "Product added to your cart.";
            }

            SaveCart(cart);

            return RedirectToAction(nameof(Index));
        }


        // ========================================
        // UPDATE QUANTITY
        // ========================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(
            int productVariantId,
            int quantity)
        {
            var cart = GetCart();

            var item = cart.FirstOrDefault(i =>
                i.ProductVariantId == productVariantId);

            if (item == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var variant = await _context.ProductVariants
                .FirstOrDefaultAsync(v =>
                    v.ProductVariantId == productVariantId &&
                    v.IsActive);

            if (variant == null)
            {
                cart.Remove(item);

                SaveCart(cart);

                TempData["ErrorMessage"] =
                    "This product is no longer available.";

                return RedirectToAction(nameof(Index));
            }

            if (quantity <= 0)
            {
                cart.Remove(item);

                TempData["SuccessMessage"] =
                    "Item removed from your cart.";
            }
            else if (quantity > variant.StockQuantity)
            {
                item.Quantity =
                    variant.StockQuantity;

                item.AvailableStock =
                    variant.StockQuantity;

                TempData["ErrorMessage"] =
                    $"Only {variant.StockQuantity} units are available.";
            }
            else
            {
                item.Quantity = quantity;

                item.AvailableStock =
                    variant.StockQuantity;
            }

            SaveCart(cart);

            return RedirectToAction(nameof(Index));
        }


        // ========================================
        // REMOVE
        // ========================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Remove(int productVariantId)
        {
            var cart = GetCart();

            var item = cart.FirstOrDefault(i =>
                i.ProductVariantId == productVariantId);

            if (item != null)
            {
                cart.Remove(item);

                SaveCart(cart);

                TempData["SuccessMessage"] =
                    "Item removed from your cart.";
            }

            return RedirectToAction(nameof(Index));
        }


        // ========================================
        // CLEAR CART
        // ========================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Clear()
        {
            HttpContext.Session.Remove(CartSessionKey);

            TempData["SuccessMessage"] =
                "Your cart has been cleared.";

            return RedirectToAction(nameof(Index));
        }


        // ========================================
        // GET CART
        // ========================================

        private List<CartItem> GetCart()
        {
            var cartJson =
                HttpContext.Session.GetString(
                    CartSessionKey);

            if (string.IsNullOrEmpty(cartJson))
            {
                return new List<CartItem>();
            }

            try
            {
                return JsonSerializer.Deserialize<List<CartItem>>(
                    cartJson)
                    ?? new List<CartItem>();
            }
            catch
            {
                return new List<CartItem>();
            }
        }


        // ========================================
        // SAVE CART
        // ========================================

        private void SaveCart(List<CartItem> cart)
        {
            var cartJson =
                JsonSerializer.Serialize(cart);

            HttpContext.Session.SetString(
                CartSessionKey,
                cartJson);
        }
    }
}