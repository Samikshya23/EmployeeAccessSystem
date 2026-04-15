using System.ComponentModel.DataAnnotations;

namespace EmployeeAccessSystem.Models
{
    public class PingProductItemValue
    {
        public int PingProductItemValueId { get; set; }

        [Required(ErrorMessage = "Please select Ping Product Item.")]
        public int PingProductItemId { get; set; }

        [Required(ErrorMessage = "Please select Ping Product Field.")]
        public int PingProductFieldId { get; set; }

        [Required(ErrorMessage = "Please enter field value.")]
        [StringLength(500, ErrorMessage = "Field value cannot exceed 500 characters.")]
        public string FieldValue { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public string FieldName { get; set; } = string.Empty;
        public string PingProductName { get; set; } = string.Empty;
        public string ItemName { get; set; } = string.Empty;
    }
}