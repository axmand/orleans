using Client.HanYangYun.Routes;
using Client.HanYangYun.Util;
using Engine.Facility.Helper;
using GrainInterface.BMS;
using GrainInterface.CMS;
using GrainInterface.WMS;
using ServiceStack.ServiceInterface;
using System;
using System.IO;

namespace Client.HanYangYun.Services
{
    public class BMSRestService: Service
    {

        /// <summary>
        /// 瓦片请求服务
        /// </summary>
        /// <param name="request"></param>
        [AddHeader(ContentType = "image/png")]
        public Stream Get(Routes.BMSTMSHYLayer request)
        {
            try
            {
                IWMS wms = Helper.GetGrain<IWMS>(0);
                Stream sm = wms.GetTileImagePNG(request.x, request.y, request.z).Result;
                return sm;
            }
            catch
            {
                return null;
            }
        }

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
                        return bms.AccessTDXXGeoData().Result;
                    case "LYXX":
                        return bms.AccessLYXXGeoData().Result;
                    default:
                        return Helper.DataError;
                }
            }
            catch(Exception ex)
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
                    var (userName, token, content) = CMSHelper.SerializeText(sr.ReadToEnd());
                    IGroup cms = Helper.GetGrain<IGroup>(0);
                    Type t = request.GetType();
                    if (CMSHelper.CheckAPIConfigurable(t) && cms.CheckAPIPermession(userName, token, t.FullName).Result)
                    {
                        IBMSHY hybms = Helper.GetGrain<IBMSHY>(0);
                        string response = hybms.GeoDataLYXXUpdate(content).Result;
                        return response;
                    }
                    return Helper.PermessionError;
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
                    var (userName, token, content) = CMSHelper.SerializeText(sr.ReadToEnd());
                    IGroup cms = Helper.GetGrain<IGroup>(0);
                    Type t = request.GetType();
                    if(CMSHelper.CheckAPIConfigurable(t) && cms.CheckAPIPermession(userName, token, t.FullName).Result)
                    {
                        IBMSHY hybms = Helper.GetGrain<IBMSHY>(0);
                        string response = hybms.GeoDataTDXXUpdate(content).Result;
                        return response;
                    }
                    return Helper.PermessionError;
                }
            }
            catch
            {
                return Helper.AbnormalError;
            }
        }
    }
}
