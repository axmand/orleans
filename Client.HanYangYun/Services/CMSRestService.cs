using Client.HanYangYun.Util;
using GrainInterface.CMS;
using GrainInterface.WMS;
using Microsoft.Extensions.DependencyInjection;
using Orleans;
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
                var wms = Helper.GetGrain<IWMS>(0);
                var cms = Helper.GetGrain<ICMS>(0);
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
