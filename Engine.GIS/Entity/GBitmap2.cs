using Engine.GIS.GLayer.GRasterLayer;
using System.Drawing;

namespace Engine.GIS.Entity
{
    public class Bitmap2
    {
        public Bitmap2(Bitmap bmp = null, string name = "", string dec = "", GRasterLayer gdalLayer = null, GRasterBand gdalBand = null)
        {
            BMP = bmp;
            Name = name;
            Dec = dec;
            GdalBand = gdalBand;
            GdalLayer = gdalLayer;
        }

        /// <summary>
        /// bitmap原始数据
        /// </summary>
        public Bitmap BMP { get; set; }

        /// <summary>
        /// 图片名
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 图片描述
        /// </summary>
        public string Dec { get; }

        public GRasterLayer GdalLayer { get; }

        public GRasterBand GdalBand { get; set; }
    }
}
