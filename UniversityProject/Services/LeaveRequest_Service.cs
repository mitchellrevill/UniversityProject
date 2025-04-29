using UniversityProject.Interfaces;
using UniversityProject.Model;
using UniversityProject.Repository;

public class LeaveRequestService : ILeaveRequestService
{
    // Singleton instance
    private static LeaveRequestService _instance;

    private static readonly object _lock = new object();


    private readonly LeaveRequestRepository _repository;

    private LeaveRequestService(string dbPath)
    {
        _repository = new LeaveRequestRepository(dbPath);
    }

   
    public static LeaveRequestService GetInstance(string dbPath)
    {
        if (_instance == null)
        {
            lock (_lock) // Ensure thread safety
            {
                if (_instance == null)
                {
                    _instance = new LeaveRequestService(dbPath);
                }
            }
        }
        return _instance;
    }


    public async Task<IEnumerable<LeaveRequest>> GetAllLeaveRequestsAsync()
    {
        return await Task.Run(() => _repository.GetAllLeaveRequests());
    }

    public async Task<LeaveRequest> GetLeaveRequestByEmployeeIdAsync(int employeeId)
    {
        return await Task.Run(() => _repository.GetLeaveRequestByEmployeeId(employeeId));
    }

    public async Task InsertLeaveRequestAsync(LeaveRequest leaveRequest)
    {
        await Task.Run(() => _repository.InsertLeaveRequest(leaveRequest));
    }

    public async Task UpdateLeaveRequestAsync(LeaveRequest leaveRequest)
    {
        await Task.Run(() => _repository.UpdateLeaveRequest(leaveRequest));
    }

    public async Task DeleteLeaveRequestAsync(int leaveRequestId)
    {
        await Task.Run(() => _repository.DeleteLeaveRequest(leaveRequestId));
    }

    public async Task ApproveLeaveAsync(int leaveRequestId)
    {
        await Task.Run(() => _repository.ApproveLeave(leaveRequestId));
    }

    public async Task RejectLeaveAsync(int employeeId)
    {
        await Task.Run(() => _repository.RejectLeave(employeeId));
    }
}
