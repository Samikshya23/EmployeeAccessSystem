using System.ComponentModel.DataAnnotations;

namespace EmployeeAccessSystem.Models
{
    public class SMCProduct
    {
        public int SMCProductId { get; set; }

        [Required(ErrorMessage = "Please select a product.")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Please enter SMC product name.")]
        [StringLength(100, ErrorMessage = "SMC product name cannot exceed 100 characters.")]
        public string SMCProductName { get; set; } = string.Empty;

        public bool IsActive { get; set; }
    }
}