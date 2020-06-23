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
        Task<string> AccessToLandInformation();

        /// <summary>
        /// 楼宇信息
        /// </summary>
        /// <returns></returns>
        Task<string> AccessToBuildingInformation();

        /// <summary>
        /// 修改土地信息
        /// </summary>
        /// <param name="geoJsonText"></param>
        /// <returns></returns>
        Task<string> TDXXGeoDataUpdate(string geoJsonText);

        /// <summary>
        /// 修改楼宇信息
        /// </summary>
        /// <param name="geoJsonText"></param>
        /// <returns></returns>
        Task<string> LYXXGeoDataUpdate(string geoJsonText);
    }
}
