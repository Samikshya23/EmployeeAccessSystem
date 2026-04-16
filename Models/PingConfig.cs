using System;
using System.ComponentModel.DataAnnotations;

namespace EmployeeAccessSystem.Models
{
    public class PingConfig
    {
        public int PingConfigId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select a product.")]
        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Please select server host name.")]
        public int PingProductId { get; set; }

        [Required(ErrorMessage = "Please select IP address.")]
        public string IPAddress { get; set; } = string.Empty;

        public string ServerHostName { get; set; } = string.Empty;

        public DateTime EntryDate { get; set; }

        [StringLength(200, ErrorMessage = "Value cannot exceed 200 characters.")]
        public string? ConfigValue { get; set; }

        [StringLength(500, ErrorMessage = "Remarks cannot exceed 500 characters.")]
        public string? Remarks { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsChecked { get; set; }

        [Required(ErrorMessage = "Please select save option.")]
        [StringLength(50)]
        public string? EntryMode { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }

        public DateTime? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }
    }
}