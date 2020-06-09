using Client.HanYangYun.Util;
using GrainInterface.CMS;
using GrainInterface.WMS;
using ServiceStack;
using System;

namespace Client.HanYangYun.Services
{
    public class CMSRestService: Service
    {
        public bool Get(Routes.CustomerRegister request)
        {
            try
            {
                var cms = Helper.client.GetGrain<IWMS>(0);
                var cms2 = Helper.client.GetGrain<ICMS>(0);
                return true;
                //return cms.Register(request.userName, request.pwd).Result;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
