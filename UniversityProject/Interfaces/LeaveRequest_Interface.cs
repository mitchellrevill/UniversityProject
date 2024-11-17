using UniversityProject.Model;

namespace UniversityProject.Interfaces
{
    public interface ILeaveRequestService
    {
        Task<IEnumerable<LeaveRequest>> GetAllLeaveRequestsAsync();
        Task<LeaveRequest> GetLeaveRequestByEmployeeIdAsync(int employeeId);
        Task InsertLeaveRequestAsync(LeaveRequest leaveRequest);
        Task UpdateLeaveRequestAsync(LeaveRequest leaveRequest);
        Task DeleteLeaveRequestAsync(int leaveRequestId);
        Task ApproveLeaveAsync(int leaveRequestId);
        Task RejectLeaveAsync(int leaveRequestId);
    }
}
