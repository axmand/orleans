using GrainInterface.WMS;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using System;
using System.Timers;

namespace Client.HY
{
    class Program
    {
        static IClusterClient client;

        static void Main(string[] args)
        {

            client = new ClientBuilder()
               .ConfigureApplicationParts(options =>
               {
                   options.AddApplicationPart(typeof(IWMS).Assembly);
               })
               .Configure<ClusterOptions>(options =>
               {
                   options.ClusterId = "WMSCluster";
                   options.ServiceId = "WMSCluster";
               })
               .UseLocalhostClustering()
               .ConfigureLogging(logging => logging.AddConsole())
               .Build();

            client.Connect().Wait();

            Timer t = new Timer();
            t.Elapsed += T_Elapsed;
            t.Interval = 5000;
            t.Start();


            Console.ReadKey();
        }

        private async static void T_Elapsed(object sender, ElapsedEventArgs e)
        {
            var vacationEmployee = client.GetGrain<IWMS>(0);
            var sm = await vacationEmployee.GetTileImagePNG(1, 0, 1);
            
            //var vacationEmployeeId = vacationEmployee.UpdateCache(true);
        }
    }
}
