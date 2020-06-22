namespace Engine.Facility.EWxpay
{
    /// <summary>
    /// 微信支付配置
    /// </summary>
    public class WxpayConfig
    {
        /// <summary>
        /// 统一创建订单 rest 接口地址
        /// </summary>
        public static string UnifiedPayUrl = "https://api.mch.weixin.qq.com/pay/unifiedorder";
        /// <summary>
        /// 获取token接口地址
        /// </summary>
        public static string Access_tokenUrl = "https://api.weixin.qq.com/sns/oauth2/access_token";
        /// <summary>
        /// 查询订单接口地址
        /// </summary>
        public static string OrderQueryUrl = "https://api.mch.weixin.qq.com/pay/orderquery";
        /// <summary>
        /// 订单退款接口地址
        /// </summary>
        public static string RefundOrderUrl = "https://api.mch.weixin.qq.com/secapi/pay/refund";
        /// <summary>
        /// 通知地址（自定义）
        /// </summary>
        public static string NotifyUrl = "http://114.55.119.208:10100/wxnotify/post";
        /// <summary>
        /// 微信开发者appid
        /// </summary>
        public static string AppId = "";
        /// <summary>
        /// 微信支付店铺id
        /// </summary>
        public static string MerchantId = "";
        /// <summary>
        /// 微信app key
        /// </summary>
        public static string AppKey = "";
        /// <summary>
        /// byte流文件，需要下载的微信流文件
        /// </summary>
        public static byte[] AppCert;
        /// <summary>
        /// 头文件，显示收银台名称
        /// </summary>
        public static string AppName = "";
        /// <summary>
        /// 配置微信支付参数
        /// </summary>
        /// <param name="_notifyUrl">异步支付通知地址</param>
        /// <param name="_appId">微信开放平台appid</param>
        /// <param name="_merchantId">微信支付店铺id</param>
        /// <param name="_appKey">微信开放平台appkey</param>
        /// <param name="_cert">安全校验文件流</param>
        public static void SetConfig(string _notifyUrl, string _appId, string _merchantId, string _appKey, byte[] _cert,string _appName)
        {
            AppId = _appId;
            NotifyUrl = _notifyUrl;
            MerchantId = _merchantId;
            AppKey = _appKey;
            AppCert = _cert;
            AppName = _appName;
        }
    }
}
