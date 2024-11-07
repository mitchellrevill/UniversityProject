
using UniversityProject.Model;

namespace UniversityProject.Interfaces
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Employee>> GetAllEmployeesAsync();
        Task InsertEmployeeAsync(Employee employee);
        Task UpdateEmployeeAsync(Employee employee);
        Task DeleteEmployeeAsync(string employeeId);
        Task<Employee> GetEmployeeByIdAsync(string postingId);
        Task<AuthResponse> AuthAsync(Employee employee);
    }

}