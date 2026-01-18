namespace EmployeeAccessSystem.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public int DepartmentId { get; set; }

        // Only for display (JOIN)
        public string DepartmentName { get; set; }
    }
}
