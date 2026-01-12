using System.ComponentModel.DataAnnotations;

namespace EmployeeAccessSystem.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Full name is required")]
        [MinLength(3, ErrorMessage = "Full name must be at least 3 characters")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email format is invalid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Department is required")]
        public string Department { get; set; }
    }
}
