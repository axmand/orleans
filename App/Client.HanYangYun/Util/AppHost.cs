using Funq;
using ServiceStack;
using ServiceStack.ServiceInterface.Cors;
using ServiceStack.WebHost.Endpoints;

namespace Client.HYY.Util
{
    /// <summary>
    /// Rest服务
    /// </summary>
    public class AppHost : AppHostHttpListenerBase
    {
        /// <summary>
        /// 
        /// </summary>
        public AppHost() : base("Services", typeof(Program).Assembly)
        {
            Plugins.Add(new CorsFeature(
                allowedOrigins: "*",
                allowedMethods: "GET, POST, PUT, DELETE, OPTIONS",
                allowedHeaders: "Content-Type",
                allowCredentials: false));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        public override void Configure(Container container)
        {
            //跨域
            base.SetConfig(new EndpointHostConfig()
            {

                DebugMode = true,
                //AllowJsonpRequests = true,
                //EnableFeatures = Feature.All.Remove(Feature.Html),
                //AllowAclUrlReservation = true,
                //EnableAccessRestrictions = true,
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
