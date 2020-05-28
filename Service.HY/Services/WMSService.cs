using Client.HY.Util;
using GrainInterface.WMS;
using Orleans.CodeGeneration;
using ServiceStack;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

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
            IWMS wms = Helper.client.GetGrain<IWMS>(0);
            Stream sm  =  wms.GetTileImagePNG(request.x, request.y, request.z).Result;
            return sm;
        }

    }
}
