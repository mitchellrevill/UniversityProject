using System.Net;

namespace HRSystem
{
    class HttpServer
    {
        public static HttpListener listener;
        public static string url = "http://localhost:8000/";
        public static int requestCount = 0;

        private static string baseDirectory = GetBaseDirectory();
        public static string ResourcesDirectory = Path.Combine(baseDirectory, "Resources");

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

                await MethodHandle.HandleRequest(req, resp);

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

        private static string GetBaseDirectory()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            return Directory.GetParent(currentDirectory).Parent.Parent.FullName;
        }
    }
}
