using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.Net;

namespace Silo.CMS
{
    class Program
    {

        const string connectionString = "mongodb://localhost";

        static void Main(string[] args)
        {
            ISiloHost build = new SiloHostBuilder()
             //使用本地化构造，注意集群启动必须有不同的 SiloPort, GatewayPort, 默认 11111, 30000
             .UseLocalhostClustering(siloPort: 11113, gatewayPort: 30002)
             //配置数据库持久化
             .UseMongoDBClient(connectionString)
             .AddMongoDBGrainStorage("CustomerManagerCache", options =>
             {
                    //在MongoDB中构建的数据库名字
                 options.DatabaseName = "CMS";
                 options.CreateShardKeyForCosmos = false;
                 options.ConfigureJsonSerializerSettings = settings =>
                 {
                     settings.NullValueHandling = NullValueHandling.Include;
                     settings.ObjectCreationHandling = ObjectCreationHandling.Replace;
                     settings.DefaultValueHandling = DefaultValueHandling.Populate;
                 };
             })
             .AddMongoDBGrainStorage("GroupManagerCache", options =>
             {
                 options.DatabaseName = "CMS";
                 options.CreateShardKeyForCosmos = false;
                 options.ConfigureJsonSerializerSettings = settings =>
                 {
                     settings.NullValueHandling = NullValueHandling.Include;
                     settings.ObjectCreationHandling = ObjectCreationHandling.Replace;
                     settings.DefaultValueHandling = DefaultValueHandling.Populate;
                 };
             })
             //配置集群
             .Configure<EndpointOptions>(options => options.AdvertisedIPAddress = IPAddress.Loopback)
             .Configure<ClusterOptions>(options =>
             {
                 options.ClusterId = "dev";
                 options.ServiceId = "Cluster";
             })
             .ConfigureLogging(logging => logging.AddConsole())
             .Build();
            //
            build.StartAsync().Wait();
            Console.ReadKey();
        }
    }
}
