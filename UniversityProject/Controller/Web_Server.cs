using System;
using System.IO;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using UniversityProject.Data;
using UniversityProject.Model;

namespace HRSystem
{
    class HttpServer
    {
        public static HttpListener listener;
        public static string url = "http://localhost:8000/";
        public static int requestCount = 0;

        // Declare baseDirectory as a static field
        private static string baseDirectory = GetBaseDirectory();
        private static string resourcesDirectory = Path.Combine(baseDirectory, "Resources");

        // For demo purposes, let's assume the permission level is stored in a variable
        public static string PermissionLevel = "Admin";

        public static async Task HandleIncomingConnections()
        {
            bool runServer = true;

            while (runServer)
            {
                HttpListenerContext ctx = await listener.GetContextAsync();
                HttpListenerRequest req = ctx.Request;
                HttpListenerResponse resp = ctx.Response;

                Console.WriteLine("Request #: {0}", ++requestCount);
                Console.WriteLine(req.Url.ToString());
                Console.WriteLine(req.HttpMethod);
                Console.WriteLine(req.UserHostName);
                Console.WriteLine(req.UserAgent);
                Console.WriteLine();

                // Get the requested file path based on the URL
                string requestedPath = req.Url.AbsolutePath.TrimStart('/'); // Remove leading slash
                string filePath = Path.Combine(resourcesDirectory, requestedPath); // Combine with the resources directory

                if (File.Exists(filePath))
                {
                    // Serve the requested file
                    string contentType = GetContentType(filePath);
                    byte[] data = File.ReadAllBytes(filePath);
                    resp.ContentType = contentType;
                    resp.ContentLength64 = data.LongLength;

                    // Write out to the response stream
                    await resp.OutputStream.WriteAsync(data, 0, data.Length);
                }
                else if (requestedPath == "Home.html") // Check for specific HTML file
                {
                    // Serve the Home.html file with dynamic content
                    string htmlContent = File.ReadAllText(Path.Combine(resourcesDirectory, "Home.html"))
                        .Replace("{PermissionLevel}", PermissionLevel);
                    byte[] data = Encoding.UTF8.GetBytes(htmlContent);
                    resp.ContentType = "text/html";
                    resp.ContentLength64 = data.LongLength;

                    // Write out to the response stream
                    await resp.OutputStream.WriteAsync(data, 0, data.Length);
                }
                else
                {
                    // File not found - send a 404 response
                    resp.StatusCode = (int)HttpStatusCode.NotFound;
                    byte[] data = Encoding.UTF8.GetBytes("404 Not Found");
                    resp.ContentLength64 = data.Length;
                    await resp.OutputStream.WriteAsync(data, 0, data.Length);
                }

                // Close the response
                resp.Close();
            }
        }

        public static void Main(string[] args)
        {

            listener = new HttpListener();
            listener.Prefixes.Add(url);
            listener.Start();
            Console.WriteLine("Listening for connections on {0}", url);
            Task listenTask = HandleIncomingConnections();
            listenTask.GetAwaiter().GetResult();

            listener.Close();
        }

        // Static method to get the base directory
        private static string GetBaseDirectory()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            return Directory.GetParent(currentDirectory).Parent.Parent.FullName;
        }

        // Helper method to determine the content type based on the file extension
        private static string GetContentType(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLowerInvariant();
            return extension switch
            {
                ".html" => "text/html",
                ".css" => "text/css",
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".svg" => "image/svg+xml",
                _ => "application/octet-stream",
            };
        }
    }
}
/**
//string dbPath = Path.Combine(resourcesDirectory, "database.db");
var employeeDb = new EmployeeDatabase(dbPath);


var newEmployee = new Employee
{
    EmployeeId = "EMPLOYEE123",
    FirstName = "Mitchell",
    LastName = "Revill",
    CompanyEmail = "123@123.com",
    PersonalEmail = "321@321.com",
    PhoneNumber = "07512321321",
    CountryId = "Country123",
    DepartmentId = "Department123",
    ManagerId = "Manager123",
    RegionId = "Region123",
    EmploymentType = "FullTime",
    StartDate = DateTime.Now, // or any specific date
    Salary = 50000.00m, // example salary
    Benefits = "Health, Dental, Vision",
    Status = Employee.EmployeeStatus.FullTime
};


employeeDb.InsertEmployee(newEmployee);
Console.WriteLine("Employee inserted successfully.");

// Retrieve and display all employees
employeeDb.GetAllEmployees();
/**/