using System;

namespace EmployeeAccessSystem.Models
{
    public class FortigateCategory
    {
        public int FortigateCategoryId { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? CategoryName { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }

        public DateTime? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }
    }
}