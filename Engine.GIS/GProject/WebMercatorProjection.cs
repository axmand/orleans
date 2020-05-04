using GeoAPI.CoordinateSystems;
using GeoAPI.CoordinateSystems.Transformations;
using GeoAPI.Geometries;
using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;
using System;
using System.Collections.Generic;

namespace Engine.GIS.GProject
{
    /// <summary>
    /// 墨卡托投影
    /// </summary>
    public class WebMercatorProjection
    {
        //转换计算
        GTransformation _transformation = new GTransformation(0.5 / (Math.PI * 6378137), 0.5, -0.5 / (Math.PI * 6378137), 0.5);
        //投影转换factory
        CoordinateTransformationFactory _transformFactory = new CoordinateTransformationFactory();
        //原始坐标系统，wgs84
        IGeographicCoordinateSystem _sourceCoord = GeographicCoordinateSystem.WGS84;
        //投影坐标系
        ICoordinateSystem _targetCoord = ProjectedCoordinateSystem.WebMercator;
        //投影
        ICoordinateTransformation _projectTransform;
        //逆投影
        ICoordinateTransformation _unProjectTransform;
        /// <summary>
        /// 瓦片像素宽高（默认宽=高）
        /// </summary>
        public int TileSize { get; set; }
        /// <summary>
        /// 构造投影类
        /// </summary>
        public WebMercatorProjection()
        {
            TileSize = 256;
            _projectTransform = _transformFactory.CreateFromCoordinateSystems(_sourceCoord, _targetCoord);
            _unProjectTransform = _transformFactory.CreateFromCoordinateSystems(_targetCoord, _sourceCoord);
        }
        /// <summary>
        /// 大地坐标->投影坐标
        /// </summary>
        /// <param name="coordinate"></param>
        /// <returns></returns>
        public Coordinate Project(Coordinate coordinate)
        {
            return _projectTransform.MathTransform.Transform(coordinate);
        }

        /// <summary>
        /// 投影
        /// </summary>
        /// <param name="lng">纬度</param>
        /// <param name="lat">经度</param>
        /// <returns></returns>
        public (double px, double py) Project(double lng, double lat)
        {
            Coordinate c0 = new Coordinate(lng, lat);
            Coordinate c1 = _projectTransform.MathTransform.Transform(c0);
            return (c1.X, c1.Y);
        }

        /// <summary>
        /// 大地坐标->投影坐标
        /// </summary>
        /// <param name="coordinates"></param>
        /// <returns></returns>
        public IList<Coordinate> Project(List<Coordinate> coordinates)
        {
            return _projectTransform.MathTransform.TransformList(coordinates);
        }
        /// <summary>
        /// 投影坐标->大地坐标
        /// </summary>
        /// <param name="coordinate"></param>
        /// <returns></returns>
        public Coordinate UnProject(Coordinate coordinate)
        {
            return _unProjectTransform.MathTransform.Transform(coordinate);
        }
        /// <summary>
        /// 投影坐标->大地坐标
        /// </summary>
        /// <param name="px"></param>
        /// <param name="py"></param>
        /// <returns></returns>
        public (double lng, double lat) UnProject(double px, double py)
        {
            Coordinate c0 = new Coordinate(px, py);
            Coordinate c1 = _projectTransform.MathTransform.Transform(c0);
            return (c1.X, c1.Y);
        }
        /// <summary>
        /// 转换当前的经纬度坐标到屏幕像素坐标
        /// </summary>
        public Coordinate LatlngToPoint(Coordinate latlng, int zoom)
        {
            Coordinate c0 = Project(latlng);
            double scale = Scale(zoom);
            //转换经纬度坐标到当前缩放层级的像素坐标
            Coordinate c1 = _transformation.Transform(c0, scale);
            return c1;
        }
        /// <summary>
        /// 转换经纬度坐标到当前缩放层级的像素坐标
        /// </summary>
        /// <param name="latlng"></param>
        /// <param name="zoom"></param>
        /// <returns></returns>
        public (double x, double y) LatlngToPoint(double lng, double lat, int zoom)
        {
            var(x0, y0) = Project(lng, lat);
            double scale = Scale(zoom);
            //转换经纬度坐标到当前缩放层级的像素坐标
            Coordinate c1 = _transformation.Transform(new Coordinate(x0, y0), scale);
            return (c1.X, c1.Y);
        }
        /// <summary>
        /// 屏幕像素坐标转经纬度坐标
        /// </summary>
        public Coordinate PointToLatLng(Coordinate point, int zoom)
        {
            double scale = Scale(zoom);
            Coordinate c0 = _transformation.UnTransform(point, scale);
            Coordinate c1 = UnProject(c0);
            return c1;
        }
        /// <summary>
        /// 屏幕像素坐标转经纬度坐标
        /// </summary>
        public (double lng, double lat) PointToLatLng(double x, double y, int zoom)
        {
            double scale = Scale(zoom);
            Coordinate c0 = _transformation.UnTransform(new Coordinate(x,y), scale);
            var (lng, lat) = UnProject(c0.X, c0.Y);
            return (lng, lat);
        }
        /// <summary>
        /// 获取当前比例尺地图的总分辨率
        /// </summary>
        public double Scale(int zoom)
        {
            return TileSize * Math.Pow(2, zoom);
        }
    }
}
