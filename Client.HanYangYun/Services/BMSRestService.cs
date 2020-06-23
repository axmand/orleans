using Client.HanYangYun.Routes;
using Client.HanYangYun.Util;
using GrainInterface.BMS;
using ServiceStack;
using System.IO;

namespace Client.HanYangYun.Services
{
    public class BMSRestService: Service
    {
        /// <summary>
        /// 地理数据供应
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string Get(BMSGeoDataProvider request)
        {
            try
            {
                IBMSHY bms = Helper.GetGrain<IBMSHY>(0);
                switch (request.name)
                {
                    case "TDXX":
                        return bms.AccessToLandInformation().Result;
                    case "LYXX":
                        return bms.AccessToBuildingInformation().Result;
                    default:
                        return Helper.DataError;
                }
            }
            catch
            {
                return Helper.AbnormalError;
            }
        }

        /// <summary>
        /// 更新楼宇信息矢量数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string Post(BMSLYXXGeoDataUpdate request)
        {
            try
            {
                using (StreamReader sr = new StreamReader(request.RequestStream))
                {
                    IBMSHY bms = Helper.GetGrain<IBMSHY>(0);
                    string response = bms.LYXXGeoDataUpdate(sr.ReadToEnd()).Result;
                    return response;
                }
            }
            catch
            {
                return Helper.AbnormalError;
            }
        }

        /// <summary>
        /// 更新土地信息适量数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string Post(BMSTDXXGeoDataUpdate request)
        {
            try
            {
                using (StreamReader sr = new StreamReader(request.RequestStream))
                {
                    IBMSHY bms = Helper.GetGrain<IBMSHY>(0);
                    string response = bms.TDXXGeoDataUpdate(sr.ReadToEnd()).Result;
                    return response;
                }
            }
            catch
            {
                return Helper.AbnormalError;
            }
        }
    }
}
