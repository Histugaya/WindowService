﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Extractor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            Service1 service = new Service1();
            var args = Environment.GetCommandLineArgs();
            if (args.Any(x => x.Equals("/DebugInConsole", StringComparison.OrdinalIgnoreCase)))
            {
                service.StartServiceConsole();
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                new Service1()
                };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
