using Engine.Facility.EResponse;
using Microsoft.Extensions.DependencyInjection;
using Orleans;
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
            return provider.GetRequiredService<IOrleansClient>().GetGrain<T>(number);
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

        #region 服务器应答

        public static string AbnormalError = new FailResponse("服务异常").ToString();

        public static string PermessionError = new FailResponse("接口需要配置权限").ToString();

        public static string DataError = new FailResponse("数据错误").ToString();

        #endregion

    }
}
