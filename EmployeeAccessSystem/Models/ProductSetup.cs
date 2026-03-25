using System.ComponentModel.DataAnnotations;

namespace EmployeeAccessSystem.Models
{
    public class ProductSetup
    {
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product Name is required")]
        public string ProductName { get; set; } = "";

        public bool IsActive { get; set; }
    }
}