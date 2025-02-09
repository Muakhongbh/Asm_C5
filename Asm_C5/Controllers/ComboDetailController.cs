using Asm_C5.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Asm_C5.Controllers
{
    public class ComboDetailController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ComboDetailController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ComboDetail
        public async Task<IActionResult> Index(string comboName, string foodName, int? minQuantity, int? maxQuantity)
        {
            var query = from cfi in _context.ComboDetails
                        join c in _context.Combos on cfi.ComboId equals c.Id
                        join f in _context.FoodItems on cfi.FoodItemId equals f.Id
                        select new
                        {
                            ComboId = c.Id,
                            ComboName = c.Name,
                            ComboPrice = c.Price,
                            ComboImageUrl = c.ImagePath,
                            FoodName = f.Name,
                            FoodPrice = f.Price,
                            FoodQuantity = cfi.Quantity,
                            FoodImageUrl = f.ImageUrl,
                            TotalPrice = cfi.Quantity * f.Price
                        };

            // Lọc theo tên combo
            if (!string.IsNullOrEmpty(comboName))
            {
                query = query.Where(item => item.ComboName.Contains(comboName));
            }

            // Lọc theo tên món ăn
            if (!string.IsNullOrEmpty(foodName))
            {
                query = query.Where(item => item.FoodName.Contains(foodName));
            }

            // Lọc theo số lượng
            if (minQuantity.HasValue)
            {
                query = query.Where(item => item.FoodQuantity >= minQuantity.Value);
            }
            if (maxQuantity.HasValue)
            {
                query = query.Where(item => item.FoodQuantity <= maxQuantity.Value);
            }

            ViewBag.ComboName = comboName;
            ViewBag.FoodName = foodName;
            ViewBag.MinQuantity = minQuantity;
            ViewBag.MaxQuantity = maxQuantity;

            var result = await query.ToListAsync(); // Chuyển đổi sang List

            return View(result); // Trả về danh sách
        }
        public async Task<IActionResult> Detail(int id)
        {
            var combo = await _context.Combos
                .Include(c => c.ComboDetails)
                .ThenInclude(cd => cd.FoodItem)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (combo == null)
            {
                return NotFound();
            }

            var comboDetails = new
            {
                ComboId = combo.Id,
                ComboName = combo.Name,
                ComboPrice = combo.Price,
                ComboImageUrl = combo.ImagePath,
                ComboDescription = "Mô tả về combo", 
                FoodItems = combo.ComboDetails
                    .Select(cd => new
                    {
                        FoodName = cd.FoodItem.Name,
                        FoodPrice = cd.FoodItem.Price,
                        FoodImageUrl = cd.FoodItem.ImageUrl,
                        Quantity = cd.Quantity,
                        TotalFoodPrice = cd.FoodItem.Price * cd.Quantity
                    }).ToList(),
                TotalComboPrice = combo.ComboDetails
                    .Sum(cd => cd.FoodItem.Price * cd.Quantity)
            };

            return View(comboDetails);
        }


        public IActionResult Create(int comboId)
        {
            ViewBag.ComboId = comboId;
            ViewBag.FoodItems = _context.FoodItems.ToList(); 
            return View();
        }

        // POST: ComboDetail/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ComboDetail comboDetail)
        {
            if (ModelState.IsValid)
            {
                _context.ComboDetails.Add(comboDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { comboId = comboDetail.ComboId });
            }
            return View(comboDetail);
        }

        // GET: ComboDetail/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var comboDetail = await _context.ComboDetails.FindAsync(id);
            if (comboDetail == null)
            {
                return NotFound();
            }
            ViewBag.FoodItems = await _context.FoodItems.ToListAsync(); 
            return View(comboDetail);
        }

        // POST: ComboDetail/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ComboDetail comboDetail)
        {
            if (id != comboDetail.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comboDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComboDetailExists(comboDetail.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index), new { comboId = comboDetail.ComboId });
            }
            return View(comboDetail);
        }

        // GET: ComboDetail/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var comboDetail = await _context.ComboDetails
                .Include(cd => cd.FoodItem)
                .FirstOrDefaultAsync(cd => cd.Id == id);
            if (comboDetail == null)
            {
                return NotFound();
            }
            return View(comboDetail);
        }

        // POST: ComboDetail/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comboDetail = await _context.ComboDetails.FindAsync(id);
            _context.ComboDetails.Remove(comboDetail);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { comboId = comboDetail.ComboId });
        }

        private bool ComboDetailExists(int id)
        {
            return _context.ComboDetails.Any(e => e.Id == id);
        }
    }
}
