using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShashaStyle.Data;
using Microsoft.AspNetCore.Authorization;

namespace ShashaStyle.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Orders
        public async Task<IActionResult> Index(string? search, string? status)
        {
            var query = _context.Orders
                .Include(o => o.Items)
                .AsQueryable();

            // Search by order number or customer
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                query = query.Where(o =>
                    o.OrderNumber.Contains(search) ||
                    o.FirstName.Contains(search) ||
                    o.LastName.Contains(search) ||
                    o.Email.Contains(search));
            }

            // Filter by status
            if (!string.IsNullOrWhiteSpace(status) &&
                status != "All")
            {
                query = query.Where(o =>
                    o.Status == status);
            }

            var orders = await query
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            ViewBag.Search = search;
            ViewBag.Status = status ?? "All";

            return View(orders);
        }

        // GET: /Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o =>
                    o.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: /Orders/UpdateStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(
            int id,
            string status)
        {
            var allowedStatuses = new[]
            {
                "Pending",
                "Processing",
                "Shipped",
                "Delivered",
                "Cancelled"
            };

            if (!allowedStatuses.Contains(status))
            {
                TempData["ErrorMessage"] =
                    "Invalid order status.";

                return RedirectToAction(
                    nameof(Details),
                    new { id });
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(o =>
                    o.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            order.Status = status;
            order.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                $"Order {order.OrderNumber} status updated to {status}.";

            return RedirectToAction(
                nameof(Details),
                new { id });
        }
    }
}