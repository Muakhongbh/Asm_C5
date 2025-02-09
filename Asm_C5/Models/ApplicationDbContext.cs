using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Asm_C5.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<FoodItem> FoodItems { get; set; }
        public DbSet<Combo> Combos { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }    
        public DbSet<ComboDetail> ComboDetails { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); 

            // định nghĩa các khóa chính , khóa ngoại và các mối quan hệ giữa các bảng 
            builder.Entity<OrderDetail>().HasKey(od => od.Id);

            builder.Entity<OrderDetail>().HasOne(od => od.Order)
                                         .WithMany(o => o.OrderDetails)
                                         .HasForeignKey(od => od.OrderId);
            // bảng order chi tiết có mối quan hệ 1 với bẳng order và banangrv order có mối quan hệ 1 nhiều với bảng order chi tiết và có khóa ngoại là orderId
            builder.Entity<OrderDetail>().HasOne(od => od.FoodItem)
                                         .WithMany()
                                         .HasForeignKey(od => od.FoodItemId);
            builder.Entity<Order>().HasOne(o => o.User)
                                   .WithMany()
                                   .HasForeignKey(o => o.UserId);
            builder.Entity<ComboDetail>().HasOne(cb => cb.Combo)
                                         .WithMany(c => c.ComboDetails)
                                         .HasForeignKey(cb => cb.ComboId);
            builder.Entity<ComboDetail>().HasOne(cb => cb.FoodItem)
                                         .WithMany()
                                         .HasForeignKey(cb => cb.FoodItemId);

        }
    }
}
