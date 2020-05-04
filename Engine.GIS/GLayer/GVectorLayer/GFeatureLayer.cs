using OSGeo.GDAL;
using OSGeo.OGR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.GIS.GLayer.VectorLayer
{
    /// <summary>
    /// 矢量图层，可以基于GridLayer切割Feature和FeatureCollection，构建矢量金字塔
    /// </summary>
    public class GFeatureLayer
    {
        public DataSource DS {get;}

        public GFeatureLayer(string shpFilename)
        {
            //注册gdal库
            Gdal.AllRegister();
            Ogr.RegisterAll();
            //
            DS = Ogr.Open(shpFilename, 0);
        }

    }
}
