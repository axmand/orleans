using Engine.GIS.GeoType;
using Engine.GIS.GProject;
using NetTopologySuite.Geometries;
using OSGeo.GDAL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace Engine.GIS.GOperator
{
    /// <summary>
    /// 矢量金字塔
    /// </summary>
    public class VectorPyramid
    {
        /// <summary>
        /// web墨卡托投影
        /// </summary>
        public WebMercatorProjection Projection { get; } = new WebMercatorProjection();

        /// <summary>
        /// 设置图像范围，上[3] 左[0] 
        /// </summary>
        public double[] GeoTransform { get; } = new double[6];
        /// <summary>
        /// GDAL Dataset
        /// </summary>
        public Dataset PDataSet { get; }

        public int BandCount { get; }

        // 原始图像东西方向像素分辨率
        public double src_w_e_pixel_resolution { get; }

        // 原始图像南北方向像素分辨率
        public double src_n_s_pixel_resolution { get; }

        public GBound Bound { get; }

        public VectorPyramid(string rasterFilename)
        {
            //注册gdal库
            Gdal.AllRegister();
            //只读方式读取图层
            PDataSet = Gdal.Open(rasterFilename, Access.GA_ReadOnly);
            //读取图像范围
            PDataSet.GetGeoTransform(GeoTransform);
            //波段数目
            BandCount = PDataSet.RasterCount;
            Coordinate c0 = new Coordinate(GeoTransform[0], GeoTransform[3]);
            Coordinate c1 = new Coordinate(
                GeoTransform[0] + GeoTransform[1] * PDataSet.RasterXSize + GeoTransform[2] * PDataSet.RasterYSize,
                GeoTransform[3] + GeoTransform[4] * PDataSet.RasterXSize + GeoTransform[5] * PDataSet.RasterYSize
                );
            //计算边界
            Bound = new GBound();
            var (x0, y0) = Projection.UnProject(c0.X, c0.Y);
            Bound.Extend(new Coordinate(x0, y0));
            var (x1,y1) = Projection.UnProject(c1.X, c1.Y);
            Bound.Extend(new Coordinate(x1, y1));
            //原始分辨率
            src_w_e_pixel_resolution = (Bound.Right - Bound.Left) / PDataSet.RasterXSize;
            src_n_s_pixel_resolution = (Bound.Top - Bound.Bottom) / PDataSet.RasterYSize;
            //构建瓦片层级
            for (int i = 14; i <= 15; i++)
                Build(Bound, i);
        }

        //这里面，xOff和yOff是指偏移量，即从影像的左上角起始坐标（xOff,yOff）开始读取数据。
        //xSize和ySize是指读取图像数据的行列数，即宽度和高度，单位都是像素。Buffer是图像数据缓存。
        //buf_xSize和buf_ySize是缓存区的大小，它们须与buffer申请的大小保持一致，通过这两个参数可以控制缩放，
        //如果它们小于xSize和ySize就是将原图缩小，反之如果它们大于xSize和ySize就是将原图放大。
        //pixelSpace和lineSpace一般默认取0即可。

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offsetX">指偏移量，即从影像的左上角起始坐标(offsetX, offsetY)</param>
        /// <param name="offsetY">指偏移量，即从影像的左上角起始坐标(offsetX, offsetY)</param>
        /// <param name="blockX">指读取原始图像数据的行列数，即宽度和高度，单位都是像素</param>
        /// <param name="blockY">指读取原始图像数据的行列数，即宽度和高度，单位都是像素</param>
        /// <param name="bufferX">指缓存区的大小，必须与buffer大小一致，通过这两个参数可以控制缩放</param>
        /// <param name="bufferY">指缓存区的大小，必须与buffer大小一致，通过这两个参数可以控制缩放</param>
        /// <param name="imageX">指写入时对图像的偏移，指瓦片与底图重叠点与瓦片左上角的距离</param>
        /// <param name="imageY">指写入时对图像的偏移，指瓦片与底图重叠点与瓦片左上角的距离</param>
        /// <param name="fullFilename">存放的地址</param>
        public void CratePNG(int offsetX, int offsetY, int blockX, int blockY, int bufferX, int bufferY, int imageX, int imageY, string fullFilename)
        {
            string[] options = new string[] { "BLOCKXSIZE=" + 256, "BLOCKYSIZE=" + 256 };
            Driver mdrv = Gdal.GetDriverByName("MEM");
            Dataset mds = mdrv.Create("", 256, 256, BandCount, DataType.GDT_Byte, options);
            for (int bandIndex = 1; bandIndex <= BandCount; bandIndex++)
            {
                byte[] buffer = new byte[256 * 256];
                Band pband = PDataSet.GetRasterBand(bandIndex);
                pband.ReadRaster(offsetX, offsetY, blockX, blockY, buffer, bufferX, bufferY, 0, 0);
                Band dband = mds.GetRasterBand(bandIndex);
                dband.WriteRaster(imageX, imageY, bufferX, bufferY, buffer, bufferX, bufferY, 0, 0);
                mds.FlushCache();
            }
            //转换成png
            Driver drv = Gdal.GetDriverByName("PNG");
            Dataset ds = drv.CreateCopy(fullFilename, mds, 0, null, null, null);
            ds.FlushCache();
        }

        /// <summary>
        /// http://https//docs.microsoft.com/zh-cn/dotnet/api/system.drawing.imaging.encoderparameter?view=netframework-4.6
        /// 进一步压缩png图片大小
        /// </summary>
        public void CompressPNG(string fullFilename, int quality)
        {
            Bitmap bmp = new Bitmap(fullFilename);
            ImageCodecInfo codecInfo = GetEncoder(bmp.RawFormat); //图片编解码信息
            System.Drawing.Imaging.Encoder encoder = System.Drawing.Imaging.Encoder.Quality;
            EncoderParameters encoderParameters = new EncoderParameters(1);
            EncoderParameter encoderParameter = new EncoderParameter(encoder, quality);
            encoderParameters.Param[0] = encoderParameter; //编码器参数
            //压缩图路径
            ImageFormat format = bmp.RawFormat;
            String newFilePath = String.Empty; //压缩图所在路径
            // Guid.NewGuid().ToString()
            //GUID 是一个 128 位整数 （16 个字节），它可用于跨所有计算机和网络中，任何唯一标识符是必需的。 此类标识符具有重复的可能性非常小
            String deskPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (format.Equals(ImageFormat.Jpeg))
            {
                newFilePath = deskPath + @"\" + Guid.NewGuid().ToString() + ".jpeg";
            }
            else if (format.Equals(ImageFormat.Png))
            {
                newFilePath = deskPath + @"\" + Guid.NewGuid().ToString() + ".png";
            }
            else if (format.Equals(ImageFormat.Bmp))
            {
                newFilePath = deskPath + @"\" + Guid.NewGuid().ToString() + ".bmp";
            }
            else if (format.Equals(ImageFormat.Gif))
            {
                newFilePath = deskPath + @"\" + Guid.NewGuid().ToString() + ".gif";
            }
            else if (format.Equals(ImageFormat.Icon))
            {
                newFilePath = deskPath + @"\" + Guid.NewGuid().ToString() + ".icon";
            }
            else
            {
                newFilePath = deskPath + @"\" + Guid.NewGuid().ToString() + ".jpg";
            }
            bmp.Save(newFilePath, codecInfo, encoderParameters); //保存压缩图
        }

        private static ImageCodecInfo GetEncoder(ImageFormat rawFormat)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == rawFormat.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        /// <summary>
        /// 构造函数
        /// example var layer = new WebMercatorGridLayer(）
        /// </summary>
        /// <param name="bound"></param>
        public VectorPyramid(GBound bound = null)
        {
            if (bound == null)
                bound = new GBound(new List<Coordinate>() { new Coordinate(-180, 90), new Coordinate(180, -90) });
            for (int i = 0; i <= 19; i++)
                Build(bound, i);
        }

        /// <summary>
        /// 格网瓦片缓存，缩放层级 : 瓦片集合
        /// </summary>
        public Dictionary<int, List<GTileElement>> TileDictionary { get; } = new Dictionary<int, List<GTileElement>>();

        /// <summary>
        /// 构建当前缩放层级的瓦片
        /// </summary>
        /// <param name="bound"></param>
        /// <param name="zoom"></param>
        void Build(GBound bound, int zoom)
        {
            int _tileSize = Projection.TileSize;
            //构建裁剪边界的矩形，用于整体裁剪
            //IGeometry _boundGeometry = bound.ToInsertPolygon();
            //瓦片多尺度缓存
            if (TileDictionary.ContainsKey(zoom))
                TileDictionary[zoom].Clear();
            else
                TileDictionary[zoom] = new List<GTileElement>();
            //1.获取坐上右下坐标
            Coordinate p0 = bound.Min;
            Coordinate p1 = bound.Max;
            //2.分尺度计算格网位置
            //2.1 转换成尺度下的pixel
            var (minX, minY) = Projection.LatlngToPoint(p0.X, p0.Y, zoom);
            var (maxX, maxY) = Projection.LatlngToPoint(p1.X, p1.Y, zoom);
            //2.2 计算pixel下边界范围
            GBound pixelBound = new GBound();
            pixelBound.Extend(new Coordinate(minX, minY));
            pixelBound.Extend(new Coordinate(maxX, maxY));
            //2.3 通过pixelbound计算range
            GBound range = new GBound(new List<Coordinate>() {
                new Coordinate( (int)Math.Floor(pixelBound.Min.X / _tileSize), (int)Math.Floor(pixelBound.Min.Y / _tileSize)),
                 new Coordinate( (int)Math.Ceiling(pixelBound.Max.X / _tileSize)-1, (int)Math.Ceiling(pixelBound.Max.Y / _tileSize)-1),
            });
            //2.3统计区域内瓦片的编号，边界经纬度等信息
            for (int j = Convert.ToInt32(range.Min.Y); j <= Convert.ToInt32(range.Max.Y); j++)
            {
                for (int i = Convert.ToInt32(range.Min.X); i <= Convert.ToInt32(range.Max.X); i++)
                {
                    //反算每块瓦片的边界经纬度
                    List<Coordinate> coordinates = new List<Coordinate>();
                    var (lng0, lat0) = Projection.PointToLatLng(i * 256, j * 256, zoom);
                    var (lng1, lat1) = Projection.PointToLatLng(i * 256 + 256, j * 256 + 256, zoom);
                    coordinates.Add(new Coordinate(lng0,lat0));
                    coordinates.Add(new Coordinate(lng1, lat1));
                    //
                    GTileElement tile = new GTileElement()
                    {
                        X = i,
                        Y = j,
                        Z = zoom,
                        Bound = new GBound(coordinates)
                    };
                    TileDictionary[zoom].Add(tile);
                }
            }
        }
    }
}
