using System;
using System.Collections.Generic;

namespace GrainImplement.WMS.Cache
{

    [Serializable]
    public sealed class TileSchema
    {
        /// <summary>
        /// 
        /// </summary>
        public int VERSION { get; set; } = 0;

        /// <summary>
        /// 
        /// </summary>
        public List<TileCache> TILECACHE { get; set; } = new List<TileCache>();
    }

    [Serializable]
    public sealed  class TileCache
    {

        /// <summary>
        /// 目标对象id
        /// </summary>
        public string pngId { get; set; }

        public int z { get; set; }

        public int x { get; set; }

        public int y { get; set; }
    }
}
