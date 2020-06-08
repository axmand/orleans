using System;

namespace Establishment.Phone
{
    /// <summary>
    /// 短信服务配置
    /// </summary>
    public class PhoneConfig
    {
        /// <summary>
        /// 联荣云通讯账户
        /// </summary>
        public static string phoneAccount { get; set; }
        /// <summary>
        /// token
        /// </summary>
        public static string phoneToken { get; set; }
        /// <summary>
        /// apppid
        /// </summary>
        public static string phoneAppid { get; set; }
        /// <summary>
        /// 发送短信服务接口地址
        /// </summary>
        public static string phoneRest { get; set; }
        /// <summary>
        /// 发送短信服务端口
        /// </summary>
        public static string phoneProt { get; set; }
        /// <summary>
        /// 短信模版id号
        /// </summary>
        public static string phoneTepmplate { get; set; }
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="_phoneAccount">联荣云通讯账户</param>
        /// <param name="_phoneToken">token</param>
        /// <param name="_phoneAppid">appid</param>
        /// <param name="_phoneRest">发送短信服务接口地址</param>
        /// <param name="_phoneProt">发送短信服务端口</param>
        /// <param name="_phoneTepmplate">短信模版id号</param>
        public static void SetConfig(string _phoneAccount, string _phoneToken, string _phoneAppid, string _phoneRest, string _phoneProt, string _phoneTepmplate)
        {
            phoneAccount = _phoneAccount;
            phoneToken = _phoneToken;
            phoneAppid = _phoneAppid;
            phoneRest = _phoneRest;
            phoneProt = _phoneProt;
            phoneTepmplate = _phoneTepmplate;
            //
            api = new CCPRestSDK();
            bool isInit = api.init(phoneRest, phoneProt);
            if (isInit)
            {
                api.setAccount(phoneAccount, phoneToken);
                api.setAppId(phoneAppid);
            }
            else
            {
                Console.Write("短信模块初始化失败");
            }
        }
        /// <summary>
        /// 短信发送模块
        /// </summary>
        public static CCPRestSDK api;

    }
}
