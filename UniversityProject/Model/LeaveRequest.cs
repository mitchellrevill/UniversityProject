namespace UniversityProject.Model
{
    public class LeaveRequest
    {
        public int LeaveRequestId { get; set; }
        public string? EmployeeId { get; set; }  
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int HoursUsed { get; set; }
        public string IsApproved { get; set; }

    }
}
