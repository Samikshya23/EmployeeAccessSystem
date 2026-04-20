using System;

namespace EmployeeAccessSystem.Models
{
    public class FortigateItem
    {
        public int FortigateItemId { get; set; }

        public int FortigateCategoryId { get; set; }
        public string? CategoryName { get; set; }

        public string? ItemName { get; set; }

        public bool IsActive { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }

        public DateTime? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }
    }
}