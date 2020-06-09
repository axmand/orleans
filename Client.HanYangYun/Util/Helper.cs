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

        /// <summary>
        /// 
        /// </summary>
        public static IClusterClient client;

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
