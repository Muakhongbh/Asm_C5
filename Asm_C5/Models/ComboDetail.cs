namespace Asm_C5.Models
{
    public class ComboDetail
    {
        public int Id { get; set; }
        public int ComboId { get; set; } 
        public int FoodItemId { get; set; } 
        public int Quantity { get; set; } 

        public Combo Combo { get; set; }
        public FoodItem FoodItem { get; set; }
    }
}
