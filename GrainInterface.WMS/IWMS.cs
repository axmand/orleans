using Orleans;
using System.IO;
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
        Task<Stream> GetTileImagePNG(int x, int y, int z);

        /// <summary>
        /// 检查chache是否完成
        /// </summary>
        /// <returns></returns>
        Task<bool> UpdateCache(bool forceUpdate = false);
    }
}
