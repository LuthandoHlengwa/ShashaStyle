using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using ShashaStyle.Data;
using ShashaStyle.Models;
using ShashaStyle.Models.Cart;
using ShashaStyle.Models.ViewModels;

namespace ShashaStyle.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ApplicationDbContext _context;

        private const string CartSessionKey = "ShashaStyleCart";

        public CheckoutController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Checkout
        public IActionResult Index()
        {
            var cart = GetCart();

            // Prevent checkout with an empty cart
            if (!cart.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            var subtotal = cart.Sum(item => item.Total);

            var shipping = subtotal >= 1000
                ? 0
                : 99;

            var model = new CheckoutViewModel
            {
                Subtotal = subtotal,
                Shipping = shipping,
                Total = subtotal + shipping
            };

            return View(model);
        }

        // POST: /Checkout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(
            CheckoutViewModel model)
        {
            var cart = GetCart();

            // Make sure the cart still contains items
            if (!cart.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            // Recalculate totals
            var subtotal = cart.Sum(item => item.Total);

            var shipping = subtotal >= 1000
                ? 0
                : 99;

            var total = subtotal + shipping;

            model.Subtotal = subtotal;
            model.Shipping = shipping;
            model.Total = total;

            // Validate customer information
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Check stock before creating the order
            foreach (var cartItem in cart)
            {
                var variant = await _context.ProductVariants
                    .Include(v => v.Product)
                    .FirstOrDefaultAsync(v =>
                        v.ProductVariantId ==
                        cartItem.ProductVariantId);

                if (variant == null ||
                    !variant.IsActive ||
                    variant.Product == null ||
                    !variant.Product.IsActive)
                {
                    TempData["ErrorMessage"] =
                        $"The product '{cartItem.ProductName}' is no longer available.";

                    return RedirectToAction("Index", "Cart");
                }

                if (variant.StockQuantity < cartItem.Quantity)
                {
                    TempData["ErrorMessage"] =
                        $"Not enough stock available for {cartItem.ProductName}. " +
                        $"Only {variant.StockQuantity} item(s) remain.";

                    return RedirectToAction("Index", "Cart");
                }
            }

            // Create the order
            var order = new Order
            {
                OrderNumber = GenerateOrderNumber(),

                FirstName = model.FirstName.Trim(),

                LastName = model.LastName.Trim(),

                Email = model.Email.Trim(),

                Phone = model.Phone.Trim(),

                Address = model.Address.Trim(),

                City = model.City.Trim(),

                Province = model.Province.Trim(),

                PostalCode = model.PostalCode.Trim(),

                Subtotal = subtotal,

                Shipping = shipping,

                Total = total,

                Status = "Pending",

                CreatedAt = DateTime.UtcNow
            };

            // Add order items and reduce stock
            foreach (var cartItem in cart)
            {
                var variant = await _context.ProductVariants
                    .FirstAsync(v =>
                        v.ProductVariantId ==
                        cartItem.ProductVariantId);

                // Reduce stock
                variant.StockQuantity -= cartItem.Quantity;

                var orderItem = new OrderItem
                {
                    ProductId = cartItem.ProductId,

                    ProductVariantId =
                        cartItem.ProductVariantId,

                    ProductName =
                        cartItem.ProductName,

                    Size =
                        cartItem.Size,

                    Colour =
                        cartItem.Colour,

                    SKU =
                        cartItem.SKU,

                    Quantity =
                        cartItem.Quantity,

                    UnitPrice =
                        cartItem.UnitPrice,

                    Total =
                        cartItem.Total
                };

                order.Items.Add(orderItem);
            }

            // Save order to database
            _context.Orders.Add(order);

            await _context.SaveChangesAsync();

            // Clear cart after successful order
            HttpContext.Session.Remove(CartSessionKey);

            // Send customer to confirmation page
            return RedirectToAction(
                nameof(Confirmation),
                new
                {
                    orderNumber = order.OrderNumber
                });
        }

        // GET: /Checkout/Confirmation
        public async Task<IActionResult> Confirmation(
            string? orderNumber)
        {
            if (string.IsNullOrWhiteSpace(orderNumber))
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o =>
                    o.OrderNumber == orderNumber);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // Generate a unique order number
        private string GenerateOrderNumber()
        {
            return "SS-"
                + DateTime.UtcNow.ToString("yyyyMMdd")
                + "-"
                + Guid.NewGuid()
                    .ToString("N")
                    .Substring(0, 6)
                    .ToUpper();
        }

        // Get cart from session
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
                return JsonSerializer
                    .Deserialize<List<CartItem>>(
                        cartJson)
                    ?? new List<CartItem>();
            }
            catch
            {
                return new List<CartItem>();
            }
        }
    }
}