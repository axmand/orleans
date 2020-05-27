using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GrainImplement.WMS.Util
{

    /// <summary>
    /// loading percentage
    /// </summary>
    /// <param name="percentage"></param>
    public delegate void CachedProcessEventHandler(double percentage);

    public class Helper
    {

        /// <summary>
        /// 报告缓存进度
        /// </summary>
        public static event CachedProcessEventHandler OnCacheProcess;

        public static void CacheTMS(string tmsDir)
        {
            //1.遍历dir文件夹
            List<string> zdirs = Directory.GetDirectories(tmsDir).ToList();
            int seed = 0;
            //2.遍历子文件夹
            zdirs.ForEach(zdir => {
                List<string> xdirs = Directory.GetDirectories(zdir).ToList();
                xdirs.ForEach(xdir => {
                    List<string> yfiles = Directory.GetFiles(xdir).ToList();
                    yfiles.ForEach(yfile => {
                        string id = string.Format("{0}_{1}_{2}", Path.GetFileName(zdir), Path.GetFileNameWithoutExtension(xdir), Path.GetFileNameWithoutExtension(yfile));
                        TileCache.CacheImageStream(id, tmsDir);
                    });
                });
                OnCacheProcess?.Invoke((double)++seed / zdirs.Count);
            });
        }

    }
}
