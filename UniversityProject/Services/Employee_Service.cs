using UniversityProject.Model;
using UniversityProject.Repository;
using UniversityProject.Interfaces;

public class Employee_Service : IEmployeeService
{
    private readonly EmployeeSQL _EmployeeSQL;

    public Employee_Service(string dbPath)
    {
        _EmployeeSQL = new EmployeeSQL(dbPath);
    }

    public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
    {
        return await Task.Run(() => _EmployeeSQL.GetAllEmployees());
    }

    public async Task InsertEmployeeAsync(Employee employee)
    {
        await Task.Run(() => _EmployeeSQL.InsertEmployee(employee));
    }

    public async Task UpdateEmployeeAsync(Employee employee)
    {

        await Task.Run(() => _EmployeeSQL.UpdateEmployee(employee));
    }

    public async Task DeleteEmployeeAsync(string employeeId)
    {
        await Task.Run(() => _EmployeeSQL.DeleteEmployee(employeeId));
    }
}
