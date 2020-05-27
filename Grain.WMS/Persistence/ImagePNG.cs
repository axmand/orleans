using Orleans.Providers;

namespace GrainImplement.WMS.Cache
{
    public  class ImagePNG
    {
        /// <summary>
        /// 行号
        /// </summary>
        public int x { get; set; }

        /// <summary>
        /// 列号
        /// </summary>
        public int y { get; set; }

        /// <summary>
        /// 缩放等级
        /// </summary>
        public int z { get; set; }
    }
}
