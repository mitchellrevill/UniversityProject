using System.Collections.Specialized;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using HRSystem;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using UniversityProject.Interfaces;
using UniversityProject.Model;

public static class MethodHandle
{
    private static readonly string dbPath = Path.Combine(HttpServer.ResourcesDirectory, "database.db");

    // Adjust all services to use Singleton `GetInstance()` if applicable
    private static readonly Employee_Service employeeService = new Employee_Service(dbPath);
    private static readonly DepartmentService departmentService = DepartmentService.GetInstance(dbPath);
    private static readonly JobPostingsService jobPostingsService = JobPostingsService.GetInstance(dbPath);
    private static readonly ManagerService managerService = ManagerService.GetInstance(dbPath);
    private static readonly CountryService countryService = CountryService.GetInstance(dbPath);
    private static readonly RegionService regionService = RegionService.GetInstance(dbPath);
    private static readonly ApplicantService applicantService = ApplicantService.GetInstance(dbPath);
    private static readonly LocationService locationService = LocationService.GetInstance(dbPath);
    private static readonly LeaveRequestService leaveRequestService = LeaveRequestService.GetInstance(dbPath);
    private static readonly PayrollService payrollService = PayrollService.GetInstance(dbPath);



    // Get requests
    private static readonly Dictionary<string, Func<HttpListenerRequest, HttpListenerResponse, Task>> _getRoutes =
        new Dictionary<string, Func<HttpListenerRequest, HttpListenerResponse, Task>>
        {
        { "GetAllEmployees", GetEmployees },
        { "GetAllJobPostings", GetAllJobPostings },
        { "GetAllApplications", GetAllApplications},
        { "GetAllDepartments",GetDepartments},
        { "GetCountries", GetCountries },
        { "GetAllRegions", GetAllRegions },
        { "GetManagers",  GetManagers },
        { "GetLocations", GetLocations },
        { "GetLeaveRequest", GetLeaveRequest }
        };
    // Post requests
    private static readonly Dictionary<string, Func<HttpListenerRequest, HttpListenerResponse, Task>> _postRoutes =
        new Dictionary<string, Func<HttpListenerRequest, HttpListenerResponse, Task>>
        {
        { "insertEmployee", InsertEmployee },
        { "UpdateEmployees", UpdateEmployee },
        { "DeleteEmployee", DeleteEmployee },
        { "GetEmployeeById", GetEmployeeById },
        { "InsertJobPosting", InsertJobPosting },
        { "UpdateJobPosting", UpdateJobPosting },
        { "DeleteJobPosting", DeleteJobPosting },
        { "InsertApplication", InsertApplication },
        { "UpdateApplication", UpdateApplication },
        { "DeleteApplication", DeleteApplication },
        { "InsertDepartment", InsertDepartment},
        { "UpdateDepartment", UpdateDepartment},
        { "DeleteDepartment" , DeleteDepartment},
        { "InsertRegion" , InsertRegion},
        { "UpdateRegion", UpdateRegion },
        { "DeleteRegion", DeleteRegion },
        { "InsertCountry", InsertCountry },
        { "UpdateCountry", UpdateCountry },
        { "DeleteCountry", DeleteCountry },
        { "InsertManager", InsertManager },
        { "UpdateManager", UpdateManager },
        { "DeleteManager", DeleteManager },
        { "InsertLocation", InsertLocation },
        { "UpdateLocation", UpdateLocation },
        { "DeleteLocation", DeleteLocation },
        { "GetJobPostingById", GetJobPostingById },
        { "Authenticate", Authenticate },
        { "InsertLeaveRequest", InsertLeaveRequest },
        { "GetAllPayrollsById", GetAllPayrollsById },
        { "GetLocationById", GetLocationById }
        };


    public static async Task HandleRequest(HttpListenerRequest req, HttpListenerResponse resp)
    {
        string requestedPath = req.Url.AbsolutePath.TrimStart('/');

        if (req.HttpMethod == "POST" && _postRoutes.TryGetValue(requestedPath, out var postHandler))
        {
            await postHandler(req, resp);
        }
        else if (req.HttpMethod == "GET" && _getRoutes.TryGetValue(requestedPath, out var getHandler))
        {
            await getHandler(req, resp);
            Console.WriteLine("Entered GET Handler");
        }
        else
        {
            await ServeStaticFile(req, resp, requestedPath);
        }
    }

   
    public class JwtAuthStrategy : IAuthStrategy
    {
        private readonly TokenValidationParameters _validationParams;

        public JwtAuthStrategy(string issuer, string audience, byte[] signingKey)
        {
            _validationParams = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(signingKey)
            };
        }

        public bool Authenticate(HttpListenerRequest req, out ClaimsPrincipal claimsPrincipal)
        {
            claimsPrincipal = null;
            var authHeader = req.Headers["Authorization"];
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                return false;

            var token = authHeader.Substring("Bearer ".Length).Trim();
            try
            {
                var handler = new JwtSecurityTokenHandler();
                claimsPrincipal = handler.ValidateToken(token, _validationParams, out _);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
    public class ApiKeyAuthStrategy : IAuthStrategy
    {
        private const string HEADER = "X-Api-Key";
        private readonly HashSet<string> _validKeys;

        public ApiKeyAuthStrategy(IEnumerable<string> validKeys)
            => _validKeys = new HashSet<string>(validKeys);

        public bool Authenticate(HttpListenerRequest req, out ClaimsPrincipal claimsPrincipal)
        {
            claimsPrincipal = null;
            var key = req.Headers[HEADER];
            if (string.IsNullOrEmpty(key) || !_validKeys.Contains(key))
                return false;

            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, "api-client"), new Claim(ClaimTypes.Role, "ApiUser") };
            claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "ApiKey"));
            return true;
        }
    }


    // Auth
    public static bool TokenStatus(HttpListenerRequest req)
    {
        string authHeader = req.Headers["Authorization"];


        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {

            string token = authHeader.Substring("Bearer ".Length).Trim();


            ClaimsPrincipal claimsPrincipal;
            return ValidateTokenAndGetClaims(token, out claimsPrincipal);
        }


        return false;
    }
    // MARK
    private static bool ValidateTokenAndGetClaims(HttpListenerRequest req, out ClaimsPrincipal claimsPrincipal)
    {
        claimsPrincipal = null;
        foreach (var strat in AuthRegistry.Strategies)
        {
            if (strat.Authenticate(req, out claimsPrincipal))
                return true;
        }
        return false;
    }


    public static bool ValidateTokenAndGetClaims(string token, out ClaimsPrincipal claimsPrincipal)
    {
        claimsPrincipal = null;
        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes("YourVerySecureKeyEvenThisWasntLongEnoughBlyat123456789"));

        try
        {
            var handler = new JwtSecurityTokenHandler();
            var principal = handler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,           
                IssuerSigningKey = securityKey,

                ValidateIssuer = true,
                ValidIssuer = "mitchellrevill",

                ValidateAudience = true,
                ValidAudience = "AccessAPI",

                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero                  
            }, out SecurityToken validatedToken);

            claimsPrincipal = principal;
            return true;
        }
        catch (SecurityTokenExpiredException ex)
        {
            Console.WriteLine($"Token expired: {ex.Message}");
            return false;
        }
        catch (SecurityTokenSignatureKeyNotFoundException ex)
        {
            Console.WriteLine($"Signature key problem: {ex.Message}");
            return false;
        }
        catch (SecurityTokenInvalidIssuerException ex)
        {
            Console.WriteLine($"Invalid issuer: {ex.Message}");
            return false;
        }
        catch (SecurityTokenInvalidAudienceException ex)
        {
            Console.WriteLine($"Invalid audience: {ex.Message}");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Other validation failure: {ex.GetType().Name}: {ex.Message}");
            return false;
        }
    }



    private static bool HasValidRole(ClaimsPrincipal claimsPrincipal, params string[] allowedRoles)
    {
        var userRole = claimsPrincipal?.FindFirst(ClaimTypes.Role)?.Value;

        return allowedRoles.Contains(userRole);
    }

    private static async Task Authenticate(HttpListenerRequest req, HttpListenerResponse resp)
    {
        try
        {
            GeneratePayrollAsync();
            Console.WriteLine("Here");
            var newEmployee = await ReadRequestBodyAsync<Employee>(req);
            var Authresp = await employeeService.AuthAsync(newEmployee);
            var jsonResponse = JsonConvert.SerializeObject(Authresp);
            Console.WriteLine(jsonResponse);
            await SendResponse(resp, jsonResponse, "application/json");
        }
        catch (Exception ex)
        {
            await HandleError(resp, ex);
        }
    }
    // Auth


    public static async Task GeneratePayrollAsync()
    {

        var employees = await employeeService.GetAllEmployeesAsync();

        foreach (Employee employee in employees)
        {
            var existingPayrolls = await payrollService.GetAllPayrollsByIdAsync(employee.EmployeeId);

            Payroll mostRecentPayroll = existingPayrolls
                .OrderByDescending(p => p.PaymentDate)
                .FirstOrDefault();

            if (mostRecentPayroll != null)
            {

                if (mostRecentPayroll.PaymentDate.AddDays(28) < DateTime.Today)
                {
                    Console.WriteLine($"Inserting new payroll for EmployeeId: {employee.EmployeeId}");
                    await payrollService.InsertPayrollAsync(new Payroll
                    {
                        EmployeeId = employee.EmployeeId,
                        TaxNumberCode = "1257l",
                        BaseSalary = employee.Salary,
                        ThisPaycheck = CalculatePay(employee),
                        Recurrence = (365 / 12).ToString(),
                        Tax = 2,
                        ExtraDeductions = 0,
                        PayPeriodStart = employee.StartDate,
                        PayPeriodEnd = employee.StartDate.AddDays(28),
                        NetPay = CalculatePay(employee),
                        Bonuses = 0,
                        OvertimeHours = 0,
                        OvertimePay = 0,
                        PaymentDate = DateTime.Today,
                        PaymentMethod = "Credit",
                        Deductions = new List<string>()
                    });
                }
            }
            else
            {
                // No payroll exists for this employee, insert the first payroll
                Console.WriteLine($"Inserting initial payroll for EmployeeId: {employee.EmployeeId}");
                await payrollService.InsertPayrollAsync(new Payroll
                {
                    EmployeeId = employee.EmployeeId,
                    TaxNumberCode = "1257l",
                    BaseSalary = employee.Salary,
                    ThisPaycheck = CalculatePay(employee),
                    Recurrence = (365 / 12).ToString(),
                    Tax = 2,
                    ExtraDeductions = 0,
                    PayPeriodStart = employee.StartDate,
                    PayPeriodEnd = employee.StartDate.AddDays(28),
                    NetPay = CalculatePay(employee),
                    Bonuses = 0,
                    OvertimeHours = 0,
                    OvertimePay = 0,
                    PaymentDate = DateTime.Today,
                    PaymentMethod = "Credit",
                    Deductions = new List<string>()
                });
            }
        }
    }

    private static decimal CalculatePay(Employee employee)
    {

        return employee.Salary / 12;
    }



    private static async Task<T> ReadRequestBodyAsync<T>(HttpListenerRequest req) // Repeated code i replaced, just takes the HTTP content and converts it, efficiency???? lmao
    {

        using var reader = new StreamReader(req.InputStream, req.ContentEncoding);
        string json = await reader.ReadToEndAsync();
        return JsonConvert.DeserializeObject<T>(json);
    }

    private static async Task SendResponse(HttpListenerResponse resp, string message, string contentType = "text/plain") // same deal as ReadRequestBody
    {
        byte[] data = Encoding.UTF8.GetBytes(message);
        resp.ContentType = contentType;
        resp.ContentLength64 = data.Length;
        await resp.OutputStream.WriteAsync(data, 0, data.Length);
    }

    private static async Task HandleError(HttpListenerResponse resp, Exception ex) // error checking 
    {
        resp.StatusCode = (int)HttpStatusCode.InternalServerError;
        await SendResponse(resp, ex.Message);
    }



    private static async Task GetLeaveRequest(HttpListenerRequest req, HttpListenerResponse resp)
    {
        try
        {

                if (!ValidateTokenAndGetClaims(req, out var claimsPrincipal))
                {
                    resp.StatusCode = 401; // Unauthorized
                    await SendResponse(resp, "{\"error\":\"Unauthorized: Invalid token.\"}", "application/json");
                    return;
                }

                if (!HasValidRole(claimsPrincipal, "Admin", "User"))
                {
                    resp.StatusCode = 403; // Forbidden
                    await SendResponse(resp, "{\"error\":\"Forbidden: Insufficient permissions.\"}", "application/json");
                    return;
                }

            var employees = await leaveRequestService.GetAllLeaveRequestsAsync();
            string jsonResponse = JsonConvert.SerializeObject(employees);
            await SendResponse(resp, jsonResponse, "application/json");
        }
        catch (Exception ex)
        {
            await HandleError(resp, ex);
        }
    }

    private static async Task InsertLeaveRequest(HttpListenerRequest req, HttpListenerResponse resp)
    {
        try
        {
            Console.WriteLine("Entered Insert Leave Request");
            if (!ValidateTokenAndGetClaims(req, out var claimsPrincipal))
            {
                resp.StatusCode = 401; // Unauthorized
                await SendResponse(resp, "{\"error\":\"Unauthorized: Invalid token.\"}", "application/json");
                return;
            }

            if (!HasValidRole(claimsPrincipal, "Admin", "User"))
            {
                resp.StatusCode = 403; // Forbidden
                await SendResponse(resp, "{\"error\":\"Forbidden: Insufficient permissions.\"}", "application/json");
                return;
            }

            var newLeaveRequest = await ReadRequestBodyAsync<LeaveRequest>(req);
            Console.WriteLine("HandlerFailure");
            await leaveRequestService.InsertLeaveRequestAsync(newLeaveRequest);
            await SendResponse(resp, "LeaveRequest inserted successfully.");
        }
        catch (Exception ex)
        {
            await HandleError(resp, ex);
        }
    }

    // EMPLOYEE
    public static async Task GetEmployees(HttpListenerRequest req, HttpListenerResponse resp)
    {
        try
        {

            if (!ValidateTokenAndGetClaims(req, out var claimsPrincipal))
            {
                resp.StatusCode = 401; // Unauthorized
                await SendResponse(resp, "{\"error\":\"Unauthorized: Invalid token.\"}", "application/json");
                return;
            }

            if (!HasValidRole(claimsPrincipal, "Admin", "User"))
            {
                resp.StatusCode = 403; // Forbidden
                await SendResponse(resp, "{\"error\":\"Forbidden: Insufficient permissions.\"}", "application/json");
                return;
            }

            var employees = await employeeService.GetAllEmployeesAsync();
            string jsonResponse = JsonConvert.SerializeObject(employees);
            await SendResponse(resp, jsonResponse, "application/json");
        }
        catch (Exception ex)
        {
            await HandleError(resp, ex);
        }
    }

    public static async Task InsertEmployee(HttpListenerRequest req, HttpListenerResponse resp)
    {
        try
        {
            if (!ValidateTokenAndGetClaims(req, out var claimsPrincipal))
            {
                resp.StatusCode = 401; // Unauthorized
                await SendResponse(resp, "{\"error\":\"Unauthorized: Invalid token.\"}", "application/json");
                return;
            }

            if (!HasValidRole(claimsPrincipal, "Admin", "User"))
            {
                resp.StatusCode = 403; // Forbidden
                await SendResponse(resp, "{\"error\":\"Forbidden: Insufficient permissions.\"}", "application/json");
                return;
            }

            var newEmployee = await ReadRequestBodyAsync<Employee>(req);
            Console.WriteLine(newEmployee.ManagerId);
            await employeeService.InsertEmployeeAsync(newEmployee);
            await SendResponse(resp, "Employee inserted successfully.");
        }
        catch (Exception ex)
        {
            await HandleError(resp, ex);
        }
    }

    private static async Task UpdateEmployee(HttpListenerRequest req, HttpListenerResponse resp)
    {
        try
        {
            if (!ValidateTokenAndGetClaims(req, out var claimsPrincipal))
            {
                resp.StatusCode = 401; // Unauthorized
                await SendResponse(resp, "{\"error\":\"Unauthorized: Invalid token.\"}", "application/json");
                return;
            }

            if (!HasValidRole(claimsPrincipal, "Admin", "User"))
            {
                resp.StatusCode = 403; // Forbidden
                await SendResponse(resp, "{\"error\":\"Forbidden: Insufficient permissions.\"}", "application/json");
                return;
            }
            var newEmployee = await ReadRequestBodyAsync<Employee>(req);
            await employeeService.UpdateEmployeeAsync(newEmployee);
            await SendResponse(resp, "Employee inserted successfully.");
        }
        catch (Exception ex)
        {
            await HandleError(resp, ex);
        }
    }

    private static async Task DeleteEmployee(HttpListenerRequest req, HttpListenerResponse resp)
    {
        try
        {
            if (!ValidateTokenAndGetClaims(req, out var claimsPrincipal))
            {
                resp.StatusCode = 401; // Unauthorized
                await SendResponse(resp, "{\"error\":\"Unauthorized: Invalid token.\"}", "application/json");
                return;
            }

            if (!HasValidRole(claimsPrincipal, "Admin", "User"))
            {
                resp.StatusCode = 403; // Forbidden
                await SendResponse(resp, "{\"error\":\"Forbidden: Insufficient permissions.\"}", "application/json");
                return;
            }

            var newEmployee = await ReadRequestBodyAsync<Employee>(req);
            await employeeService.DeleteEmployeeAsync(newEmployee.EmployeeId);
            await SendResponse(resp, "Employee inserted successfully.");
        }
        catch (Exception ex)
        {
            await HandleError(resp, ex);
        }
    }


    private static async Task GetEmployeeById(HttpListenerRequest req, HttpListenerResponse resp)
    {

        try
        {
            if (!ValidateTokenAndGetClaims(req, out var claimsPrincipal))
            {
                resp.StatusCode = 401; // Unauthorized
                await SendResponse(resp, "{\"error\":\"Unauthorized: Invalid token.\"}", "application/json");
                return;
            }

            if (!HasValidRole(claimsPrincipal, "Admin", "User"))
            {
                resp.StatusCode = 403; // Forbidden
                await SendResponse(resp, "{\"error\":\"Forbidden: Insufficient permissions.\"}", "application/json");
                return;
            }
            Console.WriteLine("Entered try part 1");
            var Employee = await ReadRequestBodyAsync<Employee>(req);
            var EmployeeObject = await employeeService.GetEmployeeByIdAsync(Employee.EmployeeId);
            string jsonResponse = JsonConvert.SerializeObject(EmployeeObject);
            await SendResponse(resp, jsonResponse, "application/json");
            Console.WriteLine("Exited try part 1");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed part 1");
            await HandleError(resp, ex);
        }
    }

    // JOB POSTING
    private static async Task GetAllJobPostings(HttpListenerRequest req, HttpListenerResponse resp)
    {
        Console.WriteLine("Method start");
        try
        {
            if (!ValidateTokenAndGetClaims(req, out var claimsPrincipal))
            {
                resp.StatusCode = 401; // Unauthorized
                await SendResponse(resp, "{\"error\":\"Unauthorized: Invalid token.\"}", "application/json");
                return;
            }

            if (!HasValidRole(claimsPrincipal, "Admin", "User"))
            {
                resp.StatusCode = 403; // Forbidden
                await SendResponse(resp, "{\"error\":\"Forbidden: Insufficient permissions.\"}", "application/json");
                return;
            }

            Console.WriteLine("Entered method");
            var JobPostings = await jobPostingsService.GetAllJobPostingsAsync();
            string jsonResponse = JsonConvert.SerializeObject(JobPostings);
            await SendResponse(resp, jsonResponse, "application/json");
            Console.WriteLine("Method finished");
        }
        catch (Exception ex)
        {
            await HandleError(resp, ex);
        }
    }
    private static async Task UpdateJobPosting(HttpListenerRequest req, HttpListenerResponse resp)
    {
        try
        {
            if (!ValidateTokenAndGetClaims(req, out var claimsPrincipal))
            {
                resp.StatusCode = 401; // Unauthorized
                await SendResponse(resp, "{\"error\":\"Unauthorized: Invalid token.\"}", "application/json");
                return;
            }

            if (!HasValidRole(claimsPrincipal, "Admin", "User"))
            {
                resp.StatusCode = 403; // Forbidden
                await SendResponse(resp, "{\"error\":\"Forbidden: Insufficient permissions.\"}", "application/json");
                return;
            }

            var JobPostings = await ReadRequestBodyAsync<JobPostings>(req);
            await jobPostingsService.UpdateJobPostingsAsync(JobPostings);
            await SendResponse(resp, "Employee inserted successfully.");
        }
        catch (Exception ex)
        {
            await HandleError(resp, ex);
        }
    }

    private static async Task DeleteJobPosting(HttpListenerRequest req, HttpListenerResponse resp)
    {
        try
        {
            if (!ValidateTokenAndGetClaims(req, out var claimsPrincipal))
            {
                resp.StatusCode = 401; // Unauthorized
                await SendResponse(resp, "{\"error\":\"Unauthorized: Invalid token.\"}", "application/json");
                return;
            }

            if (!HasValidRole(claimsPrincipal, "Admin", "User"))
            {
                resp.StatusCode = 403; // Forbidden
                await SendResponse(resp, "{\"error\":\"Forbidden: Insufficient permissions.\"}", "application/json");
                return;
            }
            var newEmployee = await ReadRequestBodyAsync<JobPostings>(req);
            await jobPostingsService.DeleteJobPostingsAsync(newEmployee.postingId);
            await SendResponse(resp, "Employee inserted successfully.");
        }
        catch (Exception ex)
        {
            await HandleError(resp, ex);
        }
    }
    private static async Task InsertJobPosting(HttpListenerRequest req, HttpListenerResponse resp)
    {

        try
        {
            if (!ValidateTokenAndGetClaims(req, out var claimsPrincipal))
            {
                resp.StatusCode = 401; // Unauthorized
                await SendResponse(resp, "{\"error\":\"Unauthorized: Invalid token.\"}", "application/json");
                return;
            }

            if (!HasValidRole(claimsPrincipal, "Admin", "User"))
            {
                resp.StatusCode = 403; // Forbidden
                await SendResponse(resp, "{\"error\":\"Forbidden: Insufficient permissions.\"}", "application/json");
                return;
            }

            Console.WriteLine("Entered try part 1");

            var newJobPosting = await ReadRequestBodyAsync<JobPostings>(req);

            Console.WriteLine("Entered try part 1.5");
            await jobPostingsService.InsertJobPostingsAsync(newJobPosting);
            await SendResponse(resp, "Employee inserted successfully.");
            Console.WriteLine("Exited try part 1");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed part 1");
            await HandleError(resp, ex);
        }
    }
    private static async Task GetJobPostingById(HttpListenerRequest req, HttpListenerResponse resp)
    {

        try
        {
            if (!ValidateTokenAndGetClaims(req, out var claimsPrincipal))
            {
                resp.StatusCode = 401; // Unauthorized
                await SendResponse(resp, "{\"error\":\"Unauthorized: Invalid token.\"}", "application/json");
                return;
            }

            if (!HasValidRole(claimsPrincipal, "Admin", "User"))
            {
                resp.StatusCode = 403; // Forbidden
                await SendResponse(resp, "{\"error\":\"Forbidden: Insufficient permissions.\"}", "application/json");
                return;
            }

            Console.WriteLine("Entered try part 1");
            var newJobPosting = await ReadRequestBodyAsync<JobPostings>(req);
            await jobPostingsService.GetJobPostingByIdAsync(newJobPosting.postingId);
            await SendResponse(resp, "Employee inserted successfully.");
            Console.WriteLine("Exited try part 1");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed part 1");
            await HandleError(resp, ex);
        }
    }



    // APPLICATION
    private static async Task GetAllPayrollsById(HttpListenerRequest req, HttpListenerResponse resp)
    {

        try
        {
            if (!ValidateTokenAndGetClaims(req, out var claimsPrincipal))
            {
                resp.StatusCode = 401; // Unauthorized
                await SendResponse(resp, "{\"error\":\"Unauthorized: Invalid token.\"}", "application/json");
                return;
            }

            if (!HasValidRole(claimsPrincipal, "Admin", "User"))
            {
                resp.StatusCode = 403; // Forbidden
                await SendResponse(resp, "{\"error\":\"Forbidden: Insufficient permissions.\"}", "application/json");
                return;
            }

            Console.WriteLine("Entered try part 1");
            var newPayroll = await ReadRequestBodyAsync<Payroll>(req);
            var payload = await payrollService.GetAllPayrollsByIdAsync(newPayroll.EmployeeId);
            string jsonResponse = JsonConvert.SerializeObject(payload);
            await SendResponse(resp, jsonResponse, "Success");
            Console.WriteLine("Exited try part 1");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed part 1");
            await HandleError(resp, ex);
        }
    }


    private static async Task GetAllApplications(HttpListenerRequest req, HttpListenerResponse resp)
    {
        Console.WriteLine("Method start");
        try
        {
            if (!ValidateTokenAndGetClaims(req, out var claimsPrincipal))
            {
                resp.StatusCode = 401; // Unauthorized
                await SendResponse(resp, "{\"error\":\"Unauthorized: Invalid token.\"}", "application/json");
                return;
            }

            if (!HasValidRole(claimsPrincipal, "Admin", "User"))
            {
                resp.StatusCode = 403; // Forbidden
                await SendResponse(resp, "{\"error\":\"Forbidden: Insufficient permissions.\"}", "application/json");
                return;
            }

            Console.WriteLine("Entered method");

            var Applications = await applicantService.GetAllApplicantsAsync();

            string jsonResponse = JsonConvert.SerializeObject(Applications);
            await SendResponse(resp, jsonResponse, "application/json");
            Console.WriteLine("Method finished");
        }
        catch (Exception ex)
        {
            await HandleError(resp, ex);
        }
    }

    private static async Task UpdateApplication(HttpListenerRequest req, HttpListenerResponse resp)
    {
        try
        {
            if (!ValidateTokenAndGetClaims(req, out var claimsPrincipal))
            {
                resp.StatusCode = 401; // Unauthorized
                await SendResponse(resp, "{\"error\":\"Unauthorized: Invalid token.\"}", "application/json");
                return;
            }

            if (!HasValidRole(claimsPrincipal, "Admin", "User"))
            {
                resp.StatusCode = 403; // Forbidden
                await SendResponse(resp, "{\"error\":\"Forbidden: Insufficient permissions.\"}", "application/json");
                return;
            }

            var Applications = await ReadRequestBodyAsync<Applicant>
                (req);
            await applicantService.UpdateApplicantAsync(Applications);
            await SendResponse(resp, "Employee inserted successfully.");
        }
        catch (Exception ex)
        {
            await HandleError(resp, ex);
        }
    }

    private static async Task DeleteApplication(HttpListenerRequest req, HttpListenerResponse resp)
    {
        try
        {
            if (!ValidateTokenAndGetClaims(req, out var claimsPrincipal))
            {
                resp.StatusCode = 401; // Unauthorized
                await SendResponse(resp, "{\"error\":\"Unauthorized: Invalid token.\"}", "application/json");
                return;
            }

            if (!HasValidRole(claimsPrincipal, "Admin", "User"))
            {
                resp.StatusCode = 403; // Forbidden
                await SendResponse(resp, "{\"error\":\"Forbidden: Insufficient permissions.\"}", "application/json");
                return;
            }

            var newEmployee = await ReadRequestBodyAsync<Applicant>
                (req);
            await applicantService.DeleteApplicantAsync(newEmployee.applicantId);
            await SendResponse(resp, "Employee inserted successfully.");
        }
        catch (Exception ex)
        {
            await HandleError(resp, ex);
        }
    }
    private static async Task InsertApplication(HttpListenerRequest req, HttpListenerResponse resp)
    {
        try
        {
            if (!ValidateTokenAndGetClaims(req, out var claimsPrincipal))
            {
                resp.StatusCode = 401; // Unauthorized
                await SendResponse(resp, "{\"error\":\"Unauthorized: Invalid token.\"}", "application/json");
                return;
            }

            if (!HasValidRole(claimsPrincipal, "Admin", "User"))
            {
                resp.StatusCode = 403; // Forbidden
                await SendResponse(resp, "{\"error\":\"Forbidden: Insufficient permissions.\"}", "application/json");
                return;
            }
            var newApplication = await ReadRequestBodyAsync<Applicant>(req);
            await applicantService.InsertApplicantAsync(newApplication);
            await SendResponse(resp, "Employee inserted successfully.");
        }
        catch (Exception ex)
        {
            await HandleError(resp, ex);
        }
    }

    // DEPARTMENTS
    private static async Task GetDepartments(HttpListenerRequest req, HttpListenerResponse resp)
    {
        try
        {
            if (!ValidateTokenAndGetClaims(req, out var claimsPrincipal))
            {
                resp.StatusCode = 401; // Unauthorized
                await SendResponse(resp, "{\"error\":\"Unauthorized: Invalid token.\"}", "application/json");
                return;
            }

            if (!HasValidRole(claimsPrincipal, "Admin", "User"))
            {
                resp.StatusCode = 403; // Forbidden
                await SendResponse(resp, "{\"error\":\"Forbidden: Insufficient permissions.\"}", "application/json");
                return;
            }
            var Departments = await departmentService.GetAllDepartmentsAsync();
            string jsonResponse = JsonConvert.SerializeObject(Departments);
            await SendResponse(resp, jsonResponse, "application/json");
        }
        catch (Exception ex)
        {
            await HandleError(resp, ex);
        }
    }

    private static async Task InsertDepartment(HttpListenerRequest req, HttpListenerResponse resp)
    {
        try
        {
            if (!ValidateTokenAndGetClaims(req, out var claimsPrincipal))
            {
                resp.StatusCode = 401; // Unauthorized
                await SendResponse(resp, "{\"error\":\"Unauthorized: Invalid token.\"}", "application/json");
                return;
            }

            if (!HasValidRole(claimsPrincipal, "Admin", "User"))
            {
                resp.StatusCode = 403; // Forbidden
                await SendResponse(resp, "{\"error\":\"Forbidden: Insufficient permissions.\"}", "application/json");
                return;
            }
            var newDepartment = await ReadRequestBodyAsync<Department>(req);
            await departmentService.InsertDepartmentAsync(newDepartment);
            await SendResponse(resp, "Department inserted successfully.");
        }
        catch (Exception ex)
        {
            await HandleError(resp, ex);
        }
    }
    private static async Task UpdateDepartment(HttpListenerRequest req, HttpListenerResponse resp)
    {
        try
        {
            if (!ValidateTokenAndGetClaims(req, out var claimsPrincipal))
            {
                resp.StatusCode = 401; // Unauthorized
                await SendResponse(resp, "{\"error\":\"Unauthorized: Invalid token.\"}", "application/json");
                return;
            }

            if (!HasValidRole(claimsPrincipal, "Admin", "User"))
            {
                resp.StatusCode = 403; // Forbidden
                await SendResponse(resp, "{\"error\":\"Forbidden: Insufficient permissions.\"}", "application/json");
                return;
            }

            var newDepartment = await ReadRequestBodyAsync<Department>(req);
            await departmentService.UpdateDepartmentAsync(newDepartment);
            await SendResponse(resp, "Department inserted successfully.");
        }
        catch (Exception ex)
        {
            await HandleError(resp, ex);
        }
    }

    private static async Task DeleteDepartment(HttpListenerRequest req, HttpListenerResponse resp)
    {
        try
        {
            if (!ValidateTokenAndGetClaims(req, out var claimsPrincipal))
            {
                resp.StatusCode = 401; // Unauthorized
                await SendResponse(resp, "{\"error\":\"Unauthorized: Invalid token.\"}", "application/json");
                return;
            }

            if (!HasValidRole(claimsPrincipal, "Admin", "User"))
            {
                resp.StatusCode = 403; // Forbidden
                await SendResponse(resp, "{\"error\":\"Forbidden: Insufficient permissions.\"}", "application/json");
                return;
            }

            var newDepartment = await ReadRequestBodyAsync<Department>(req);
            await departmentService.DeleteDepartmentAsync(newDepartment.DepartmentId);
            await SendResponse(resp, "Department inserted successfully.");
        }
        catch (Exception ex)
        {
            await HandleError(resp, ex);
        }
    }

    // MANAGERS
    private static async Task GetManagers(HttpListenerRequest req, HttpListenerResponse resp)
    {
        try
        {
            if (!ValidateTokenAndGetClaims(req, out var claimsPrincipal))
            {
                resp.StatusCode = 401; // Unauthorized
                await SendResponse(resp, "{\"error\":\"Unauthorized: Invalid token.\"}", "application/json");
                return;
            }

            if (!HasValidRole(claimsPrincipal, "Admin", "User"))
            {
                resp.StatusCode = 403; // Forbidden
                await SendResponse(resp, "{\"error\":\"Forbidden: Insufficient permissions.\"}", "application/json");
                return;
            }

            var Managers = await managerService.GetAllManagersAsync();
            string jsonResponse = JsonConvert.SerializeObject(Managers);
            await SendResponse(resp, jsonResponse, "application/json");
        }
        catch (Exception ex)
        {
            await HandleError(resp, ex);
        }
    }

    private static async Task InsertManager(HttpListenerRequest req, HttpListenerResponse resp)
    {
        try
        {
            if (!ValidateTokenAndGetClaims(req, out var claimsPrincipal))
            {
                resp.StatusCode = 401; // Unauthorized
                await SendResponse(resp, "{\"error\":\"Unauthorized: Invalid token.\"}", "application/json");
                return;
            }

            if (!HasValidRole(claimsPrincipal, "Admin", "User"))
            {
                resp.StatusCode = 403; // Forbidden
                await SendResponse(resp, "{\"error\":\"Forbidden: Insufficient permissions.\"}", "application/json");
                return;
            }

            var newManager = await ReadRequestBodyAsync<Manager>(req);
            await managerService.InsertManagerAsync(newManager);
            await SendResponse(resp, "Manager inserted successfully.");
        }
        catch (Exception ex)
        {
            await HandleError(resp, ex);
        }
    }
    private static async Task UpdateManager(HttpListenerRequest req, HttpListenerResponse resp)
    {
        try
        {
            if (!ValidateTokenAndGetClaims(req, out var claimsPrincipal))
            {
                resp.StatusCode = 401; // Unauthorized
                await SendResponse(resp, "{\"error\":\"Unauthorized: Invalid token.\"}", "application/json");
                return;
            }

            if (!HasValidRole(claimsPrincipal, "Admin", "User"))
            {
                resp.StatusCode = 403; // Forbidden
                await SendResponse(resp, "{\"error\":\"Forbidden: Insufficient permissions.\"}", "application/json");
                return;
            }

            var newManager = await ReadRequestBodyAsync<Manager>(req);
            await managerService.UpdateManagerAsync(newManager);
            await SendResponse(resp, "Manager inserted successfully.");
        }
        catch (Exception ex)
        {
            await HandleError(resp, ex);
        }
    }

    private static async Task DeleteManager(HttpListenerRequest req, HttpListenerResponse resp)
    {
        try
        {
            if (!ValidateTokenAndGetClaims(req, out var claimsPrincipal))
            {
                resp.StatusCode = 401; // Unauthorized
                await SendResponse(resp, "{\"error\":\"Unauthorized: Invalid token.\"}", "application/json");
                return;
            }

            if (!HasValidRole(claimsPrincipal, "Admin", "User"))
            {
                resp.StatusCode = 403; // Forbidden
                await SendResponse(resp, "{\"error\":\"Forbidden: Insufficient permissions.\"}", "application/json");
                return;
            }

            var newManager = await ReadRequestBodyAsync<Manager>(req);
            await managerService.DeleteManagerAsync(newManager.ManagerId);
            await SendResponse(resp, "Manager inserted successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Test");
            await HandleError(resp, ex);
        }
    }

    // COUNTRY
    private static async Task GetCountries(HttpListenerRequest req, HttpListenerResponse resp)
    {
        try
        {
            if (!ValidateTokenAndGetClaims(req, out var claimsPrincipal))
            {
                resp.StatusCode = 401; // Unauthorized
                await SendResponse(resp, "{\"error\":\"Unauthorized: Invalid token.\"}", "application/json");
                return;
            }

            if (!HasValidRole(claimsPrincipal, "Admin", "User"))
            {
                resp.StatusCode = 403; // Forbidden
                await SendResponse(resp, "{\"error\":\"Forbidden: Insufficient permissions.\"}", "application/json");
                return;
            }

            var Countrys = await countryService.GetAllCountriesAsync();
            string jsonResponse = JsonConvert.SerializeObject(Countrys);
            await SendResponse(resp, jsonResponse, "application/json");
        }
        catch (Exception ex)
        {
            await HandleError(resp, ex);
        }
    }

    private static async Task InsertCountry(HttpListenerRequest req, HttpListenerResponse resp)
    {
        try
        {
            if (!ValidateTokenAndGetClaims(req, out var claimsPrincipal))
            {
                resp.StatusCode = 401; // Unauthorized
                await SendResponse(resp, "{\"error\":\"Unauthorized: Invalid token.\"}", "application/json");
                return;
            }

            if (!HasValidRole(claimsPrincipal, "Admin", "User"))
            {
                resp.StatusCode = 403; // Forbidden
                await SendResponse(resp, "{\"error\":\"Forbidden: Insufficient permissions.\"}", "application/json");
                return;
            }

            var newCountry = await ReadRequestBodyAsync<Country>(req);
            await countryService.InsertCountryAsync(newCountry);
            await SendResponse(resp, "Country inserted successfully.");
        }
        catch (Exception ex)
        {
            await HandleError(resp, ex);
        }
    }

    private static async Task UpdateCountry(HttpListenerRequest req, HttpListenerResponse resp)
    {
        try
        {
            if (!ValidateTokenAndGetClaims(req, out var claimsPrincipal))
            {
                resp.StatusCode = 401; // Unauthorized
                await SendResponse(resp, "{\"error\":\"Unauthorized: Invalid token.\"}", "application/json");
                return;
            }

            if (!HasValidRole(claimsPrincipal, "Admin", "User"))
            {
                resp.StatusCode = 403; // Forbidden
                await SendResponse(resp, "{\"error\":\"Forbidden: Insufficient permissions.\"}", "application/json");
                return;
            }

            var newCountry = await ReadRequestBodyAsync<Country>(req);
            await countryService.UpdateCountryAsync(newCountry);
            await SendResponse(resp, "Country inserted successfully.");
        }
        catch (Exception ex)
        {
            await HandleError(resp, ex);
        }
    }

    private static async Task DeleteCountry(HttpListenerRequest req, HttpListenerResponse resp)
    {
        try
        {
            if (!ValidateTokenAndGetClaims(req, out var claimsPrincipal))
            {
                resp.StatusCode = 401; // Unauthorized
                await SendResponse(resp, "{\"error\":\"Unauthorized: Invalid token.\"}", "application/json");
                return;
            }

            if (!HasValidRole(claimsPrincipal, "Admin", "User"))
            {
                resp.StatusCode = 403; // Forbidden
                await SendResponse(resp, "{\"error\":\"Forbidden: Insufficient permissions.\"}", "application/json");
                return;
            }

            var newCountry = await ReadRequestBodyAsync<Country>(req);
            await countryService.DeleteCountryAsync(newCountry.CountryId);
            await SendResponse(resp, "Country inserted successfully.");
        }
        catch (Exception ex)
        {
            await HandleError(resp, ex);
        }
    }

    // Region
    private static async Task GetAllRegions(HttpListenerRequest req, HttpListenerResponse resp)
    {
        try
        {
            if (!ValidateTokenAndGetClaims(req, out var claimsPrincipal))
            {
                resp.StatusCode = 401; // Unauthorized
                await SendResponse(resp, "{\"error\":\"Unauthorized: Invalid token.\"}", "application/json");
                return;
            }

            if (!HasValidRole(claimsPrincipal, "Admin", "User"))
            {
                resp.StatusCode = 403; // Forbidden
                await SendResponse(resp, "{\"error\":\"Forbidden: Insufficient permissions.\"}", "application/json");
                return;
            }

            Console.WriteLine("Entered Regions");
            var Regions = await regionService.GetAllRegionsAsync();
            string jsonResponse = JsonConvert.SerializeObject(Regions);
            await SendResponse(resp, jsonResponse, "application/json");
        }
        catch (Exception ex)
        {
            await HandleError(resp, ex);
        }
    }
    private static async Task InsertRegion(HttpListenerRequest req, HttpListenerResponse resp)
    {
        try
        {
            if (!ValidateTokenAndGetClaims(req, out var claimsPrincipal))
            {
                resp.StatusCode = 401; // Unauthorized
                await SendResponse(resp, "{\"error\":\"Unauthorized: Invalid token.\"}", "application/json");
                return;
            }

            if (!HasValidRole(claimsPrincipal, "Admin", "User"))
            {
                resp.StatusCode = 403; // Forbidden
                await SendResponse(resp, "{\"error\":\"Forbidden: Insufficient permissions.\"}", "application/json");
                return;
            }

            var newRegion = await ReadRequestBodyAsync<Region>(req);
            await regionService.InsertRegionAsync(newRegion);
            await SendResponse(resp, "Region inserted successfully.");
        }
        catch (Exception ex)
        {
            await HandleError(resp, ex);
        }
    }
    private static async Task UpdateRegion(HttpListenerRequest req, HttpListenerResponse resp)
    {
        try
        {
            if (!ValidateTokenAndGetClaims(req, out var claimsPrincipal))
            {
                resp.StatusCode = 401; // Unauthorized
                await SendResponse(resp, "{\"error\":\"Unauthorized: Invalid token.\"}", "application/json");
                return;
            }

            if (!HasValidRole(claimsPrincipal, "Admin", "User"))
            {
                resp.StatusCode = 403; // Forbidden
                await SendResponse(resp, "{\"error\":\"Forbidden: Insufficient permissions.\"}", "application/json");
                return;
            }

            var newRegion = await ReadRequestBodyAsync<Region>(req);
            await regionService.UpdateRegionAsync(newRegion);
            await SendResponse(resp, "Region inserted successfully.");
        }
        catch (Exception ex)
        {
            await HandleError(resp, ex);
        }
    }

    private static async Task DeleteRegion(HttpListenerRequest req, HttpListenerResponse resp)
    {
        try
        {
            if (!ValidateTokenAndGetClaims(req, out var claimsPrincipal))
            {
                resp.StatusCode = 401; // Unauthorized
                await SendResponse(resp, "{\"error\":\"Unauthorized: Invalid token.\"}", "application/json");
                return;
            }

            if (!HasValidRole(claimsPrincipal, "Admin", "User"))
            {
                resp.StatusCode = 403; // Forbidden
                await SendResponse(resp, "{\"error\":\"Forbidden: Insufficient permissions.\"}", "application/json");
                return;
            }
            var newRegion = await ReadRequestBodyAsync<Region>(req);
            await regionService.DeleteRegionAsync(newRegion.RegionId);
            await SendResponse(resp, "Region inserted successfully.");
        }
        catch (Exception ex)
        {
            await HandleError(resp, ex);
        }
    }

    private static async Task GetLocations(HttpListenerRequest req, HttpListenerResponse resp)
    {
        try
        {
            if (!ValidateTokenAndGetClaims(req, out var claimsPrincipal))
            {
                resp.StatusCode = 401; // Unauthorized
                await SendResponse(resp, "{\"error\":\"Unauthorized: Invalid token.\"}", "application/json");
                return;
            }

            if (!HasValidRole(claimsPrincipal, "Admin", "User"))
            {
                resp.StatusCode = 403; // Forbidden
                await SendResponse(resp, "{\"error\":\"Forbidden: Insufficient permissions.\"}", "application/json");
                return;
            }
            var Locations = await locationService.GetAllLocationsAsync();
            string jsonResponse = JsonConvert.SerializeObject(Locations);
            await SendResponse(resp, jsonResponse, "application/json");
        }
        catch (Exception ex)
        {
            await HandleError(resp, ex);
        }
    }
    private static async Task GetLocationById(HttpListenerRequest req, HttpListenerResponse resp)
    {

        try
        {
            if (!ValidateTokenAndGetClaims(req, out var claimsPrincipal))
            {
                resp.StatusCode = 401; // Unauthorized
                await SendResponse(resp, "{\"error\":\"Unauthorized: Invalid token.\"}", "application/json");
                return;
            }

            if (!HasValidRole(claimsPrincipal, "Admin", "User"))
            {
                resp.StatusCode = 403; // Forbidden
                await SendResponse(resp, "{\"error\":\"Forbidden: Insufficient permissions.\"}", "application/json");
                return;
            }
            Console.WriteLine("Entered try part 1");
            var location = await ReadRequestBodyAsync<Location>(req);
            var locationObject = await locationService.GetLocationIdAsync(location.LocationId);
            string jsonResponse = JsonConvert.SerializeObject(locationObject);
            await SendResponse(resp, jsonResponse, "application/json");
            Console.WriteLine("Exited try part 1");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed part 1");
            await HandleError(resp, ex);
        }
    }
    private static async Task InsertLocation(HttpListenerRequest req, HttpListenerResponse resp)
    {
        try
        {
            if (!ValidateTokenAndGetClaims(req, out var claimsPrincipal))
            {
                resp.StatusCode = 401; // Unauthorized
                await SendResponse(resp, "{\"error\":\"Unauthorized: Invalid token.\"}", "application/json");
                return;
            }

            if (!HasValidRole(claimsPrincipal, "Admin", "User"))
            {
                resp.StatusCode = 403; // Forbidden
                await SendResponse(resp, "{\"error\":\"Forbidden: Insufficient permissions.\"}", "application/json");
                return;
            }
            var newLocation = await ReadRequestBodyAsync<Location>(req);
            await locationService.InsertLocationAsync(newLocation);
            await SendResponse(resp, "Location inserted successfully.");
        }
        catch (Exception ex)
        {
            await HandleError(resp, ex);
        }
    }

    private static async Task UpdateLocation(HttpListenerRequest req, HttpListenerResponse resp)
    {
        try
        {
            if (!ValidateTokenAndGetClaims(req, out var claimsPrincipal))
            {
                resp.StatusCode = 401; // Unauthorized
                await SendResponse(resp, "{\"error\":\"Unauthorized: Invalid token.\"}", "application/json");
                return;
            }

            if (!HasValidRole(claimsPrincipal, "Admin", "User"))
            {
                resp.StatusCode = 403; // Forbidden
                await SendResponse(resp, "{\"error\":\"Forbidden: Insufficient permissions.\"}", "application/json");
                return;
            }
            var newLocation = await ReadRequestBodyAsync<Location>(req);
            await locationService.UpdateLocationAsync(newLocation);
            await SendResponse(resp, "Location inserted successfully.");
        }
        catch (Exception ex)
        {
            await HandleError(resp, ex);
        }
    }

    private static async Task DeleteLocation(HttpListenerRequest req, HttpListenerResponse resp)
    {
        try
        {
            if (!ValidateTokenAndGetClaims(req, out var claimsPrincipal))
            {
                resp.StatusCode = 401; // Unauthorized
                await SendResponse(resp, "{\"error\":\"Unauthorized: Invalid token.\"}", "application/json");
                return;
            }

            if (!HasValidRole(claimsPrincipal, "Admin", "User"))
            {
                resp.StatusCode = 403; // Forbidden
                await SendResponse(resp, "{\"error\":\"Forbidden: Insufficient permissions.\"}", "application/json");
                return;
            }
            var newLocation = await ReadRequestBodyAsync<Location>(req);
            await locationService.DeleteLocationAsync(newLocation.LocationId);
            await SendResponse(resp, "Location inserted successfully.");
        }
        catch (Exception ex)
        {
            await HandleError(resp, ex);
        }
    }




    // STATIC FOR KNOWN FILE TYPES USED FOR STATIC FILES THAT NEED REQUESTING
    public static async Task ServeStaticFile(HttpListenerRequest req, HttpListenerResponse resp, string requestedPath)
    {
        string filePath = Path.Combine(HttpServer.ResourcesDirectory, requestedPath);

        if (File.Exists(filePath))
        {
            string contentType = GetContentType(filePath);
            byte[] data = await File.ReadAllBytesAsync(filePath);
            resp.ContentType = contentType;
            resp.ContentLength64 = data.LongLength;
            await resp.OutputStream.WriteAsync(data, 0, data.Length);
        }
        else
        {
            resp.StatusCode = (int)HttpStatusCode.NotFound;
            await SendResponse(resp, "404 Not Found");
        }
    }

    private static string GetContentType(string filePath)
    {
        string extension = Path.GetExtension(filePath).ToLowerInvariant();
        return extension switch
        {
            ".html" => "text/html",
            ".css" => "text/css",
            ".js" => "application/javascript",
            ".jpg" => "image/jpeg",
            ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".svg" => "image/svg+xml",
            _ => "application/octet-stream",
        };
    }
}
