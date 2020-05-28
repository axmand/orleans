using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GrainImplement.WMS.Util
{

    /// <summary>
    /// loading percentage
    /// </summary>
    /// <param name="percentage"></param>
    public delegate void CachedProcessEventHandler(int x, int y, int z, string pngId);

    public class Helper
    {
        /// <summary>
        /// 
        /// </summary>
        static string connectString = "mongodb://localhost:27017";

        /// <summary>
        /// 报告缓存进度
        /// </summary>
        public static event CachedProcessEventHandler OnCacheProcess;

        /// <summary>
        /// 
        /// </summary>
        static readonly Dictionary<string, Image> _cache = new Dictionary<string, Image>();

        /// <summary>
        /// 
        /// </summary>
        static readonly IMongoDatabase db = new MongoClient(connectString).GetDatabase("WMS");

        /// <summary>
        /// 
        /// </summary>
        static readonly IGridFSBucket bucket = new GridFSBucket(db, new GridFSBucketOptions
        {
            BucketName = "TMS",
            ChunkSizeBytes = 1048576 // 1MB
        });

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Task<ObjectId> UploadFromBytes(string fileName, byte[] source)
        {
            //构建查询，用于检查是否已存在
            FilterDefinition<GridFSFileInfo> filter = Builders<GridFSFileInfo>.Filter.And(
                Builders<GridFSFileInfo>.Filter.Eq(x => x.Filename, fileName)
                //Builders<GridFSFileInfo>.Filter.Gte(x => x.UploadDateTime, new DateTime(2015, 1, 1, 0, 0, 0, DateTimeKind.Utc)),
                );
            //排序规则
            SortDefinition<GridFSFileInfo> sort = Builders<GridFSFileInfo>.Sort.Descending(x => x.UploadDateTime);
            //查找限制，提高处理速度
            GridFSFindOptions options = new GridFSFindOptions{
                Limit = 1,
                Sort = sort
            };
            using (var cursor = bucket.Find(filter, options))
            {
                GridFSFileInfo fileInfo = cursor.ToList().FirstOrDefault();
                return fileInfo == null ? Task.FromResult(bucket.UploadFromBytesAsync(fileName, source).Result) : Task.FromResult(fileInfo.Id);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pngId"></param>
        /// <returns></returns>
        public async static Task<MemoryStream> QueryBitmap(string pngId)
        {
            try
            {
                byte[] buffer = await bucket.DownloadAsBytesAsync(new ObjectId(pngId));
                MemoryStream m = new MemoryStream(buffer);
                return m;
            }
            catch
            {
                byte[] buffer = new byte[] { 0 };
                MemoryStream m = new MemoryStream(buffer);
                return m;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dir"></param>
        public static byte[] GetFileByteBufer(string id, string dir)
        {
            string[] ids = id.Split('_');
            string fullTilename = dir + ids[0] + @"\" + ids[1] + @"\" + ids[2] + ".png";
            using (Image image = Image.FromFile(fullTilename))
            using (MemoryStream m = new MemoryStream())
            {
                image.Save(m, image.RawFormat);
                byte[] imageBytes = m.ToArray();
                return imageBytes;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tmsDir"></param>
        public static void StartCache(string tmsDir)
        {
            //1.遍历dir文件夹
            List<string> zdirs = Directory.GetDirectories(tmsDir).ToList();
            //2.遍历子文件夹
            zdirs.ForEach(zdir =>
            {
                List<string> xdirs = Directory.GetDirectories(zdir).ToList();
                xdirs.ForEach(xdir =>
                {
                    List<string> yfiles = Directory.GetFiles(xdir).ToList();
                    yfiles.ForEach(yfile =>
                    {
                        int z = Convert.ToInt32(Path.GetFileName(zdir)), x = Convert.ToInt32(Path.GetFileNameWithoutExtension(xdir)), y = Convert.ToInt32(Path.GetFileNameWithoutExtension(yfile));
                        string id = string.Format("{0}_{1}_{2}", z, x, y);
                        byte[] source = GetFileByteBufer(id, tmsDir);
                        ObjectId objectId = UploadFromBytes(id, source).Result;
                        OnCacheProcess?.Invoke(x, y, z, objectId.ToString());
                    });
                });
            });
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
