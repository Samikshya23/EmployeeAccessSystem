using System.ComponentModel.DataAnnotations;

namespace EmployeeAccessSystem.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Category Name is required")]
        public string CategoryName { get; set; } = "";

        public bool IsActive { get; set; }
    }
}
