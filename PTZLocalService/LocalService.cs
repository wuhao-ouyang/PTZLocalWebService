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
using System.Text.RegularExpressions;

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
            PTZDevice device = null;
            httpsv = new HttpServer(4649);

            httpsv.OnGet += (sender, e) =>
            {
                HttpListenerRequest req = e.Request;
                HttpListenerResponse res = e.Response;

                string msg = "";
                string query = req.QueryString.ToString();
                if (query.Contains("init"))
                {
                    Match match = Regex.Match(query, "init=(.+)");
                    string deviceName = match.Groups[1].Captures[0].ToString();
                    try
                    {
                        device = PTZDevice.GetDevice(deviceName, PTZType.Relative);
                        msg = "Device created";
                        res.StatusCode = (int)HttpStatusCode.OK;
                    }
                    catch (Exception ex)
                    {
                        msg = ex.Message;
                        res.StatusCode = (int)HttpStatusCode.InternalServerError;
                    }
                }
                else if (query.Contains("action"))
                {
                    if (device == null)
                    {
                        msg = "Device must be initialized before using";
                        res.StatusCode = (int)HttpStatusCode.BadRequest;
                    }
                    else
                    {
                        Match match = Regex.Match(query, "action=(.+)");
                        string action = match.Groups[1].Captures[0].ToString();
                        if (action.Equals("move_left"))
                        {
                            device.Move(-1, 0);
                        }
                        else if (action.Equals("move_right"))
                        {
                            device.Move(1, 0);
                        }
                        else if (action.Equals("move_up"))
                        {
                            device.Move(0, -1);
                        }
                        else if (action.Equals("move_down"))
                        {
                            device.Move(0, 1);
                        }
                        else if(action.Equals("zoom_in"))
                        {
                            device.Zoom(1);
                        }
                        else if (action.Equals("zoom_out"))
                        {
                            device.Zoom(-1);
                        }
                        else
                        {
                            msg = "Unsupported action: " + action;
                            res.StatusCode = (int)HttpStatusCode.BadRequest;
                        }
                    }
                }
                else
                {
                    msg = "Invalida request";
                    res.StatusCode = (int)HttpStatusCode.BadRequest;
                }

                string json = "{\"message\":\"" + msg + "\", \"query\":\"" + req.QueryString + "\"}";
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
