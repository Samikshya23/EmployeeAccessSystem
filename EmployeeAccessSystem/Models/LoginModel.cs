using System.ComponentModel.DataAnnotations;

namespace EmployeeAccessSystem.Models
{
    public class LoginModel
    {
        public string Email { get; set; } = "";

        
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = "";
    }
}
