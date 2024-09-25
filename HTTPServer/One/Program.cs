using System.Net;
using System.Text;

namespace One;

class Program
{
    static async Task Main(string[] args)
    {
        HttpListener listener = new HttpListener();
        listener.Prefixes.Add("http://localhost:8888/");
        listener.Start();
        Console.WriteLine("Waiting for a request...");

        while (true)
        {
            HttpListenerContext context = await listener.GetContextAsync();
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;

            string method = request.HttpMethod;
            string requestPath = request.Url.AbsolutePath;
            string resText = "";
            int statusCode = 200;
            if (method == "GET" && requestPath == "/")
            {
                resText = @"
                    <html>
                        <head>
                            <meta charset='utf-8'>
                            <style>
                                body { 
                                    background-color: #323140;
                                    color: #cffff4; 
                                }
                                h1 { 
                                    text-align: center; 
                                    padding: 1.2em;
                                    border: 0.2em solid #cffff4;
                                    margin: 2em auto;
                                }
                            </style>
                        </head>
                        <body>
                            <h1>Follow the white rabbit</h1>
                        </body>
                    </html>";
            }
            else if (method == "GET" && requestPath == "/white_rabbit")
            {
                resText = @"
                    <html>
                        <head>
                            <meta charset='utf-8'>
                            <style>
                                body { 
                                    background-color: #323140;
                                    color: #fff5cf; 
                                }
                                h1 { 
                                    text-align: center; 
                                    padding: 1.2em;
                                    border: 0.2em solid #fff5cf;
                                    margin: 2em auto;
                                }
                            </style>
                        </head>
                        <body>
                            <h1>You are living in the matrix</h1>
                        </body>
                    </html>";
            }
            
            response.StatusCode = statusCode;
            byte[] buffer = Encoding.UTF8.GetBytes(resText);
            response.ContentLength64 = buffer.Length;
            await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
            response.OutputStream.Close();
        }
    }
}