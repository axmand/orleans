using GrainImplement.WMS.Cache;
using GrainImplement.WMS.Util;
using GrainInterface.WMS;
using Orleans.Providers;
using System.Threading.Tasks;

namespace GrainImplement.WMS.Service
{
    [StorageProvider(ProviderName = "MongoDBStore")]
    public class WMSGrain: Orleans.Grain<ImagePNG>, IWMS
    {
        Task<string> IWMS.GetTileImagePNG(int x, int y, int z)
        {
            State.x = 1;
            return Task.FromResult("");
        }

        /// <summary>
        /// 检查缓存
        /// </summary>
        /// <param name="forceUpdate"></param>
        /// <returns></returns>
       Task<bool> IWMS.CheckCache(bool forceUpdate)
        {
            Helper.CacheTMS(@"D:\Share\TMS\");
            return Task.FromResult(true);
        }

    }
}
