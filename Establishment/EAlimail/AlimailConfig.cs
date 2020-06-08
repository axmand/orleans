namespace Establishment.EAlimail
{
    /// <summary>
    /// 阿里云企业邮箱配置
    /// </summary>
    public class AlimailConfig
    {
        /// <summary>
        /// 企业邮箱账户
        /// </summary>
        public static string mailServerName { get; set; }
        /// <summary>
        /// 企业邮箱密码
        /// </summary>
        public static string mailServerPwd { get; set; }
        /// <summary>
        /// 企业邮箱smtp服务地址
        /// </summary>
        public static string mailServerSmtpUrl { get; set; }
        /// <summary>
        /// 企业邮箱服务号标题，例如：XX安全中心
        /// </summary>
        public static string serverTitle { get; set; }
        /// <summary>
        /// smtp服务端口号
        /// </summary>
        public static int mailServerPort { get; set; }
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="_mailServerName"></param>
        /// <param name="_mailServerPwd"></param>
        /// <param name="_mailServerSmtpUrl"></param>
        /// <param name="_serverTitle"></param>
        /// <param name="_mailServerPort"></param>
        public static void SetConfig(string _mailServerName,string _mailServerPwd,string _mailServerSmtpUrl,string _serverTitle,int _mailServerPort)
        {
            mailServerName = _mailServerName;
            mailServerPwd = _mailServerPwd;
            mailServerSmtpUrl = _mailServerSmtpUrl;
            serverTitle = _serverTitle;
            mailServerPort = _mailServerPort;
        }
    }
}
