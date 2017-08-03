using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using WebSocketSharp;
using WebSocketSharp.Net;
using WebSocketSharp.Server;
using PTZ;

namespace PTZLocalService
{

    public partial class LocalService : ServiceBase
    {

        private HttpServer httpsv;

        public LocalService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            PTZDevice device;
            httpsv = new HttpServer(4649);

            httpsv.OnGet += (sender, e) =>
            {
                HttpListenerRequest req = e.Request;
                HttpListenerResponse res = e.Response;

                string msg = "";
                if (req.QueryString.ToString().Contains("init"))
                {
                    try
                    {
                        device = PTZDevice.GetDevice("BCC950 ConferenceCam", PTZType.Relative);
                    }
                    catch (Exception ex)
                    {
                        msg = ex.Message;
                    }
                }

                string json = "{\"result\":\"" + msg + "\", \"query\":\"" + req.QueryString + "\"}";
                res.StatusCode = (int)HttpStatusCode.OK;
                res.ContentType = "application/json";
                res.ContentEncoding = Encoding.UTF8;

                res.WriteContent(Encoding.UTF8.GetBytes(json));
            };

            httpsv.Start();

            if (httpsv.IsListening)
            {
                Console.WriteLine("Listening on port {0}, and providing WebSocket services:", httpsv.Port);
                foreach (var path in httpsv.WebSocketServices.Paths)
                    Console.WriteLine("- {0}", path);
            }

            
        }

        protected override void OnStop()
        {
            if (httpsv != null)
            {
                httpsv.Stop();
            }
        }
    }
}
