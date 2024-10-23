using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using UniversityProject.Data;
using UniversityProject.Model;

namespace HRSystem
{
    public static class MethodHandle
    {
        // Links to methods here, will handle requests, if request not sent will got to serve static file 
        public static async Task HandleRequest(HttpListenerRequest req, HttpListenerResponse resp)
        {
            string requestedPath = req.Url.AbsolutePath.TrimStart('/');
            Console.WriteLine(requestedPath);
            if (requestedPath == "insertEmployee" && req.HttpMethod == "POST")
            {
                await InsertEmployee(req, resp);
            }
            else if (requestedPath == "GetAllEmployees" && req.HttpMethod == "GET")
            {
                Console.WriteLine("Reached GetAllEmployee method Start");
                await GetEmployees(req, resp);
            }
            else if (requestedPath == "UpdateEmployees" && req.HttpMethod == "POST")
            {
                Console.WriteLine("Reached method Start");
                await UpdateEmployee(req, resp);
            } else if (requestedPath == "DeleteEmployee" && req.HttpMethod == "POST")
            {
                DeleteEmployee(req, resp);
            }
            else
            {
                await ServeStaticFile(req, resp, requestedPath);
            }
        }


        private static async Task GetEmployees(HttpListenerRequest req, HttpListenerResponse resp)
        {
            string dbPath = Path.Combine(HttpServer.ResourcesDirectory, "database.db");
            var employeeDb = new EmployeeDatabase(dbPath);
            //GETS DATA FROM SQL
            var employees = employeeDb.GetAllEmployees();
            //TURNS DATA FROM C# OBject to JSON
            string jsonResponse = JsonConvert.SerializeObject(employees);

           // REPLIES BACK TO WEBSITE
            byte[] data = Encoding.UTF8.GetBytes(jsonResponse);
            resp.ContentType = "application/json";
            resp.ContentLength64 = data.Length;
            await resp.OutputStream.WriteAsync(data, 0, data.Length);
            Console.WriteLine("Reached method end");
        }


        private static async Task InsertEmployee(HttpListenerRequest req, HttpListenerResponse resp)
        {
            using (var reader = new StreamReader(req.InputStream, req.ContentEncoding))
            {
                string json = await reader.ReadToEndAsync();
                var newEmployee = JsonConvert.DeserializeObject<Employee>(json);

                string dbPath = Path.Combine(HttpServer.ResourcesDirectory, "database.db");
                var employeeDb = new EmployeeDatabase(dbPath);
                employeeDb.InsertEmployee(newEmployee);

                byte[] data = Encoding.UTF8.GetBytes("Employee inserted successfully.");
                resp.ContentType = "text/plain";
                resp.ContentLength64 = data.Length;
                await resp.OutputStream.WriteAsync(data, 0, data.Length);
            }
        }

        // Same as insertemployee, used inpart of method to update if current employeeId exists 
        private static async Task UpdateEmployee(HttpListenerRequest req, HttpListenerResponse resp)
        {
            using (var reader = new StreamReader(req.InputStream, req.ContentEncoding))
            {
                string json = await reader.ReadToEndAsync();
                var newEmployee = JsonConvert.DeserializeObject<Employee>(json);

                string dbPath = Path.Combine(HttpServer.ResourcesDirectory, "database.db");
                var employeeDb = new EmployeeDatabase(dbPath);
                employeeDb.InsertEmployee(newEmployee);

                byte[] data = Encoding.UTF8.GetBytes("Employee inserted successfully.");
                resp.ContentType = "text/plain";
                resp.ContentLength64 = data.Length;
                await resp.OutputStream.WriteAsync(data, 0, data.Length);
            }
        }


        // can use employee class to insert only employeeId if needed, no need to pass more than employeeId
        private static async Task DeleteEmployee(HttpListenerRequest req, HttpListenerResponse resp)
        {
            using (var reader = new StreamReader(req.InputStream, req.ContentEncoding))
            {
                string json = await reader.ReadToEndAsync();
                var Employee = JsonConvert.DeserializeObject<Employee>(json);
                string dbPath = Path.Combine(HttpServer.ResourcesDirectory, "database.db");
                var employeeDb = new EmployeeDatabase(dbPath);
                employeeDb.DeleteEmployee(Employee.EmployeeId);
            }
        }




















        // Static dont touch 
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
                byte[] data = Encoding.UTF8.GetBytes("404 Not Found");
                resp.ContentLength64 = data.Length;
                await resp.OutputStream.WriteAsync(data, 0, data.Length);
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
}
