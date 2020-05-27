using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace GrainImplement.WMS.Util
{
    /// <summary>
    /// 瓦片缓存
    /// </summary>
    public class TileCache
    {
        /// <summary>
        /// 
        /// </summary>
        static Dictionary<string, Image> _cache = new Dictionary<string, Image>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dir"></param>
        public static void CacheImageStream(string id, string dir)
        {
            string[] ids = id.Split('_');
            if (!_cache.ContainsKey(id))
            {
                string fullTilename = dir + ids[0] + @"\" + ids[1] + @"\" + ids[2] + ".png";
                Image img = Image.FromFile(fullTilename, false);
                _cache[id] = img;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Image GetImageStreamById(string id)
        {
            return _cache.ContainsKey(id) ? _cache[id] : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                { return codec; }
            }
            return null;
        }
    }
}
