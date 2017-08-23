using System;

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
        }
    }
}
