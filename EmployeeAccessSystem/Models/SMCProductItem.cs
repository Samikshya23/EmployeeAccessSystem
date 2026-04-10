using System.ComponentModel.DataAnnotations;

namespace EmployeeAccessSystem.Models
{
    public class SMCProductItem
    {
        public int SMCProductItemId { get; set; }

        [Required(ErrorMessage = "Please select SMC Product.")]
        public int SMCProductId { get; set; }

        [Required(ErrorMessage = "Item Name is required.")]
        [StringLength(200, ErrorMessage = "Item Name cannot exceed 200 characters.")]
        public string ItemName { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public string? SMCProductName { get; set; }
    }
}