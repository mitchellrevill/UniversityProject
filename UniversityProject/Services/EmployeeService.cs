using UniversityProject.Model;
using UniversityProject.Repository;
using UniversityProject.Interfaces;

public class EmployeeService : IEmployeeService
{
    private readonly EmployeeDatabase _employeeDatabase;

    public EmployeeService(string dbPath)
    {
        _employeeDatabase = new EmployeeDatabase(dbPath);
    }

    public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
    {
        return await Task.Run(() => _employeeDatabase.GetAllEmployees());
    }

    public async Task InsertEmployeeAsync(Employee employee)
    {
        await Task.Run(() => _employeeDatabase.InsertEmployee(employee));
    }

    public async Task UpdateEmployeeAsync(Employee employee)
    {
        // Ensure you have this method implemented in EmployeeDatabase
        await Task.Run(() => _employeeDatabase.UpdateEmployee(employee));
    }

    public async Task DeleteEmployeeAsync(string employeeId)
    {
        await Task.Run(() => _employeeDatabase.DeleteEmployee(employeeId));
    }
}
