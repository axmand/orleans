using Orleans;
using System.Threading.Tasks;

namespace GrainInterface.BMS
{
    public interface IBMS: IGrainWithIntegerKey
    {
        /// <summary>
        /// 系统初始化校验 
        /// </summary>
        /// <returns></returns>
        Task<bool> InitialCheck();
    }
}
