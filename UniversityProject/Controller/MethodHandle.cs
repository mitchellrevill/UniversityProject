using HRSystem;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using UniversityProject.Interfaces;
using UniversityProject.Model;

public static class MethodHandle
{
    private static readonly string dbPath = Path.Combine(HttpServer.ResourcesDirectory, "database.db");
    private static readonly IEmployeeService employeeService = new Employee_Service(dbPath);
    private static readonly IJobPostingsService jobPostingsService = new JobPostingsService(dbPath);
    private static readonly ApplicantService ApplicantService = new ApplicantService(dbPath);
    // Get requests
    private static readonly Dictionary<string, Func<HttpListenerRequest, HttpListenerResponse, Task>> _getRoutes =
        new Dictionary<string, Func<HttpListenerRequest, HttpListenerResponse, Task>>
        {
        { "GetAllEmployees", GetEmployees },
        { "GetAllJobPostings", GetAllJobPostings },
        { "GetAllApplications", GetAllApplications}
        };

    // Post requests
    private static readonly Dictionary<string, Func<HttpListenerRequest, HttpListenerResponse, Task>> _postRoutes =
        new Dictionary<string, Func<HttpListenerRequest, HttpListenerResponse, Task>>
        {
        { "insertEmployee", InsertEmployee },
        { "UpdateEmployees", UpdateEmployee },
        { "DeleteEmployee", DeleteEmployee },
        { "InsertJobPosting", InsertJobPosting },
        { "UpdateJobPosting", UpdateJobPosting },
        { "DeleteJobPosting", DeleteJobPosting },
        { "InsertApplication", InsertApplication },
        { "UpdateApplication", UpdateApplication },
        { "DeleteApplication", DeleteApplication }
        };


    

    public static async Task HandleRequest(HttpListenerRequest req, HttpListenerResponse resp)
    {
        string requestedPath = req.Url.AbsolutePath.TrimStart('/');

        if (req.HttpMethod == "POST" && _postRoutes.TryGetValue(requestedPath, out var postHandler)) // Is dictionary same as request? if true out value (method) post handler 
        {
            await postHandler(req, resp); // post handler inherits / invokes method that was outed from the dictionary FUCKING HELL THIS WAS PAIN 
        }
        else if (req.HttpMethod == "GET" && _getRoutes.TryGetValue(requestedPath, out var getHandler))
        {
            await getHandler(req, resp);
        }
        else
        {
            await ServeStaticFile(req, resp, requestedPath);
        }
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



    private static async Task GetEmployees(HttpListenerRequest req, HttpListenerResponse resp)
    {
        try
        {
            var employees = await employeeService.GetAllEmployeesAsync();
            string jsonResponse = JsonConvert.SerializeObject(employees);
            await SendResponse(resp, jsonResponse, "application/json");
        }
        catch (Exception ex)
        {
            await HandleError(resp, ex);
        }
    }

    private static async Task InsertEmployee(HttpListenerRequest req, HttpListenerResponse resp)
    {
        try
        {
            var newEmployee = await ReadRequestBodyAsync<Employee>(req);
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
            var newEmployee = await ReadRequestBodyAsync<Employee>(req);
            await employeeService.DeleteEmployeeAsync(newEmployee.EmployeeId);
            await SendResponse(resp, "Employee inserted successfully.");
        }
        catch (Exception ex)
        {
            await HandleError(resp, ex);
        }
    }

    private static async Task GetAllJobPostings(HttpListenerRequest req, HttpListenerResponse resp)
    {
        Console.WriteLine("Method start");
        try
        {
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
            Console.WriteLine("Entered try part 1");
            var newJobPosting= await ReadRequestBodyAsync<JobPostings>(req);
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



    private static async Task GetAllApplications(HttpListenerRequest req, HttpListenerResponse resp)
    {
        Console.WriteLine("Method start");
        try
        {
            Console.WriteLine("Entered method");
            var Applications = await ApplicantService.GetAllApplicantsAsync();
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
            var Applications = await ReadRequestBodyAsync<Applicant>
                (req);
            await ApplicantService.UpdateApplicantAsync(Applications);
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
            var newEmployee = await ReadRequestBodyAsync<Applicant>
                (req);
            await ApplicantService.DeleteApplicantAsync(newEmployee.applicantId);
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
            var newApplication = await ReadRequestBodyAsync<Applicant>(req);
            await ApplicantService.InsertApplicantAsync(newApplication);
            await SendResponse(resp, "Employee inserted successfully.");
        }
        catch (Exception ex)
        {
            await HandleError(resp, ex);
        }
    }



    // STATIC FOR KNOWN FILE TYPES USED FOR STATIC FILES THAT NEED REQUESTING
    private static async Task ServeStaticFile(HttpListenerRequest req, HttpListenerResponse resp, string requestedPath)
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
