using Funq;
using ServiceStack;
using ServiceStack.WebHost.Endpoints;

namespace Client.HanYangYun.Util
{
    /// <summary>
    /// Rest服务
    /// </summary>
    public class AppHost : AppHostHttpListenerBase
    {
        /// <summary>
        /// 
        /// </summary>
        public AppHost() : base("Services", typeof(Program).Assembly) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        public override void Configure(Container container)
        {
            //跨域
            //this.Plugins.Add(new CorsFeature());
            base.SetConfig(new EndpointHostConfig()
            {
                //DebugMode = true,
                AllowJsonpRequests = true,
                //EnableFeatures = Feature.All.Remove(Feature.Html),
                AllowAclUrlReservation = true,
                EnableAccessRestrictions = true,
                GlobalResponseHeaders =
                {
                    { "Access-Control-Allow-Origin", "*" },
                    { "Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS" },
                    { "Access-Control-Allow-Headers", "Content-Type" },
                },
            });
        }
    }
}
