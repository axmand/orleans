using Engine.GIS.GLayer.GRasterLayer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.GIS.GTools
{
    public class GRasterPrecentClipTool : IRasterPrecentClipTool
    {
        GRasterBand _pBand;

        public void Dispose()
        {
            _pBand = null;
        }

        public void Visit(GRasterBand pBand)
        {
            _pBand = pBand;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="percentNum">default 0.02. defalut 2% clip</param>
        /// <returns></returns>
        public void ApplyPercentClipStretch(double percentNum = 0.02)
        {
            double percentMin = percentNum, percentMax = 1 - percentMin;
            using(IRasterBandStatisticTool pRasterBandStatisticTool = new GRasterBandStatisticTool())
            {
                pRasterBandStatisticTool.Visit(_pBand);
                Dictionary<double, int>  dict = pRasterBandStatisticTool.StaisticalRawPixelGraph.OrderBy(p => p.Key).ToDictionary(p => p.Key, o => o.Value);
                var (min, max) = ApplyPrecentClipStretch(dict, percentMin, percentMax);
                for (int i = 0; i < _pBand.RawData.Length; i++)
                    if (_pBand.RawData[i] <= min)
                        _pBand.RawData[i] = (float)min;
                    else if (_pBand.RawData[i] >= max)
                        _pBand.RawData[i] = (float)max;
                    else
                        _pBand.RawData[i] = _pBand.RawData[i] - (float)min;
                //更新min和max，用于normalize
                _pBand.Min = min; _pBand.Max = max;
            }
        }

        private (double nMin, double nMax) ApplyPrecentClipStretch(Dictionary<double, int> dict, double percentMin, double percentMax)
        {
            int Width = _pBand.Width, Height = _pBand.Height;
            //找出 percentMin的像素的灰度值
            int percentMinCount = Convert.ToInt32(percentMin * Width * Height), percentMaxCount = Convert.ToInt32(percentMax * Width * Height);
            int countMin = 0, countMax = 0;
            double dbMin = 0, dbMax = 0;
            //找出达到percnetMin的像素的灰度值
            foreach (double key in dict.Keys)
            {
                countMin += dict[key];
                if(countMin > percentMinCount)
                {
                    dbMin = key;
                    break;
                }
            }
            //找出达到percnetMax的像素的灰度值
            foreach (double key in dict.Keys)
            {
                countMax += dict[key];
                if(countMax > percentMaxCount)
                {
                    dbMax = key;
                }
            }
            return (dbMin, dbMax);
            //calcute scale
            //for (int count = 0; count < Width*Height; count++)
            //{
            //    if (RawData[count] <= dbMin)
            //        NormalData[count % Width, count / Width] = 0;
            //    else if (RawData[count] >= dbMax)
            //        NormalData[count % Width, count / Width] = 1;
            //    else
            //        NormalData[count % Width, count / Width] = (RawData[count] - dbMin) / (dbMax - dbMin);
            //}
        }

    }
}
