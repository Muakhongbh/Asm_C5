using Asm_C5.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Asm_C5
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("def"));
            });
            builder.Services.AddIdentity<ApplicationUser,IdentityRole>(options =>
            {
                // cấu hình dành cho mật khẩu 
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;

                // cấu hình dảnh cho emial và đăng nhập 
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                //Thiết lập khoong cần dùng kí tự đặc biệt cho usename 
                //options.SignIn.RequireConfirmedEmail = true;

                // Cấu hình dành cho tài khoản khi đăng nhập
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(4);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
            }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            builder.Services.AddAuthentication().AddGoogle( options => {
                options.ClientId = "YOUR_GOOGLE_CLIENT_ID";
                options.ClientSecret = "YOUR_GOOGLE_CLIENT_SECRET";
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Food}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
