using System;
using System.ComponentModel.DataAnnotations;

namespace EmployeeAccessSystem.Models
{
    public class PingConfig
    {
        public int PingConfigId { get; set; }

        [Required(ErrorMessage = "Product is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select Product.")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Ping Product is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select Ping Product.")]
        public int PingProductId { get; set; }

        [Required(ErrorMessage = "Ping Product Item is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select Ping Product Item.")]
        public int PingProductItemId { get; set; }

        public DateTime EntryDate { get; set; }

        [StringLength(100, ErrorMessage = "Config Value cannot exceed 100 characters.")]
        public string? ConfigValue { get; set; }

        public bool IsChecked { get; set; }

        [Required(ErrorMessage = "Save Option is required.")]
        [StringLength(20)]
        public string? EntryMode { get; set; }

        [StringLength(250, ErrorMessage = "Remarks cannot exceed 250 characters.")]
        public string? Remarks { get; set; }

        public bool IsActive { get; set; }

        public string? ProductName { get; set; }
        public string? PingProductName { get; set; }
        public string? ItemName { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }
    }
}