using Asm_C5.Models;
using Microsoft.AspNetCore.Mvc;

namespace Asm_C5.Controllers
{
    public class BanHangController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BanHangController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Details(int id)
        {
            var foodItem = await _context.FoodItems.FindAsync(id);
            if (foodItem == null)
                return NotFound();

            return View(foodItem);
        }
    }
}
