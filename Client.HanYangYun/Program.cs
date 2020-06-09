using Client.HanYangYun.Util;
using GrainInterface.CMS;
using GrainInterface.WMS;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using System;
using System.Net;

namespace Client.HanYangYun
{
    class Program
    {
        static void Main(string[] args)
        {       
            //启动服务
            Helper.Start();
            //集群地址配置
            IPEndPoint[] gateways = new IPEndPoint[]{
                new IPEndPoint(IPAddress.Loopback, 30000),
                //new IPEndPoint(IPAddress.Loopback, 30001)
            };
            //构造集群客户端
            Helper.client = new ClientBuilder()
                .UseStaticClustering(gateways)
               .ConfigureApplicationParts(options =>
               {
                   //options.AddApplicationPart(typeof(IWMS).Assembly);
                   options.AddApplicationPart(typeof(ICMS).Assembly);
               })
               .Configure<ClusterOptions>(options =>
               {
                   options.ClusterId = "Client_dev";
                   options.ServiceId = "ClientCluster";
               })
               .ConfigureLogging(logging => logging.AddConsole())
               .Build();
            //延迟启动客户端
            Helper.DelayInvoke(6000, () =>{
                Helper.client.Connect().Wait();
            });
            //组织客户端退出
            Console.ReadKey();


        }
    }
}
