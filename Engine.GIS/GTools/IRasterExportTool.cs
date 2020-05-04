using OSGeo.GDAL;

namespace Engine.GIS.GTools
{
    public interface IRasterExportTool : IRasterTool
    {
        /// <summary>
        /// prepare for export raster
        /// </summary>
        /// <returns></returns>
        GRasterExportTool Prepare();
        /// <summary>
        /// combine bands
        /// </summary>
        /// <param name="outputBuffer"></param>
        /// <returns></returns>
        GRasterExportTool CombineBand(double[] outputBuffer);
        /// <summary>
        /// export raster layer
        /// </summary>
        /// <param name="nGeoTrans"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="fullFilename"></param>
        /// <param name="dateType"></param>
        void Export(double[] nGeoTrans, int width, int height, string fullFilename, DataType dateType = DataType.GDT_CFloat32);
    }
}
