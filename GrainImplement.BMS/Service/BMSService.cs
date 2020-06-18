using System.Threading.Tasks;
using GrainInterface.BMS;

namespace GrainImplement.BMS.Service
{
    public class BMSService : Orleans.Grain, IHanYangCloudMapService
    {
        Task<bool> IHanYangCloudMapService.InitialCheck()
        {
            return Task.FromResult(true);
        }
    }
}
