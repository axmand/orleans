using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;

namespace Engine.GIS.GeoType
{
    /// <summary>
    /// @author yellow
    /// @date 2017/11/3
    /// 记录box边界
    /// </summary>
    public class GBound
    {

        /// <summary>
        /// box区域最小坐标
        /// </summary>
        public Coordinate Min { get; private set; }

        /// <summary>
        /// box区域最大坐标
        /// </summary>
        public Coordinate Max { get; private set; }

        /// <summary>
        /// 左侧
        /// </summary>
        public double Left { get; private set; }

        /// <summary>
        /// 底部
        /// </summary>
        public double Bottom { get; private set; }

        /// <summary>
        /// 右侧
        /// </summary>
        public double Right { get; private set; }

        /// <summary>
        /// 顶部
        /// </summary>
        public double Top { get; private set; }

        /// <summary>
        /// 根据点构造box区域（extent）
        /// </summary>
        /// <param name="coordinates"></param>
        public GBound(List<Coordinate> coordinates)
        {
            foreach (Coordinate p in coordinates)
                Extend(p);
        }

        /// <summary>
        /// 默认初始化
        /// </summary>
        public GBound()
        {

        }

        /// <summary>
        /// 计算外轮廓
        /// </summary>
        public void Extend(Coordinate point)
        {
            if (Min == null && Max == null)
            {
                Min = point.Copy() as Coordinate;
                Max = point.Copy() as Coordinate;
            }
            else
            {
                Min.X = Math.Min(point.X, Min.X);
                Max.X = Math.Max(point.X, Max.X);
                Min.Y = Math.Min(point.Y, Min.Y);
                Max.Y = Math.Max(point.Y, Max.Y);
            }
            Left = Min.X;
            Bottom = Min.Y;
            Right = Max.X;
            Top = Max.Y;
        }

        /// <summary>
        /// 转换成多边形，便于裁剪计算
        /// </summary>
        /// <returns></returns>
        public Coordinate[] ToClipPolygon()
        {
            return new Coordinate[4] {
                new Coordinate(Left,Top),
                new Coordinate(Left,Bottom),
                new Coordinate(Right,Bottom),
                new Coordinate(Right,Top)
            };
        }

        /// <summary>
        /// 转换成内判断矩形，用于筛选
        /// </summary>
        /// <returns></returns>
        public Polygon ToInsertPolygon()
        {
            Coordinate[] coordinates = new Coordinate[5] { new Coordinate(Left, Top), new Coordinate(Left, Bottom), new Coordinate(Right, Bottom), new Coordinate(Right, Top), new Coordinate(Left, Top) };
            LinearRing ring = new LinearRing(coordinates);
            Polygon polygon = new Polygon(ring);
            return polygon;
        }

        /// <summary>
        /// 判断点是否在多边形内
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool PointInPolygon(Coordinate p)
        {
            return !(p.X <= Left || p.X >= Right || p.Y <= Bottom || p.Y >= Top);
        }

        public GBound Insection(GBound b)
        {
            double wb = b.Right - b.Left,
                    wa = Right - Left,
                    hb = b.Top - b.Bottom,
                    ha = Top - Bottom;
            if (Math.Abs(Right + Left - b.Right - b.Left) > wa + wb || Math.Abs(Top + Bottom - b.Top - b.Bottom) > ha + hb)
                return null;
            return new GBound(new List<Coordinate>() {
                new Coordinate(Math.Max(Left, b.Left), Math.Min(Top, b.Top)),
                new Coordinate(Math.Min(Right, b.Right), Math.Max(Bottom, b.Bottom))
            });
        }

    }
}
