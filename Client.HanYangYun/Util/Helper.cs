using Microsoft.Extensions.DependencyInjection;
using Orleans;
using ServiceStack;
using System;

namespace Client.HanYangYun.Util
{
    public class Helper
    {
        /// <summary>
        /// 
        /// </summary>
        public const string _url = "http://*:1338/";

        public static IServiceCollection service = new ServiceCollection();

        public static ServiceProvider provider;

        public static T GetGrain<T>(int number) where T: Orleans.IGrainWithIntegerKey
        {
            return Helper.provider.GetRequiredService<IOrleansClient>().GetGrain<T>(number);
        }

        /// <summary>
        /// 
        /// </summary>
        public static AppHost _appHost;

        public static void Start()
        {
            _appHost = new AppHost();
            _appHost.Init();
            _appHost.Start(_url);
        }

        /// <summary>
        /// 延迟执行某个操作
        /// </summary>
        /// <param name="delayTime"></param>
        /// <param name="action"></param>
        public static void DelayInvoke(double delayTime, Action action)
        {
            System.Timers.Timer timer = new System.Timers.Timer(delayTime);
            timer.Elapsed += delegate (object sender, System.Timers.ElapsedEventArgs e)
            {
                timer.Enabled = false;
                action();
            };
            timer.Enabled = true;
        }
    }
}
