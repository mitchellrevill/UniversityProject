using HRSystem;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using UniversityProject.Interfaces;
using UniversityProject.Model;

public static class MethodHandle
{
    private static readonly string dbPath = Path.Combine(HttpServer.ResourcesDirectory, "database.db");
    private static readonly IEmployeeService employeeService = new EmployeeService(dbPath);

    // Get requests
    private static readonly Dictionary<string, Func<HttpListenerRequest, HttpListenerResponse, Task>> _getRoutes =
        new Dictionary<string, Func<HttpListenerRequest, HttpListenerResponse, Task>>
        {
        { "GetAllEmployees", GetEmployees },
        };

    // Post requests
    private static readonly Dictionary<string, Func<HttpListenerRequest, HttpListenerResponse, Task>> _postRoutes =
        new Dictionary<string, Func<HttpListenerRequest, HttpListenerResponse, Task>>
        {
        { "insertEmployee", InsertEmployee },
        { "UpdateEmployees", UpdateEmployee },
        { "DeleteEmployee", DeleteEmployee },
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
