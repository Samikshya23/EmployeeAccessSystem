using System.ComponentModel.DataAnnotations;

namespace EmployeeAccessSystem.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Full Name is required")]
        public string FullName { get; set; } = "";

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter a valid email address")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Department is required")]
        public int DepartmentId { get; set; }

        public string? DepartmentName { get; set; }

        // ✅ ADD THIS
        public int AccountId { get; set; }
        public bool IsActive { get; set; }

    }
}
