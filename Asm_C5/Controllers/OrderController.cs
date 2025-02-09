using Asm_C5.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Asm_C5.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Customer đặt món
        [Authorize(Roles = "Customer")]
        [HttpPost("place-order")]
        public async Task<IActionResult> PlaceOrder([FromBody] Order order)
        {
            order.OrderDate = System.DateTime.Now;
            order.Status = "Chưa giao";
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Đơn hàng đã được đặt!" });
        }

        // Customer xem lịch sử đơn hàng
        [Authorize(Roles = "Customer")]
        [HttpGet("history/{userId}")]
        public IActionResult GetOrderHistory(string userId)
        {
            var orders = _context.Orders.Where(o => o.UserId == userId).ToList();
            return Ok(orders);
        }

        // Admin cập nhật trạng thái đơn hàng
        [Authorize(Roles = "Admin")]
        [HttpPut("update-status/{orderId}/{status}")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, string status)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
                return NotFound();

            order.Status = status;
            await _context.SaveChangesAsync();
            return Ok(new { message = "Cập nhật trạng thái đơn hàng thành công!" });
        }
    }
}
