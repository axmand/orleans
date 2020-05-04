using System.Collections.Generic;
using System.Drawing;

namespace Engine.GIS.GTools
{
    public interface IRasterBandStatisticTool : IRasterBandTool
    {
        /// <summary>
        /// static raw value query table (convert one-dim to [x,y] two dim]
        /// </summary>
        float[,] StatisticalRawQueryTable { get; }

        /// <summary>
        /// static raw value graph
        /// </summary>
        Dictionary<int, List<Point>> StaisticalRawGraph { get; }

        /// <summary>
        /// 
        /// </summary>
        Dictionary<double, int> StaisticalRawPixelGraph { get; }
    }
}
