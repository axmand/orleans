using Establishment.EWaybill;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Establishment.Helper
{
    /// <summary>
    /// 提供物流查询相关方法
    /// </summary>
    public class WaybillHelper
    {
        /// <summary>
        /// 物流查询
        /// </summary>
        /// <param name="orderCode">订单编号,可填 "" 代替</param>
        /// <param name="ShipperCode">物流公司编码</param>
        /// <param name="LogisticCode">物流单号</param>
        public static string QueryWaybill(string orderCode, string ShipperCode, string LogisticCode)
        {
            try
            {
                //查询字符串
                string requestData = "{'OrderCode':'" + orderCode + "'" + ",'ShipperCode':'" + ShipperCode + "','LogisticCode':'" + LogisticCode + "'}";
                Dictionary<string, string> param = new Dictionary<string, string>();
                param.Add("RequestData", HttpUtility.UrlEncode(requestData, Encoding.UTF8));
                param.Add("EBusinessID", WaybillConfig.EBusinessID);
                param.Add("RequestType", "1002");
                string dataSign = WaybillCore.Encrypt(requestData, WaybillConfig.AppKey, "UTF-8");
                param.Add("DataSign", HttpUtility.UrlEncode(dataSign, Encoding.UTF8));
                param.Add("DataType", "2");
                string result = WaybillCore.SendPost(WaybillConfig.ReqURL, param);
                //根据公司业务处理返回的信息......
                return result;
            }
            catch
            {
                return null;
            }
        }
    }
}
