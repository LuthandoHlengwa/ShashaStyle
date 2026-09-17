using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShashaStyle.Data;
using ShashaStyle.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace ShashaStyle.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Dashboard
        public async Task<IActionResult> Index()
        {
            var model = new DashboardViewModel();

            // Product statistics
            model.TotalProducts =
                await _context.Products.CountAsync();

            model.ActiveProducts =
                await _context.Products
                    .CountAsync(p => p.IsActive);

            // Category statistics
            model.TotalCategories =
                await _context.Categories.CountAsync();

            // Variant statistics
            model.ActiveVariants =
                await _context.ProductVariants
                    .CountAsync(v => v.IsActive);

            // Total stock
            model.TotalStock =
                await _context.ProductVariants
                    .SumAsync(v => v.StockQuantity);

            // Low stock
            model.LowStockCount =
                await _context.ProductVariants
                    .CountAsync(v =>
                        v.StockQuantity > 0 &&
                        v.StockQuantity <= 5);

            // Out of stock
            model.OutOfStockCount =
                await _context.ProductVariants
                    .CountAsync(v =>
                        v.StockQuantity == 0);

            // Recent products
            model.RecentProducts =
                await _context.Products
                    .Include(p => p.Category)
                    .OrderByDescending(p => p.CreatedAt)
                    .Take(5)
                    .ToListAsync();

            // Low stock products
            model.LowStockVariants =
                await _context.ProductVariants
                    .Include(v => v.Product)
                    .Where(v => v.StockQuantity <= 5)
                    .OrderBy(v => v.StockQuantity)
                    .Take(10)
                    .ToListAsync();

            return View(model);
        }
    }
}