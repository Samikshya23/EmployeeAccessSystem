using System.ComponentModel.DataAnnotations;

namespace EmployeeAccessSystem.Models
{
    public class PingProductField
    {
        public int PingProductFieldId { get; set; }

        [Required(ErrorMessage = "Please select Ping Product.")]
        public int PingProductId { get; set; }

        [Required(ErrorMessage = "Please enter field name.")]
        [StringLength(100, ErrorMessage = "Field name cannot exceed 100 characters.")]
        public string FieldName { get; set; } = string.Empty;

        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; }

        public string PingProductName { get; set; } = string.Empty;
    }
}