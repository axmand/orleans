using Client.HanYangYun.Util;
using GrainInterface.BMS;
using GrainInterface.CMS;
using GrainInterface.WMS;
using Microsoft.Extensions.DependencyInjection;
using Orleans;
using System;

namespace Client.HanYangYun
{
    class Program
    {
        static void Main(string[] args)
        {
            //启动服务
            Helper.Start();
            //启动集群链接
            Helper.DelayInvoke(6000, () =>{
                Helper.service.AddLogging();
                Helper.service.AddOrleansMultiClient(build => {
                    //silo 1
                    build.AddClient(opt => {
                        opt.ServiceId = "dev1";
                        opt.ClusterId = "App";
                        opt.SetServiceAssembly(typeof(IWMS).Assembly);
                        opt.Configure = (o =>
                        {
                            o.UseLocalhostClustering(gatewayPort: 30001);
                        });
                    });
                    //silo 2
                    build.AddClient(opt => {
                        opt.ServiceId = "dev2";
                        opt.ClusterId = "App";
                        opt.SetServiceAssembly(typeof(ICustomer).Assembly);
                        opt.Configure = (o =>
                        {
                            o.UseLocalhostClustering(gatewayPort: 30002);
                        });
                    });
                    //silo3
                    build.AddClient(opt => {
                        opt.ServiceId = "dev3";
                        opt.ClusterId = "App";
                        opt.SetServiceAssembly(typeof(IBMSHY).Assembly);
                        opt.Configure = (o =>
                        {
                            o.UseLocalhostClustering(gatewayPort: 30003);
                        });
                    });
                });
                Helper.provider = Helper.service.BuildServiceProvider();
                Console.WriteLine(string.Format("{0}: {1}", DateTime.Now.ToLongTimeString(), "clustion connection successful..."));
                //CMS系统初始化校验
                bool cms_check = Helper.GetGrain<ICustomer>(0).InitialCheck().Result;
                Console.WriteLine(string.Format("{0}: CMS module passed check : {1} !", DateTime.Now.ToLongTimeString(), cms_check));
                //WMS系统初始化校验
                bool wms_check = Helper.GetGrain<IWMS>(0).InitialCheck().Result;
                Console.WriteLine(string.Format("{0}: WMS module passed check : {1} !", DateTime.Now.ToLongTimeString(), wms_check));
                //BMS系统初始化校验
                bool bms_check = Helper.GetGrain<IWMS>(0).InitialCheck().Result;
                Console.WriteLine(string.Format("{0}: BMS module passed check : {1} !", DateTime.Now.ToLongTimeString(), bms_check));
            });
            //阻止客户端退出
            Console.ReadKey();
        }
    }
}
