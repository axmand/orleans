using Client.HanYangYun.Util;
using Establishment.EResponse;
using GrainInterface.CMS;
using ServiceStack;
using System;
using System.IO;

namespace Client.HanYangYun.Services
{
    public class CMSRestService: Service
    {
        public string Post(Routes.Register request)
        {
            try
            {
                using(StreamReader sr = new StreamReader(request.RequestStream))
                {
                    ICMS cms = Helper.GetGrain<ICMS>(0);
                    string response = cms.Register(sr.ReadToEnd()).Result;
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
