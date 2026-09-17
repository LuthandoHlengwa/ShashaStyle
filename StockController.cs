using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShashaStyle.Data;
using ShashaStyle.Models;
using Microsoft.AspNetCore.Authorization;

namespace ShashaStyle.Controllers
{
    [Authorize]
    public class StockController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StockController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Stock
        public async Task<IActionResult> Index(string? search, string? status)
        {
            var query = _context.ProductVariants
                .Include(v => v.Product)
                .AsQueryable();

            // Search
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                query = query.Where(v =>
                    v.SKU.Contains(search) ||
                    v.Size.Contains(search) ||
                    v.Colour.Contains(search) ||
                    (v.Product != null &&
                     v.Product.Name.Contains(search)));
            }

            // Filter
            if (status == "low")
            {
                query = query.Where(v =>
                    v.StockQuantity > 0 &&
                    v.StockQuantity <= 5);
            }
            else if (status == "out")
            {
                query = query.Where(v =>
                    v.StockQuantity == 0);
            }
            else if (status == "available")
            {
                query = query.Where(v =>
                    v.StockQuantity > 5);
            }

            var variants = await query
                .OrderBy(v => v.StockQuantity)
                .ThenBy(v => v.Product!.Name)
                .ToListAsync();

            ViewBag.Search = search;
            ViewBag.Status = status;

            return View(variants);
        }

        // GET: Stock/Adjust/5
        public async Task<IActionResult> Adjust(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var variant = await _context.ProductVariants
                .Include(v => v.Product)
                .FirstOrDefaultAsync(
                    v => v.ProductVariantId == id);

            if (variant == null)
            {
                return NotFound();
            }

            return View(variant);
        }

        // POST: Stock/Adjust
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Adjust(
            int id,
            string actionType,
            int quantity)
        {
            if (quantity <= 0)
            {
                TempData["ErrorMessage"] =
                    "Quantity must be greater than zero.";

                return RedirectToAction(
                    nameof(Adjust),
                    new { id });
            }

            var variant = await _context.ProductVariants
                .Include(v => v.Product)
                .FirstOrDefaultAsync(
                    v => v.ProductVariantId == id);

            if (variant == null)
            {
                return NotFound();
            }

            if (actionType == "add")
            {
                variant.StockQuantity += quantity;

                TempData["SuccessMessage"] =
                    $"{quantity} units added successfully.";
            }
            else if (actionType == "remove")
            {
                if (quantity > variant.StockQuantity)
                {
                    TempData["ErrorMessage"] =
                        "You cannot remove more stock than is currently available.";

                    return RedirectToAction(
                        nameof(Adjust),
                        new { id });
                }

                variant.StockQuantity -= quantity;

                TempData["SuccessMessage"] =
                    $"{quantity} units removed successfully.";
            }
            else
            {
                TempData["ErrorMessage"] =
                    "Invalid stock adjustment.";

                return RedirectToAction(
                    nameof(Adjust),
                    new { id });
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}