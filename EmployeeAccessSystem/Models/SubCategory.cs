using System.ComponentModel.DataAnnotations;

namespace EmployeeAccessSystem.Models
{
    public class SubCategory
    {
        public int SubCategoryId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public string ServerIP { get; set; } = "";

        public string? ServerName { get; set; }

        public string? CategoryName { get; set; }
    }
}
