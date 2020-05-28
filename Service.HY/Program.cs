using Client.HY.Util;
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
        static void Main(string[] args)
        {       
            //启动服务
            Helper.Start();

            Helper.client = new ClientBuilder()
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

            //启动客户端
            Helper.client.Connect().Wait();

            Console.ReadKey();

        }
    }
}
