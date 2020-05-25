using GrainInterface.WMS;
using System.Threading.Tasks;

namespace GrainImplement.WMS
{
    public class WMS: Orleans.Grain, IWMS
    {
        Task<string> IWMS.GetTileImagePNG(int x, int y, int z)
        {
            return Task.FromResult("");
        }
    }
}
