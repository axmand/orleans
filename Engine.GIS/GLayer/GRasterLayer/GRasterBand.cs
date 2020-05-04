using System;
using System.Drawing;
using Engine.GIS.Extend;
using Engine.GIS.GEntity;
using OSGeo.GDAL;

namespace Engine.GIS.GLayer.GRasterLayer
{
    /// <summary>
    /// raw data type is double
    /// </summary>
    public class GRasterBand : GBitmap, IDisposable
    {

        #region 属性

        /// <summary>
        /// 灰度直方图
        /// </summary>

        /// <summary>
        /// 统计属性
        /// </summary>
        private double _min, _max, _mean, _stdDev;

        /// <summary>
        /// 标准差
        /// </summary>
        public double StdDev { get { return _stdDev; } }

        /// <summary>
        /// 全图均值
        /// </summary>
        public double Mean { get { return _mean; } }

        /// <summary>
        /// 波段最小值
        /// </summary>
        public double Min { get { return _min; } set { _min = value; } }

        /// <summary>
        /// 波段最大值
        /// </summary>
        public double Max { get { return _max; } set { _max = value; } }

        /// <summary>
        /// 波段索引
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// 波段图层名
        /// </summary>
        public string BandName { get; set; }

        /// <summary>
        /// 波段序号
        /// </summary>
        public int BandIndex { get { return Index; } }

        /// <summary>
        /// 图像宽度
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// 图像高度
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// raw data
        /// </summary>
        public float[] RawData { get; set; }

        public Band GeoBand { get; }

        #endregion

        /// <summary>
        /// clearn the error data
        /// </summary>
        private void CheckDataError()
        {
            for (int i = 0; i < Width * Height; i++)
                if (RawData[i] > _max || RawData[i] < _min)
                    RawData[i] = 0;
        }

        /// <summary>
        /// 包装GDALBand
        /// </summary>
        /// <param name="pBand"></param>
        public GRasterBand(Band pBand)
        {
            //band 序号
            GeoBand = pBand;
            Index = pBand.GetBand();
            Width = pBand.XSize;
            Height = pBand.YSize;
            pBand.SetNoDataValue(0);
            //approx_ok ：true 表示粗略统计，false表示严格统计, bForce：表示扫描图统计生成xml
            pBand.GetStatistics(0, 1, out _min, out _max, out _mean, out _stdDev);
            //show data process
            RawData = new float[Width * Height];
            pBand.ReadRaster(0, 0, Width, Height, RawData, Width, Height, 0, 0);
            //remove error data
            CheckDataError();
            //calctue image
            GrayscaleImage = ToGrayBitmap(GetByteBuffer(), Width, Height);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        public void Update()
        {
            GrayscaleImage = ToGrayBitmap(GetByteBuffer(), Width, Height);
        }

        /// <summary>
        /// byte数据流
        /// </summary>
        /// <returns>stretched byte buffer</returns>
        public byte[,] GetByteBuffer()
        {
            byte[,] _stretchedByteData = new byte[Width, Height];
            double scale = 255.0 / (Max - Min);
            for (int count = 0; count < RawData.Length; count++)
                _stretchedByteData[count % Width, count / Width] = RawData[count] == 0.0f ? (byte)0: (byte)((RawData[count] - Min) * scale);
            return _stretchedByteData;
        }

        /// <summary>
        /// 获取未拉伸的原始bytebuffer
        /// </summary>
        /// <returns></returns>
        public float[] GetRawBuffer()
        {
            return RawData;
        }

        /// <summary>
        /// get normal data buffer
        /// </summary>
        /// <returns></returns>
        public double[] NormalDataBuffer()
        {
            double[] normalDataBuffer = new double[RawData.Length];
            double scale = Max - Min;
            for (int count = 0; count < RawData.Length; count++)
                normalDataBuffer[count] = RawData[count] == 0.0f ? 0.0 : (RawData[count] - Min) / scale;
            return normalDataBuffer;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            RawData = null;
        }

        /// <summary>
        /// gray scale image
        /// </summary>
        public Bitmap GrayscaleImage { get; private set; }

    }
}
