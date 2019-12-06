using System;
using System.ServiceProcess;
using System.Timers;
using Extractor.Controller;
using System.Configuration;

namespace Extractor
{
    public partial class Service1 : ServiceBase
    {
        Timer oTimer = null;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            double interval = double.Parse(ConfigurationManager.AppSettings["Interval"]);

            try
            {
                new Processor().SendNotification("Service has been started");
                //sched.Start();
                oTimer = new System.Timers.Timer(interval);
                oTimer.AutoReset = true;
                oTimer.Enabled = true;
                oTimer.Start();
                oTimer.Elapsed += new System.Timers.ElapsedEventHandler(oTimer_Elapsed);
            }
            catch (Exception ex)
            {
                Log.CatchErrors(ex);
            }
        }

        private void oTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            new Processor().ExtractFiles();
        }

        protected override void OnStop()
        {
            new Processor().SendNotification("Service has been stopped");
        }

        internal void StartServiceConsole()
        {
            new Processor().ExtractFiles();
        }
    }
}
