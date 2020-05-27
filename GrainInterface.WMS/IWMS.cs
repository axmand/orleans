using Orleans;
using System.Threading.Tasks;

namespace GrainInterface.WMS
{
    public interface IWMS : IGrainWithIntegerKey
    {
        /// <summary>
        /// 通过TMS获取瓦片
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        Task<string> GetTileImagePNG(int x, int y, int z);

        /// <summary>
        /// 检查chache是否完成
        /// </summary>
        /// <returns></returns>
        Task<bool> CheckCache(bool forceUpdate = false);
    }
}
