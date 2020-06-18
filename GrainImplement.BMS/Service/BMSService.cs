using System.Threading.Tasks;
using GrainInterface.BMS;

namespace GrainImplement.BMS.Service
{
    public class BMSService : Orleans.Grain, IBMS
    {
        Task<bool> IBMS.InitialCheck()
        {
            return Task.FromResult(true);
        }
    }
}
