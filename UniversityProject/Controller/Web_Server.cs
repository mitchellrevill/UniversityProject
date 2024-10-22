using System;
using System.IO;
using System.Text;
using System.Net;
using System.Threading.Tasks;

namespace HRSystem
{
    class HttpServer
    {
        public static HttpListener listener;
        public static string url = "http://localhost:8000/";
        public static int requestCount = 0;

        // Declare baseDirectory and FilePath as static fields
        private static string baseDirectory = GetBaseDirectory();
        private static string filePath = Path.Combine(baseDirectory, "Resources/Home.html"); // Corrected the path

        // Read the HTML file data into a static field
        public static string pageData = File.ReadAllText(filePath);

        // For demo purposes, let's assume the permission level is stored in a variable
        public static string PermissionLevel = "Admin";

        public static async Task HandleIncomingConnections()
        {
            bool runServer = true;

            // While a user hasn't visited the `shutdown` url, keep on handling requests
            while (runServer)
            {
                // Will wait here until we hear from a connection
                HttpListenerContext ctx = await listener.GetContextAsync();

                // Peel out the requests and response objects
                HttpListenerRequest req = ctx.Request;
                HttpListenerResponse resp = ctx.Response;

                // Print out some info about the request
                Console.WriteLine("Request #: {0}", ++requestCount);
                Console.WriteLine(req.Url.ToString());
                Console.WriteLine(req.HttpMethod);
                Console.WriteLine(req.UserHostName);
                Console.WriteLine(req.UserAgent);
                Console.WriteLine();

                // Check the URL requested
                if (req.Url.AbsolutePath == "/style.css")
                {
                    // Serve the CSS file
                    string cssData = File.ReadAllText("style.css");
                    byte[] data = Encoding.UTF8.GetBytes(cssData);
                    resp.ContentType = "text/css";
                    resp.ContentEncoding = Encoding.UTF8;
                    resp.ContentLength64 = data.LongLength;
                    await resp.OutputStream.WriteAsync(data, 0, data.Length);
                }
                else
                {
                    // Serve the Home.html file with dynamic content
                    string htmlContent = pageData.Replace("{PermissionLevel}", PermissionLevel);
                    byte[] data = Encoding.UTF8.GetBytes(htmlContent);
                    resp.ContentType = "text/html";
                    resp.ContentEncoding = Encoding.UTF8;
                    resp.ContentLength64 = data.LongLength;

                    // Write out to the response stream (asynchronously), then close it
                    await resp.OutputStream.WriteAsync(data, 0, data.Length);
                }

                // Close the response
                resp.Close();
            }
        }

        public static void Main(string[] args)
        {
            // Create a Http server and start listening for incoming connections
            listener = new HttpListener();
            listener.Prefixes.Add(url);
            listener.Start();
            Console.WriteLine("Listening for connections on {0}", url);

            // Handle requests
            Task listenTask = HandleIncomingConnections();
            listenTask.GetAwaiter().GetResult();

            // Close the listener
            listener.Close();
        }

        // Static method to get the base directory
        private static string GetBaseDirectory()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            return Directory.GetParent(currentDirectory).Parent.Parent.FullName;
        }
    }
}
