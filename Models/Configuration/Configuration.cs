using System;

namespace EmployeeAccessSystem.Models
{
    public class Configuration
    {
        public int ConfigurationId { get; set; }
        public int ProductId { get; set; }
        public int? ParentConfigurationId { get; set; }
        public string ConfigurationName { get; set; }
        public string NodeType { get; set; }
        public string InputType { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

        public DateTime? DeletedDate { get; set; }
        public string DeletedBy { get; set; }

        public string ProductName { get; set; }
        public string ParentConfigurationName { get; set; }
    }
}