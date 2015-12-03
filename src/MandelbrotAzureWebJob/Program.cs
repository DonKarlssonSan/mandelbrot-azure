using Microsoft.Azure;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.ServiceBus;
using System.Configuration;

namespace MandelbrotAzureWebJob
{
    public class Program
    {
        public static void Main()
        {
            var config = new ServiceBusConfiguration();
            config.ConnectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");

            var jobHostConfiguration = new JobHostConfiguration();
            jobHostConfiguration.UseServiceBus(config);

            var storage = ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ConnectionString;
            var dashboard = ConfigurationManager.ConnectionStrings["AzureWebJobsDashboard"].ConnectionString;
            jobHostConfiguration.StorageConnectionString = storage;
            jobHostConfiguration.DashboardConnectionString = dashboard;
            var host = new JobHost(jobHostConfiguration);

            host.RunAndBlock();
        }
    }
}
