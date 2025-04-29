namespace UniversityProject.Model
{



    public class Employee
    {

        // EMPLOYEE WILL NEED LEAVE HOURS INCLUDED 
        public string EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyEmail { get; set; }
        public string PersonalEmail { get; set; }
        public string PhoneNumber { get; set; }
        public int CountryId { get; set; }
        public int DepartmentId { get; set; }
        public int? ManagerId { get; set; }
        public int RegionId { get; set; }
        public string EmploymentType { get; set; }
        public DateTime StartDate { get; set; }
        public decimal Salary { get; set; }
        public string Benefits { get; set; }
        public string password { get; set; }
        public string Employeetype { get; set; }
        public EmployeeStatus Status { get; set; }

        public enum EmployeeStatus
        {
            FullTime,
            PartTime,
            Contract
        }
    }
}
