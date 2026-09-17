using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShashaStyle.Data;
using ShashaStyle.Models;

namespace ShashaStyle.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Variants)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return View(products);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Variants)
                .FirstOrDefaultAsync(
                    p => p.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public async Task<IActionResult> Create()
        {
            await LoadCategories();

            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                product.CreatedAt = DateTime.UtcNow;

                _context.Products.Add(product);

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] =
                    "Product created successfully.";

                return RedirectToAction(nameof(Index));
            }

            await LoadCategories(product.CategoryId);

            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(
                    p => p.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            await LoadCategories(product.CategoryId);

            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    product.UpdatedAt = DateTime.UtcNow;

                    _context.Update(product);

                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] =
                        "Product updated successfully.";

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }

                    throw;
                }
            }

            await LoadCategories(product.CategoryId);

            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Variants)
                .FirstOrDefaultAsync(
                    p => p.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products
                .Include(p => p.Variants)
                .FirstOrDefaultAsync(
                    p => p.ProductId == id);

            if (product != null)
            {
                _context.Products.Remove(product);

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] =
                    "Product deleted successfully.";
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadCategories(
            int? selectedCategory = null)
        {
            var categories = await _context.Categories
                .Where(c => c.IsActive)
                .OrderBy(c => c.Name)
                .ToListAsync();

            ViewBag.Categories = new SelectList(
                categories,
                "CategoryId",
                "Name",
                selectedCategory);
        }

        private bool ProductExists(int id)
        {
            return _context.Products
                .Any(p => p.ProductId == id);
        }
    }
}