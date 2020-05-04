using System.Collections.Generic;

namespace Engine.GIS.GTools
{
    /// <summary>
    /// raster layer tool(aim layer), base interface
    /// </summary>
    public interface IRasterBandCursorTool : IRasterBandTool
    {
        /// <summary>
        /// 
        /// </summary>
        IEnumerable<(int, int, double)> ValidatedRawCollection { get; }

        /// <summary>
        /// 
        /// </summary>
        IEnumerable<(int, int, double)> ValidatedNormalCollection { get; }

        /// <summary>
        /// pick raw value
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        float PickRawValue(int x, int y);

        /// <summary>
        /// pick normalized value
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        float PickNormalValue(int x, int y);

        /// <summary>
        /// pick value by conv
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        float[] PickRangeNormalValue(int x, int y, int row = 5, int col = 5);

        /// <summary>
        /// pick raw value by cov
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        float[] PickRangeRawValue(int x, int y, int row = 5, int col = 5);
    }
}
