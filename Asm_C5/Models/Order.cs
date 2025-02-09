namespace Asm_C5.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } 
        public List<OrderDetail> OrderDetails { get; set; }
    }
}
