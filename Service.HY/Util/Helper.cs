using Orleans;

namespace Client.HY.Util
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

    }
}
