using System.ComponentModel.DataAnnotations;

namespace EmployeeAccessSystem.Models
{
    public class RegisterModel
    {
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6,
      ErrorMessage = "Password must be at least 6 characters long")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z]).+$",
      ErrorMessage = "Password must contain 1 uppercase and 1 lowercase letter")]
        public string Password { get; set; } = "";
        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } = "";
    }
}
