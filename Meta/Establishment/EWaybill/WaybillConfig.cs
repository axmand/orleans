namespace Establishment.EWaybill
{
    /// <summary>
    /// 快递鸟-物流配置
    /// </summary>
    public class WaybillConfig
    {
        /// <summary>
        /// 设置参数值
        /// </summary>
        /// <param name="eBusinessId">电商ID</param>
        /// <param name="appKey">电商加密私钥，快递鸟提供，注意保管，不要泄漏</param>
        /// <param name="reqURL">请求url</param>
        public static void SetConfig(string eBusinessId, string appKey, string reqURL)
        {
            EBusinessID = eBusinessId;
            AppKey = appKey;
            ReqURL = reqURL;
        }
        /// <summary>
        /// 电商ID
        /// </summary>
        public static string EBusinessID = "";
        /// <summary>
        /// 电商加密私钥，快递鸟提供，注意保管，不要泄漏
        /// </summary>
        public static string AppKey = "";
        /// <summary>
        /// 请求url
        /// </summary>
        public static string ReqURL = "";
    }
}
