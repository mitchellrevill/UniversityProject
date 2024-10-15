using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityProject.Model
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Location { get; set; }
        public string Department { get; set; }
        public EmployeeStatus Status { get; set; }

        public enum EmployeeStatus
        {
            FullTime,
            PartTime,
            Contract
        }
    }
}
