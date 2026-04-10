using System;
using System.ComponentModel.DataAnnotations;

namespace EmployeeAccessSystem.Models
{
    public class SMCConfig
    {
        public int SMCConfigId { get; set; }

        [Required(ErrorMessage = "Product is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select Product.")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "SMC Product is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select SMC Product.")]
        public int SMCProductId { get; set; }

        [Required(ErrorMessage = "SMC Product Item is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select SMC Product Item.")]
        public int SMCProductItemId { get; set; }

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
        public string? SMCProductName { get; set; }
        public string? ItemName { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }
    }
}