using System.ComponentModel.DataAnnotations;

namespace Asm_C5.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Xác nhận mật khẩu không được để trống")]
        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Tên không được để trống")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Họ không được để trống")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Ngày sinh không được để trống")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Địa chỉ không được để trống")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Giới tính không được để trống")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Quốc tịch không được để trống")]
        public string Nationality { get; set; }

        [Required(ErrorMessage = "Vai trò không được để trống")]
        public string Role { get; set; } // Người dùng chọn "Admin" hoặc "User"
    }
}
