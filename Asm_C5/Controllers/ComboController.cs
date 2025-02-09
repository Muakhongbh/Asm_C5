using Asm_C5.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Asm_C5.Controllers
{
    public class ComboController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ComboController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index(string comBoName, decimal? giaCombomin, decimal? giaComBomax, string moTaComBo, int page = 1, int pageSize = 5)
        {
            var combosquery = _context.Combos.AsQueryable();

            // Tìm kiếm theo tên Combo
            if (!string.IsNullOrEmpty(comBoName))
            {
                combosquery = combosquery.Where(x => x.Name.Contains(comBoName));
            }

            // Tìm kiếm theo giá
            if (giaCombomin.HasValue)
            {
                combosquery = combosquery.Where(x => x.Price >= giaCombomin.Value);
            }
            if (giaComBomax.HasValue)
            {
                combosquery = combosquery.Where(x => x.Price <= giaComBomax.Value);
            }

            ViewBag.ComBoName = comBoName;
            ViewBag.ComboMin = giaCombomin;
            ViewBag.ComboMax = giaComBomax;

            var combos = await combosquery.ToListAsync();
            return View(combos);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Combo combo, IFormFile imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                combo.ImagePath = "/images/" + fileName;
            }

            _context.Combos.Add(combo);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index","Combo");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var combo = await _context.Combos.FindAsync(id);
            if (combo == null)
            {
                return NotFound();
            }
            return View(combo);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Combo combo, IFormFile imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                if (!string.IsNullOrEmpty(combo.ImagePath))
                {
                    var oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, combo.ImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                combo.ImagePath = "/images/" + fileName;
            }

            _context.Combos.Update(combo);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index","Combo");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var combo = await _context.Combos.FindAsync(id);
            if (combo == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(combo.ImagePath))
            {
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, combo.ImagePath.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            _context.Combos.Remove(combo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
