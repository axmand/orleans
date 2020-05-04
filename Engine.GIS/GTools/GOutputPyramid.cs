using Engine.GIS.GeoType;
using Engine.GIS.GOperator;
using Engine.GIS.GOperator.Arithmetic;
using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Engine.GIS.GTools
{
    /// <summary>
    /// 金字塔输出
    /// </summary>
    public class OutputPyramid
    {
        VectorPyramid _vectorPyramid;

        public OutputPyramid(VectorPyramid vectorPyramid)
        {
            _vectorPyramid = vectorPyramid;
        }
        /// <summary>
        /// 切割矢量并输出成bitmap
        /// </summary>
        /// <param name="featureCollection"></param>
        /// <param name="outputDir"></param>
        public void Output(FeatureCollection featureCollection, string outputDir)
        {
            var _tileDictionary = _vectorPyramid.TileDictionary;
            int _tileSize = _vectorPyramid.Projection.TileSize;
            //
            foreach (int zoom in _tileDictionary.Keys)
            {
                var tileCollection = _tileDictionary[zoom];
                foreach (var tile in tileCollection)
                {
                    try
                    {
                        //
                        Bitmap bmp = new Bitmap(_tileSize, _tileSize);
                        Graphics g = Graphics.FromImage(bmp);
                        Pen pen = new Pen(Color.Black, 3);
                        //
                        for (int i = 0; i < featureCollection.Count; i++)
                        {
                            IFeature f = featureCollection[i];
                            //点
                            if (f.Geometry.OgcGeometryType == OgcGeometryType.Point)
                            {
                                Coordinate point = f.Geometry.Coordinate;
                                if (tile.Bound.PointInPolygon(point))
                                {
                                    //2.2.1 计算点的像素坐标
                                    var (sx, sy) = _vectorPyramid.Projection.LatlngToPoint(point.X, point.Y, zoom);
                                    double deltaX = sx / _tileSize - tile.X;
                                    double deltaY = sy / _tileSize - tile.Y;
                                    int x = Convert.ToInt32(deltaX * _tileSize);
                                    int y = Convert.ToInt32(deltaY * _tileSize);
                                    g.DrawLine(pen, x, x, x, y);
                                }
                                continue;
                            }
                            //线
                            else if (f.Geometry.OgcGeometryType == OgcGeometryType.LineString)
                            {
                                //2.1瓦片裁剪道路
                                List<Coordinate> clipLine = CohenSutherland.GetIntersectedPolyline(f.Geometry.Coordinates, tile.Bound);
                                if (clipLine.Count == 0) continue;
                                int x0 = -1000, y0 = -1000;
                                //2.2 绘制clipLine
                                foreach (Coordinate point in clipLine)
                                {
                                    //2.2.1 计算点的像素坐标
                                    var (sx, sy) = _vectorPyramid.Projection.LatlngToPoint(point.X, point.Y, zoom);
                                    double deltaX = sx / _tileSize - tile.X;
                                    double deltaY = sy / _tileSize - tile.Y;
                                    int x = Convert.ToInt32(deltaX * _tileSize);
                                    int y = Convert.ToInt32(deltaY * _tileSize);
                                    if (x0 == -1000 && y0 == -1000)
                                    {
                                        x0 = x;
                                        y0 = y;
                                        continue;
                                    }
                                    else
                                    {
                                        g.DrawLine(pen, x0, y0, x, y);
                                        x0 = x;
                                        y0 = y;
                                    }
                                }
                            }
                            //面
                            else if (f.Geometry.OgcGeometryType == OgcGeometryType.Polygon)
                            {
                                List<Coordinate> clipPolygon = SutherlandHodgman.GetIntersectedPolygon(f.Geometry.Coordinates, tile.Bound);
                                if (clipPolygon.Count < 3) continue;
                                int x0 = -1000, y0 = -1000;
                                //2.2 绘制clipLine
                                foreach (Coordinate point in clipPolygon)
                                {
                                    //2.2.1 计算点的像素坐标
                                    var (sx, sy) = _vectorPyramid.Projection.LatlngToPoint(point.X, point.Y, zoom);
                                    //
                                    double deltaX = sx / _tileSize - tile.X;
                                    double deltaY = sy / _tileSize - tile.Y;
                                    int x = Convert.ToInt32(deltaX * _tileSize);
                                    int y = Convert.ToInt32(deltaY * _tileSize);
                                    if (x0 == -1000 && y0 == -1000)
                                    {
                                        x0 = x;
                                        y0 = y;
                                        continue;
                                    }
                                    else
                                    {
                                        g.DrawLine(pen, x0, y0, x, y);
                                        x0 = x;
                                        y0 = y;
                                    }
                                }
                            }
                        }
                        //2.3 保存bmp到指定路径
                        if (!System.IO.Directory.Exists(outputDir + @"\" + zoom))
                            System.IO.Directory.CreateDirectory(outputDir + @"\" + zoom);
                        //根据geometry id存储，获取不到geometry的id，所以只能自定内部序号
                        bmp.Save(outputDir + @"\" + zoom + @"\" + tile.X + "_" + tile.Y + "_" + tile.Z + ".jpg");
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
        }

        public void Output(string output)
        {
            var _tileDictionary = _vectorPyramid.TileDictionary;
            int _tileSize = _vectorPyramid.Projection.TileSize;
            //
            foreach (int zoom in _tileDictionary.Keys)
            {
                var tileCollection = _tileDictionary[zoom];
                foreach (var tile in tileCollection)
                {
                    //0. 计算瓦片与图像相交的区域
                    GBound bound = tile.Bound.Insection(_vectorPyramid.Bound);
                    //1.瓦片分辨率
                    double dst_w_e_pixel_resolution = (tile.Bound.Right - tile.Bound.Left) / 256;
                    double dst_n_s_pixel_resolution = (tile.Bound.Top - tile.Bound.Bottom) / 256;
                    //1.求切图范围和原始图像交集的起始点像素坐标
                    int offset_x = (int)((bound.Left - _vectorPyramid.Bound.Left) / _vectorPyramid.src_w_e_pixel_resolution);
                    int offset_y = -(int)((bound.Top - _vectorPyramid.Bound.Top) / _vectorPyramid.src_n_s_pixel_resolution);
                    //2.求在切图地理范围内的原始图像的像素大小
                    int block_xsize = (int)((bound.Right - bound.Left) / _vectorPyramid.src_w_e_pixel_resolution);
                    int block_ysize = (int)((bound.Top - bound.Bottom) / _vectorPyramid.src_n_s_pixel_resolution);
                    //3.求原始图像在切片内的像素大小
                    int image_Xbuf = (int)Math.Ceiling((bound.Right - bound.Left) / dst_w_e_pixel_resolution);
                    int image_Ybuf = (int)Math.Ceiling((bound.Top - bound.Bottom) / dst_n_s_pixel_resolution);
                    var pband = _vectorPyramid.PDataSet.GetRasterBand(1);
                    byte[] buffer = new byte[256 * 256];
                    pband.ReadRaster(offset_x, offset_y, block_xsize, block_ysize, buffer, image_Xbuf, image_Ybuf, 0, 0);
                    //pband.ReadRaster(0, 0, _vectorPyramid.PDataSet.RasterXSize, _vectorPyramid.PDataSet.RasterYSize, buffer, 256, 256, 0, 0);
                    // 求原始图像在切片中的偏移坐标
                    int imageOffsetX = (int)((bound.Left - tile.Bound.Left) / dst_w_e_pixel_resolution);
                    int imageOffsetY = -(int)((bound.Top - tile.Bound.Top) / dst_n_s_pixel_resolution);
                    imageOffsetX = imageOffsetX > 0 ? imageOffsetX : 0;
                    imageOffsetY = imageOffsetY > 0 ? imageOffsetY : 0;
                    //1. 计算瓦片的 geotransform，得到6参数后从原始图像中提取
                    string tms = @"D:\迅雷下载\Cache\TMS2\";
                    string tmsDir = tms + tile.Z + @"\" + tile.X + @"\";
                    if (!System.IO.Directory.Exists(tmsDir))
                        System.IO.Directory.CreateDirectory(tmsDir);
                    string fullFilename = tmsDir + tile.Y + ".png";
                    _vectorPyramid.CratePNG(offset_x, offset_y, block_xsize, block_ysize, image_Xbuf, image_Ybuf, imageOffsetX, imageOffsetY, fullFilename);
                }
            }
        }
    }
}
