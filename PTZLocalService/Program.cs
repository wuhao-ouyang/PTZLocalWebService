using System;
using System.Text;
using WebSocketSharp;
using WebSocketSharp.Net;
using WebSocketSharp.Server;
using PTZ;
using System.ServiceProcess;
using System.Text.RegularExpressions;

namespace PTZLocalService
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static void Main()
        {
            System.ServiceProcess.ServiceBase.Run(new LocalService());

            //PTZDevice device;
            //HttpServer httpsv = new HttpServer(4649);

            //httpsv.OnGet += (sender, e) =>
            //{
            //    HttpListenerRequest req = e.Request;
            //    HttpListenerResponse res = e.Response;

            //    String msg = "";
            //    if (req.QueryString.ToString().Contains("init"))
            //    {
            //        try
            //        {
            //            device = PTZDevice.GetDevice("BCC950 ConferenceCam", PTZType.Relative);
            //        }
            //        catch (Exception ex)
            //        {
            //            msg = ex.Message;
            //        }
            //    }

            //    String json = "{\"result\":\"" + msg + "\", \"query\":\"" + req.QueryString + "\"}";
            //    res.StatusCode = (int)HttpStatusCode.OK;
            //    res.ContentType = "application/json";
            //    res.ContentEncoding = Encoding.UTF8;

            //    res.WriteContent(Encoding.UTF8.GetBytes(json));
            //};

            //httpsv.Start();
            //if (httpsv.IsListening)
            //{
            //    Console.WriteLine("Listening on port {0}, and providing WebSocket services:", httpsv.Port);
            //    foreach (var path in httpsv.WebSocketServices.Paths)
            //        Console.WriteLine("- {0}", path);
            //}

            //Console.WriteLine("\nPress Enter key to stop the server...");
            //Console.ReadLine();

            //while (true) ;

            //httpsv.Stop();
        }
    }
}
