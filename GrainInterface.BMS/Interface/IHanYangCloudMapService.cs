using Orleans;
using System.Threading.Tasks;

namespace GrainInterface.BMS
{
    /// <summary>
    /// 汉阳云地图业务处理逻辑
    /// </summary>
    public interface IHanYangCloudMapService: IGrainWithIntegerKey
    {
        /// <summary>
        /// 系统初始化校验 
        /// </summary>
        /// <returns></returns>
        Task<bool> InitialCheck();
    }
}
