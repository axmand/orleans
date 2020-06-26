using Microsoft.AspNetCore.Http.Features;
using System;

namespace GrainImplement.BMS.Persistence
{
    /// <summary>
    /// 矢量对象缓存
    /// </summary>
    [Serializable]
    public class VOCache
    {
        /// <summary>
        /// 原始文本
        /// </summary>
        public string GEOJSON{ get; set; }

        /// <summary>
        /// 
        /// </summary>
        public FeatureCollection GetVO()
        {
            NetTopologySuite.IO.GeoJsonReader reader = new NetTopologySuite.IO.GeoJsonReader();
            FeatureCollection result = reader.Read<FeatureCollection>(GEOJSON);
            return result;
        }
    }
}
