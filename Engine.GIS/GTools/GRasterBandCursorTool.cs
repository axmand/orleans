using Engine.GIS.GLayer.GRasterLayer;
using System;
using System.Collections.Generic;

namespace Engine.GIS.GTools
{
    /// <summary>
    /// cursor for each band
    /// </summary>
    public class GRasterBandCursorTool : IRasterBandCursorTool, IDisposable
    {
        /// <summary>
        /// band width and hight
        /// </summary>
        int _width, _height;

        /// <summary>
        /// 
        /// </summary>
        GRasterBand _pBand;

        /// <summary>
        /// visit band
        /// </summary>
        /// <param name="pBand"></param>
        public void Visit(GRasterBand pBand)
        {
            _pBand = pBand;
            _width = pBand.Width;
            _height = pBand.Height;
        }

        /// <summary>
        /// 注意，这里的处理和GRasterBand中的归一化一致，保证输入是0-1，不会出现负值情况
        /// </summary>
        /// <returns></returns>
        public IEnumerable<(int, int, double)> ValidatedNormalCollection
        {
            get
            {
                double scale = _pBand.Max - _pBand.Min;
                for (int x = 0; x < _width; x++)
                    for (int y = 0; y < _height; y++)
                        if (_pBand.RawData[y * _width + x] != 0)
                            yield return (x, y, _pBand.RawData[x + y * _pBand.Width] == 0.0f ? 0.0 : (_pBand.RawData[x + y * _pBand.Width] - _pBand.Min) / scale);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<(int, int, double)> ValidatedRawCollection
        {
            get
            {
                for (int i = 0; i < _width * _height; i++)
                    if (_pBand.RawData[i] != 0)
                        yield return (i % _width, i / _width, _pBand.RawData[i]);
            }
        }

        /// <summary>
        /// pick raw value
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public float PickRawValue(int x, int y)
        {
            int position = y * _width + x;
            return _pBand.RawData[position];
        }

        /// <summary>
        /// pick normalized value
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public float PickNormalValue(int x, int y)
        {
            int n = y * _width + x;
            double scale = _pBand.Max - _pBand.Min;
            double v = _pBand.RawData[n] == 0.0f ? 0.0f : (_pBand.RawData[n] - _pBand.Min) / scale;
            return (float)v;
        }

        /// <summary>
        /// 获取 Row x Col 个像素的矩形区域像素值
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="rowcol">represent matrix rows and col,the matrix is row*col,i.e  9 represent 3x3 </param>
        public float[] PickRangeNormalValue(int x, int y, int row = 5, int col = 5)
        {
            int offset = row / 2;
            List<float> pixels = new List<float>();
            for (int i = -offset; i < row - offset; i++)
                for (int j = -offset; j < col - offset; j++)
                {
                    int pi = x + i;
                    int pj = y + j;
                    pi = pi <= 0 || pi >= _width ? x : pi;
                    pj = pj <= 0 || pj >= _height ? y : pj;
                    pixels.Add(PickNormalValue(pi, pj));
                }
            return pixels.ToArray();
        }

        /// <summary>
        /// 获取raw value of x,y with cov [row,col]
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public float[] PickRangeRawValue(int x, int y, int row = 5, int col = 5)
        {
            int offset = row / 2;
            List<float> pixels = new List<float>();
            for (int i = -offset; i < row - offset; i++)
                for (int j = -offset; j < col - offset; j++)
                {
                    int pi = x + i;
                    int pj = y + j;
                    pi = pi <= 0 || pi >= _width ? x : pi;
                    pj = pj <= 0 || pj >= _height ? y : pj;
                    pixels.Add(PickRawValue(pi, pj));
                }
            return pixels.ToArray();
        }

        public void Dispose()
        {
            _pBand = null;
        }

    }
}
