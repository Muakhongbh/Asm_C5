using Microsoft.AspNetCore.Identity;

namespace Asm_C5.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } 
        public string LastName { get; set; } 
        public DateTime DateOfBirth { get; set; } 
        public string Address { get; set; } 
        public string Gender { get; set; } 
        public string Nationality { get; set; } 
        public string Role { get; set; } 
    }


}
