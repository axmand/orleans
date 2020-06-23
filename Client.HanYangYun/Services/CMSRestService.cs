﻿using Client.HanYangYun.Util;
using GrainInterface.CMS;
using ServiceStack;
using System;
using System.IO;

namespace Client.HanYangYun.Services
{
    public class CMSRestService : Service
    {
        /// <summary>
        /// 账户注册
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string Post(Routes.CMSRegister request)
        {
            try
            {
                using (StreamReader sr = new StreamReader(request.RequestStream))
                {
                    ICustomer cms = Helper.GetGrain<ICustomer>(0);
                    string response = cms.Register(sr.ReadToEnd()).Result;
                    return response;
                }
            }
            catch
            {
                return Helper.AbnormalError;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string Post(Routes.CMSLogin request)
        {
            try
            {
                using (StreamReader sr = new StreamReader(request.RequestStream))
                {
                    ICustomer cms = Helper.GetGrain<ICustomer>(0);
                    string response = cms.Login(sr.ReadToEnd()).Result;
                    return response;
                }
            }
            catch
            {
                return Helper.AbnormalError;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string Get(Routes.CMSCreateGroup request)
        {
            try
            {
                IGroup cms = Helper.GetGrain<IGroup>(0);
                string response = cms.CreateGroup(request.userName, request.token, request.groupName, request.groupDesc, request.groupLevel).Result;
                return response;
            }
            catch
            {
                return Helper.AbnormalError;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string Get(Routes.CMSSearchCustomerByName request)
        {
            try
            {
                ICustomer cms = Helper.GetGrain<ICustomer>(0);
                string response = cms.SearchCustomerByName(request.searchWord, request.userName, request.token).Result;
                return response;
            }
            catch
            {
                return Helper.AbnormalError;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string Get(Routes.CMSSetCustomerGroup request)
        {
            try
            {
                IGroup cms = Helper.GetGrain<IGroup>(0);
                string response = cms.SetCustomerGroup(request.userName, request.token, request.customerObjectId, request.groupObjectId).Result;
                return response;
            }
            catch
            {
                return Helper.AbnormalError;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string Get(Routes.CMSGetGroupList request)
        {
            try
            {
                IGroup cms = Helper.GetGrain<IGroup>(0);
                string response = cms.GetGroupList(request.userName, request.token).Result;
                return response;
            }
            catch
            {
                return Helper.AbnormalError;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string Get(Routes.CMSDeleteGroup request)
        {
            try
            {
                IGroup cms = Helper.GetGrain<IGroup>(0);
                string response = cms.DeleteGroup(request.userName, request.token, request.groupObjectId).Result;
                return response;
            }
            catch
            {
                return Helper.AbnormalError;
            }
        }

        /// <summary>
        /// 获取CMS支持的可配置接口列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string Get(Routes.CMSGetConfigureableAPIList request)
        {
            try
            {
                Type[] response = Engine.Facility.Helper.CMSHelper.GetConfigurableAPIList().Result;
                return new Engine.Facility.EResponse.OkResponse(response).ToString();
            }
            catch
            {
                return Helper.AbnormalError;
            }
        }

    }
}
