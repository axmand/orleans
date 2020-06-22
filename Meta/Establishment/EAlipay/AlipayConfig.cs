using System;

namespace Engine.Facility.EAlipay
{
    /// <summary>
    /// 类名：Config
    /// 功能：基础配置类
    /// 详细：设置帐户有关信息及返回路径
    /// 版本：3.4
    /// 修改日期：2016-03-08
    /// </summary>
    public class AlipayConfig
    {
        /// <summary>
        /// 重置config文件
        /// </summary>
        public static void SetConfig(string _partner, string _key, string _notifyUrl)
        {
            partner = _partner;
            seller_user_id = partner;
            key = _key;
            notify_url = _notifyUrl;
        }

        //↓↓↓↓↓↓↓↓↓↓请在这里配置您的基本信息↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓

        // 合作身份者ID，签约账号，以2088开头由16位纯数字组成的字符串，查看地址：https://b.alipay.com/order/pidAndKey.htm
        //Properties.Resources.bdl_partner
        public static string partner = "";

        // 卖家支付宝账号，以2088开头由16位纯数字组成的字符串，一般情况下收款账号就是签约账号
        public static string seller_user_id = partner;

        // MD5密钥，安全检验码，由数字和字母组成的32位字符串，查看地址：https://b.alipay.com/order/pidAndKey.htm
        //Properties.Resources.bdl_key
        public static string key = "";

        // 服务器异步通知页面路径，需http://格式的完整路径，不能加?id=123这类自定义参数,必须外网可以正常访问
        //Properties.Resources.bdl_notify
        public static string notify_url = "";

        // 签名方式
        public static string sign_type = "MD5";

        // 退款日期 时间格式 yyyy-MM-dd HH:mm:ss
        public static string refund_date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        // 调试用，创建TXT日志文件夹路径，见AlipayCore.cs类中的LogResult(string sWord)打印方法。
        //HttpRuntime.AppDomainAppPath.ToString() + "log\\";
        public static string log_path = "";

        // 字符编码格式 目前支持 gbk 或 utf-8
        public static string input_charset = "utf-8";

        // 调用的接口名，无需修改
        public static string service = "refund_fastpay_by_platform_pwd";

        //↑↑↑↑↑↑↑↑↑↑请在这里配置您的基本信息↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑
    }
}
