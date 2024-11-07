using UniversityProject.Model;
using UniversityProject.Repository;
using UniversityProject.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

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

    public async Task<AuthResponse> AuthAsync(Employee employee)
    {
        try
        {
           
            Console.WriteLine($"Received employee ID: {employee.EmployeeId}");
            Console.WriteLine($"Received employee password: {employee.password}");

           
            Employee data = await GetEmployeeByIdAsync(employee.EmployeeId);

            if (data == null)
            {
                Console.WriteLine("No employee data found.");
                return new AuthResponse { Message = "Employee Doesn't Exist" };
            }

            Console.WriteLine($"Fetched employee: {data.EmployeeId}, Password: {data.password}, Role: {data.Employeetype}");

          
            if (data.password == employee.password)
            {
                Console.WriteLine("Password match successful");

                
                string role = data.Employeetype == "Admin" ? "Admin" : "User";
                Console.WriteLine($"Assigned role: {role}");

               
                var token = GenerateJwtToken(data.EmployeeId, role, data.Employeetype);
                Console.WriteLine($"Generated token: {token}");

                return new AuthResponse { Token = token, Role = role, Message = "Success" };
            }
            else
            {
                Console.WriteLine("Password mismatch");
                return new AuthResponse { Message = "User or Password Incorrect" };
            }
        }
        catch (Exception ex)
        {
           
            Console.WriteLine($"Exception occurred: {ex.Message}");
            return new AuthResponse { Message = "Employee Doesn't Exist" };
        }
    }


    private string GenerateJwtToken(string employeeId, string role, string employeeType)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourVerySecureKeyEvenThisWasntLongEnoughBlyat123456789"));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
        new Claim(JwtRegisteredClaimNames.Sub, employeeId),
        new Claim(ClaimTypes.Role, role), 
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), 
        new Claim("EmployeeType", employeeType), 
        new Claim("EmployeeId", employeeId) 
    };

        var token = new JwtSecurityToken(
            issuer: "mitchellrevill",
            audience: "AccessAPI",
            claims: claims,
            expires: DateTime.Now.AddHours(12), 
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }


}

