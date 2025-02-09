using Asm_C5.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Asm_C5.Controllers
{
    public class FoodController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FoodController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var foodItems = await _context.FoodItems.ToListAsync();
            return View(foodItems);
        }

        public async Task<IActionResult> Search(string name)
        {
            var result = await _context.FoodItems
                .Where(f => f.Name.Contains(name))
                .ToListAsync();
            return View("Index", result); 
        }

        public async Task<IActionResult> AdvancedSearch(string name, decimal? price, string category)
        {
            var query = _context.FoodItems.AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(f => f.Name.Contains(name));
            if (price.HasValue)
                query = query.Where(f => f.Price <= price.Value);
            if (!string.IsNullOrEmpty(category))
                query = query.Where(f => f.Category.Contains(category));

            var result = await query.ToListAsync();
            return View("Index", result); 
        }

        public async Task<IActionResult> Details(int id)
        {
            var foodItem = await _context.FoodItems.FindAsync(id);
            if (foodItem == null)
                return NotFound();

            return View(foodItem); 
        }

        //[Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(FoodItem foodItem, IFormFile imageFile)
        {
            var existingFoodItem = await _context.FoodItems
                   .FirstOrDefaultAsync(f => f.Name == foodItem.Name);

            if (existingFoodItem != null)
            {
                existingFoodItem.Quantity += foodItem.Quantity;
                _context.FoodItems.Update(existingFoodItem);
            }
            else
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    var filePath = Path.Combine("wwwroot/images", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }
                    foodItem.ImageUrl = "/images/" + fileName;
                }
            }
            _context.FoodItems.Add(foodItem);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Food");
        }



        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var foodItem = _context.FoodItems.Find(id);
            if (foodItem == null)
                return NotFound();

            return View(foodItem);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(FoodItem foodItem, IFormFile imageFile)
        {

            if (imageFile != null && imageFile.Length > 0)
            {
                if (!string.IsNullOrEmpty(foodItem.ImageUrl))
                {
                    var oldFilePath = Path.Combine("wwwroot", foodItem.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var filePath = Path.Combine("wwwroot/images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                foodItem.ImageUrl = "/images/" + fileName;
            }

            _context.FoodItems.Update(foodItem);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index","Food");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var foodItem = await _context.FoodItems.FindAsync(id);
            if (foodItem == null)
                return NotFound();

            _context.FoodItems.Remove(foodItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
