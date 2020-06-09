using Client.HanYangYun.Util;
using GrainInterface.WMS;
using ServiceStack;
using System.IO;

namespace Client.HanYangYun.Services
{
    public class WMSRestService : Service
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
                IWMS wms = Helper.GetGrain<IWMS>(0);
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
