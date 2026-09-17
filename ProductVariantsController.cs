using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShashaStyle.Data;
using ShashaStyle.Models;
using Microsoft.AspNetCore.Authorization;

namespace ShashaStyle.Controllers
{
    [Authorize]
    public class ProductVariantsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductVariantsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProductVariants
        public async Task<IActionResult> Index(int productId)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductId == productId);

            if (product == null)
            {
                return NotFound();
            }

            var variants = await _context.ProductVariants
                .Where(v => v.ProductId == productId)
                .OrderBy(v => v.Size)
                .ThenBy(v => v.Colour)
                .ToListAsync();

            ViewBag.Product = product;

            return View(variants);
        }

        // GET: ProductVariants/Create
        public async Task<IActionResult> Create(int productId)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductId == productId);

            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Product = product;

            return View();
        }

        // POST: ProductVariants/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            int productId,
            ProductVariant variant)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductId == productId);

            if (product == null)
            {
                return NotFound();
            }

            variant.ProductId = productId;

            // Check if SKU already exists
            bool skuExists = await _context.ProductVariants
                .AnyAsync(v => v.SKU == variant.SKU);

            if (skuExists)
            {
                ModelState.AddModelError(
                    "SKU",
                    "This SKU already exists. Please use a unique SKU.");
            }

            if (ModelState.IsValid)
            {
                variant.CreatedAt = DateTime.UtcNow;

                _context.ProductVariants.Add(variant);

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] =
                    "Product variant created successfully.";

                return RedirectToAction(
                    nameof(Index),
                    new { productId });
            }

            ViewBag.Product = product;

            return View(variant);
        }

        // GET: ProductVariants/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var variant = await _context.ProductVariants
                .Include(v => v.Product)
                .FirstOrDefaultAsync(v => v.ProductVariantId == id);

            if (variant == null)
            {
                return NotFound();
            }

            return View(variant);
        }

        // POST: ProductVariants/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            ProductVariant variant)
        {
            if (id != variant.ProductVariantId)
            {
                return NotFound();
            }

            var existingVariant = await _context.ProductVariants
                .FirstOrDefaultAsync(
                    v => v.ProductVariantId == id);

            if (existingVariant == null)
            {
                return NotFound();
            }

            bool duplicateSku = await _context.ProductVariants
                .AnyAsync(v =>
                    v.SKU == variant.SKU &&
                    v.ProductVariantId != id);

            if (duplicateSku)
            {
                ModelState.AddModelError(
                    "SKU",
                    "This SKU is already being used by another variant.");
            }

            if (ModelState.IsValid)
            {
                existingVariant.Size = variant.Size;
                existingVariant.Colour = variant.Colour;
                existingVariant.SKU = variant.SKU;
                existingVariant.StockQuantity = variant.StockQuantity;
                existingVariant.IsActive = variant.IsActive;

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] =
                    "Product variant updated successfully.";

                return RedirectToAction(
                    nameof(Index),
                    new { productId = existingVariant.ProductId });
            }

            variant.Product = await _context.Products
                .FirstOrDefaultAsync(
                    p => p.ProductId == existingVariant.ProductId);

            return View(variant);
        }

        // GET: ProductVariants/Delete
        public async Task<IActionResult> Delete(int? id)
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

        // POST: ProductVariants/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var variant = await _context.ProductVariants
                .FirstOrDefaultAsync(
                    v => v.ProductVariantId == id);

            if (variant != null)
            {
                int productId = variant.ProductId;

                _context.ProductVariants.Remove(variant);

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] =
                    "Product variant deleted successfully.";

                return RedirectToAction(
                    nameof(Index),
                    new { productId });
            }

            return RedirectToAction(
                nameof(Index),
                new { productId = 0 });
        }
    }
}