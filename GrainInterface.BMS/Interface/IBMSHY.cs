using Orleans;
using System.Threading.Tasks;

namespace GrainInterface.BMS
{
    /// <summary>
    /// 汉阳云地图业务处理逻辑
    /// </summary>
    public interface IBMSHY: IGrainWithIntegerKey
    {
        /// <summary>
        /// 系统初始化校验 
        /// </summary>
        /// <returns></returns>
        Task<bool> InitialCheck();

        /// <summary>
        /// 土地信息
        /// </summary>
        /// <returns></returns>
        Task<string> AccessTDXXGeoData();

        /// <summary>
        /// 楼宇信息
        /// </summary>
        /// <returns></returns>
        Task<string> AccessLYXXGeoData();

        /// <summary>
        /// 修改土地信息
        /// </summary>
        /// <param name="geoJsonText"></param>
        /// <returns></returns>
        Task<string> GeoDataTDXXUpdate(string geoJsonText);

        /// <summary>
        /// 修改楼宇信息
        /// </summary>
        /// <param name="geoJsonText"></param>
        /// <returns></returns>
        Task<string> GeoDataLYXXUpdate(string geoJsonText);
    }
}
