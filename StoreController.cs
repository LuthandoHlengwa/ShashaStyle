using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShashaStyle.Data;
using ShashaStyle.Models.ViewModels;

namespace ShashaStyle.Controllers
{
    public class StoreController : Controller
    {
        private readonly ApplicationDbContext _context;


    public StoreController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Store
        public async Task<IActionResult> Index()
        {
            var model = new StoreHomeViewModel();

            model.Categories = await _context.Categories
                .Where(c => c.IsActive)
                .OrderBy(c => c.Name)
                .ToListAsync();

            model.LatestProducts = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Variants)
                .Where(p => p.IsActive)
                .OrderByDescending(p => p.CreatedAt)
                .Take(8)
                .ToListAsync();

            model.FeaturedProducts = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Variants)
                .Where(p => p.IsActive)
                .OrderByDescending(p => p.CreatedAt)
                .Take(4)
                .ToListAsync();

            return View(model);
        }

        // GET: /Store/Categories
        public async Task<IActionResult> Categories()
        {
            var categories = await _context.Categories
                .Where(c => c.IsActive)
                .Include(c => c.Products)
                .OrderBy(c => c.Name)
                .ToListAsync();

            return View(categories);
        }

        // GET: /Store/Product/5
        public async Task<IActionResult> Product(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Variants)
                .FirstOrDefaultAsync(p =>
                    p.ProductId == id &&
                    p.IsActive);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: /Store/Category/5
        public async Task<IActionResult> Category(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(c =>
                    c.CategoryId == id &&
                    c.IsActive);

            if (category == null)
            {
                return NotFound();
            }

            var products = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Variants)
                .Where(p =>
                    p.CategoryId == id &&
                    p.IsActive)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            ViewBag.Category = category;

            return View(products);
        }
    }

}
