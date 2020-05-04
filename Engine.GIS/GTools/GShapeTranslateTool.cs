using Engine.GIS.GLayer.VectorLayer;
using OSGeo.OGR;

namespace Engine.GIS.GTools
{
    public class GShapeTranslateTool: IShapeTranslateTool
    {
        GFeatureLayer _layer;
        public void Visit(GFeatureLayer featureLayer)
        {
            _layer = featureLayer;
        }

        public bool TranslateToGeoJson(string outputFilename)
        {
            OSGeo.OGR.Driver dv = Ogr.GetDriverByName("GeoJSON");
            if (dv == null || _layer.DS == null) return false;
            dv.CopyDataSource(_layer.DS, outputFilename, null);
            return true;
        }

    }
}
