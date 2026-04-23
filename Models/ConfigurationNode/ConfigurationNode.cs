using System;
using System.Collections.Generic;

namespace EmployeeAccessSystem.Models
{
    public class ConfigurationNode
    {
        public int NodeId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int? ParentNodeId { get; set; }
        public string ParentNodeName { get; set; }
        public string NodeName { get; set; }
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

        public List<ConfigurationNode> Children { get; set; }

        public ConfigurationNode()
        {
            Children = new List<ConfigurationNode>();
        }
    }
}