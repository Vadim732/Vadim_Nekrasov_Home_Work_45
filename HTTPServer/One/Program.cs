using System.Net;

namespace One;

class Program
{
    static async Task Main(string[] args)
    {
        HttpListener listener = new HttpListener();
        listener.Prefixes.Add("http://localhost:5000/");
        listener.Start();
        Console.WriteLine("Waiting for a request...");
        while (true)
        {
            HttpListenerContext context = await listener.GetContextAsync();
            HttpListenerResponse response = context.Response;
            
            string requestPath = context.Request.Url.AbsolutePath.Trim('/');
            if (string.IsNullOrEmpty(Path.GetExtension(requestPath)))
            {
                string headerText = string.IsNullOrEmpty(requestPath) ? "Hello World!" : requestPath;
                string resText = $"""
                                  <html>
                                     <head>
                                         <meta charset='utf-8'>
                                     </head>
                                     <body>
                                         <h1>{headerText}</h1>
                                     </body>
                                  </html>
                                  """;

                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(resText);
                response.ContentLength64 = buffer.Length;
                Stream output = response.OutputStream;
                await output.WriteAsync(buffer, 0, buffer.Length);
                context.Response.OutputStream.Close();
            }
            else
            {
                response.StatusCode = (int)HttpStatusCode.NotFound;
                response.Close();
            }
        }
    }
}