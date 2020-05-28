using GrainImplement.WMS.Cache;
using GrainImplement.WMS.Util;
using GrainInterface.WMS;
using Orleans.Runtime;
using System.IO;
using System.Threading.Tasks;

namespace GrainImplement.WMS.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class WMS : Orleans.Grain, IWMS
    {
        readonly IPersistentState<TileSchema> _tileCache;

        /// <summary>
        /// “tilecache”在数据库WMS中构建的表名，对应的是GrainStorage中的WMSTileCache
        /// </summary>
        /// <param name="tileCache"></param>
        public WMS([PersistentState("TileCache", "WMSTileCache")] IPersistentState<TileSchema> tileCache)
        {
            _tileCache = tileCache;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        async Task<Stream> IWMS.GetTileImagePNG(int x, int y, int z)
        {
            await _tileCache.ReadStateAsync();
            TileCache cache = _tileCache.State.TILECACHE.Find(p => p.x ==x && p.y == y && p.z == z);
            Stream sm = await Helper.QueryBitmap(cache.pngId);
            //await WriteStateAsync();
            return sm;
        }

        /// <summary>
        /// 检查缓存
        /// </summary>
        /// <param name="forceUpdate"></param>
        /// <returns></returns>
        async Task<bool> IWMS.UpdateCache(bool forceUpdate)
        {
            await _tileCache.ReadStateAsync();
            if (forceUpdate)
            {
                Helper.OnCacheProcess += (int x, int y, int z, string pngId) =>
                {
                    if (!_tileCache.State.TILECACHE.Exists(t => t.pngId == pngId))
                    {
                        _tileCache.State.VERSION++;
                        _tileCache.State.TILECACHE.Add(new TileCache()
                        {
                            pngId = pngId,
                            x = x,
                            y = y,
                            z = z
                        });
                    }
                };
                Helper.StartCache(@"D:\Share\TMS\");
            }
            await _tileCache.WriteStateAsync();
            return true;
        }

    }
}
