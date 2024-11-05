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
    public async Task<Employee> GetEmployeeByIdAsync(string postingId)
    {
        return await Task.Run(() => _EmployeeSQL.GetEmployeeById(postingId));
    }

    public async Task<string> AuthAsync(Employee employee)
    {
        try
        {
            Employee data = await GetEmployeeByIdAsync(employee.EmployeeId);

            if (data != null && data.password == employee.password && data.Employeetype == "Admin")
            {
                var access = "Admin";
                return access; 
            }
            else if (data != null && data.password == employee.password && data.Employeetype == "User")
            {
                var access = "User";
                return access;
            }
            else
            {
                return "User or Password Incorrect";
            }
        }
        catch (Exception ex)
        {
            return "Employee Doesn't Exist";
        }
    }
}

