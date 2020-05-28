using GrainInterface.WMS;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System;

namespace Silo.WMS
{
    class Program
    {

        const string connectionString = "mongodb://localhost";

        static void Main()
        {
            ISiloHost build = new SiloHostBuilder()
                //配置数据库持久化
                .UseMongoDBClient(connectionString)
                //"WMSTileCache" 内部编号
                .AddMongoDBGrainStorage("WMSTileCache", options =>
                {
                    //在MongoDB中构建的数据库名字
                    options.DatabaseName = "WMS";
                    options.CreateShardKeyForCosmos = false;
                    options.ConfigureJsonSerializerSettings = settings =>
                    {
                        settings.NullValueHandling = NullValueHandling.Include;
                        settings.ObjectCreationHandling = ObjectCreationHandling.Replace;
                        settings.DefaultValueHandling = DefaultValueHandling.Populate;
                    };
                })
                .AddMongoDBGrainStorage("WMSLogCache", options =>
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
                //配置集群
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "WMSCluster";
                    options.ServiceId = "WMSCluster";
                })
                .ConfigureLogging(logging => logging.AddConsole())
                .Build();
            //
            build.StartAsync().Wait();
            Console.ReadKey();
        }
    }
}
