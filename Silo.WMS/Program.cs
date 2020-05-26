using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.Threading.Tasks;

namespace Silo.WMS
{
    class Program
    {
        static int Main(string[] args)
        {
            return RunMainAsync().Result;
        }

        static async Task<int> RunMainAsync()
        {
            try
            {
                var host = await StartSilo();
                Console.WriteLine("\n\n Press Enter to terminate...\n\n");
                Console.ReadLine();
                await host.StopAsync();
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 1;
            }
        }

        static async Task<ISiloHost> StartSilo()
        {
            // define the cluster configuration
            ISiloHostBuilder builder = new SiloHostBuilder()
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "OrleansBasics";
                })
                .ConfigureLogging(logging => logging.AddConsole());
            //ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(GrainImplement.WMS.WMS).Assembly).WithReferences())
            var host = builder.Build();
            await host.StartAsync();
            return host;
        }
    }
}
