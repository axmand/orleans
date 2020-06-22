using Client.HanYangYun.Routes;
using Client.HanYangYun.Util;
using GrainInterface.BMS;
using ServiceStack;

namespace Client.HanYangYun.Services
{
    public class BMSRestService: Service
    {
        public string Get(BMSGeoProvider request)
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
    }
}
