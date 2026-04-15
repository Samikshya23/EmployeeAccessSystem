using System.ComponentModel.DataAnnotations;

namespace EmployeeAccessSystem.Models
{
    public class PingProduct
    {
        public int PingProductId { get; set; }

        [Required(ErrorMessage = "Please select a product.")]
        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter Ping product name.")]
        [StringLength(100, ErrorMessage = "Ping product name cannot exceed 100 characters.")]
        public string PingProductName { get; set; } = string.Empty;

        public bool IsActive { get; set; }
    }
}