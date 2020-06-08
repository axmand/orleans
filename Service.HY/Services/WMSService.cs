using Client.HY.Util;
using GrainInterface.WMS;
using ServiceStack;
using System.IO;

namespace Client.HY.Services
{
    public class WMSService : Service
    {
        /// <summary>
        /// 瓦片请求服务
        /// </summary>
        /// <param name="request"></param>
        [AddHeader(ContentType = "image/png")]
        public Stream Get(Routes.GoogleLayer request)
        {
            try
            {
                IWMS wms = Helper.client.GetGrain<IWMS>(0);
                Stream sm = wms.GetTileImagePNG(request.x, request.y, request.z).Result;
                return sm;
            }
            catch
            {
                return null;
            }
        }
    }
}
