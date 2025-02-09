namespace Asm_C5.Models
{
    public class Combo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ImagePath { get; set; } 
        public int Quantity { get; set; } 
        public List<ComboDetail> ComboDetails { get; set; }
    }
}
