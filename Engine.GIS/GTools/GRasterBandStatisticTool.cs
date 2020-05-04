using Engine.GIS.GLayer.GRasterLayer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Engine.GIS.GTools
{
    public class GRasterBandStatisticTool : IRasterBandStatisticTool, IDisposable
    {

        /// <summary>
        /// band width and hight
        /// </summary>
        int _width, _height;

        /// <summary>
        /// raw data
        /// </summary>
        float[] _rawData;

        GRasterBand _pBand;

        public void Visit(GRasterBand pBand)
        {
            _pBand = pBand;
            _width = pBand.Width;
            _height = pBand.Height;
            _rawData = pBand.RawData;
        }

        public void Dispose()
        {
            _pBand = null;
        }

        public float[,] StatisticalRawQueryTable
        {
            get
            {
                float[,] queryTable = new float[_width, _height];
                for (int position = 0; position < _width * _height; position++)
                    queryTable[position % _width, position / _width] = _rawData[position];
                return queryTable;
            }
        }

        /// <summary>
        /// 计算每个像素对应的像素个数
        /// </summary>
        public Dictionary<double, int> StaisticalRawPixelGraph
        {
            get
            {
                using (IRasterBandCursorTool pBandCursorTool = new GRasterBandCursorTool())
                {
                    Dictionary<double, int> memory = new Dictionary<double, int>();
                    pBandCursorTool.Visit(_pBand);
                    foreach (var (x, y, value) in pBandCursorTool.ValidatedRawCollection)
                    {
                        if (memory.ContainsKey(value))
                            memory[value]++;
                        else
                            memory.Add(value, 1);
                    }
                    //sort memory
                    memory = memory.OrderBy(p => p.Key).ToDictionary(p => p.Key, o => o.Value);
                    //return statical result
                    return memory;
                }
            }
        }

        public Dictionary<int, List<Point>> StaisticalRawGraph
        {
            get {
                using(IRasterBandCursorTool pBandCursorTool = new GRasterBandCursorTool())
                {
                    Dictionary<int, List<Point>> memory = new Dictionary<int, List<Point>>();
                    pBandCursorTool.Visit(_pBand);
                    foreach (var (x, y, value) in pBandCursorTool.ValidatedRawCollection)
                    {
                        int classIndex = Convert.ToInt16(value);
                        if (memory.ContainsKey(classIndex))
                            memory[classIndex].Add(new Point(x, y));
                        else
                            memory.Add(classIndex, new List<Point>() { new Point(x, y) });
                    }
                    //sort memory
                    memory = memory.OrderBy(p => p.Key).ToDictionary(p => p.Key, o => o.Value);
                    //return statical result
                    return memory;
                }
            }
        }
    }
}
