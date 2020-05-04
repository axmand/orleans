using Engine.GIS.GLayer.GRasterLayer;
using OSGeo.GDAL;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Engine.GIS.GTools
{
    /// <summary>
    /// raster tool
    /// </summary>
    public interface IRasterTool: ITool, IDisposable { }

    /// <summary>
    /// raster band tool(aim band)
    /// base interface
    /// </summary>
    public interface IRasterBandTool : IRasterTool
    {
        void Visit(GRasterBand pBand);
    }

    /// <summary>
    /// base interface for raster layer operation
    /// </summary>
    public interface IRasterLayerTool : IRasterTool
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pLayer"></param>
        void Visit(GRasterLayer pLayer);
    }
}
