using MandelbrotAzure.Common;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.WindowsAzure;
using System;

namespace MandelbrotAzureTrigger
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new ServiceBusConfiguration();
            config.ConnectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");

            var jobHostConfiguration = new JobHostConfiguration();
            jobHostConfiguration.UseServiceBus(config);
            
            var host = new JobHost(jobHostConfiguration);
            host.Call(typeof(Program).GetMethod("DoWork"));
            host.Start();
        }

        [NoAutomaticTrigger]
        public static void DoWork([ServiceBus("jobs")] Zoom outMsg)
        {
            Console.WriteLine("Manual job started. Writing to job queue.");
            outMsg = new Zoom()
            {
                xmin = 1,
                xmax = 2,
                ymin = 3,
                ymax = 4
            };
        }
    }
}
