using Engine.GIS.GLayer.VectorLayer;

namespace Engine.GIS.GTools
{
    public interface IShapeTranslateTool
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="featureLayer"></param>
        void Visit(GFeatureLayer featureLayer);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="outputFilename"></param>
        /// <returns></returns>
        bool TranslateToGeoJson(string outputFilename);
    }
}
