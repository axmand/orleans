using GrainImplement.WMS.Service;
using GrainInterface.WMS;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.Net;

namespace Silo.WMS
{
    class Program
    {

        const string connectionString = "mongodb://localhost/WMS";

        static void Main()
        {
            ISiloHost build = new SiloHostBuilder()
                .UseMongoDBClient(connectionString)
                .UseMongoDBClustering(options =>
                {
                    options.DatabaseName = "WMS";
                    options.CreateShardKeyForCosmos = false;
                })
                .AddStartupTask(async (provider, ct) =>
                {
                    IGrainFactory grainFactory =provider.GetRequiredService<IGrainFactory>();
                    await grainFactory.GetGrain<IWMS>((int)DateTime.UtcNow.TimeOfDay.Ticks).CheckCache(true);
                })
                .UseMongoDBReminders(options =>
                {
                    options.DatabaseName = "WMS";
                    options.CreateShardKeyForCosmos = false;
                })
                .AddMongoDBGrainStorage("MongoDBStore", options =>
                {
                    options.DatabaseName = "WMS";
                    options.CreateShardKeyForCosmos = false;

                    options.ConfigureJsonSerializerSettings = settings =>
                    {
                        settings.NullValueHandling = NullValueHandling.Include;
                        settings.ObjectCreationHandling = ObjectCreationHandling.Replace;
                        settings.DefaultValueHandling = DefaultValueHandling.Populate;
                    };
                })
                  .Configure<ClusterOptions>(options =>
                  {
                      options.ClusterId = "WMSCluster";
                      options.ServiceId = "WMSCluster";
                  })
                .ConfigureEndpoints(IPAddress.Loopback, 11111, 30000)
                .ConfigureLogging(logging => logging.AddConsole())
                .Build();
            //
            build.StartAsync().Wait();
            Console.ReadKey();
        }
    }
}
