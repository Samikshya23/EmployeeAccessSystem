using System.ComponentModel.DataAnnotations;

namespace EmployeeAccessSystem.Models
{
    public class PingProductItem
    {
        public int PingProductItemId { get; set; }

        [Required(ErrorMessage = "Please select Ping Product.")]
        public int PingProductId { get; set; }

        [Required(ErrorMessage = "Please enter item name.")]
        [StringLength(100, ErrorMessage = "Item name cannot exceed 100 characters.")]
        public string ItemName { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public string PingProductName { get; set; } = string.Empty;
    }
}