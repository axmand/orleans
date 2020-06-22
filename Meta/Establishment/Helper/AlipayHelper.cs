using Engine.Facility.EAlipay;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Engine.Facility.Helper
{
    /// <summary>
    /// 提供支付宝支付处理方法
    /// </summary>
    public class AlipayHelper
    {
        /// <summary>
        /// 验证通知信息来源是否真实
        /// </summary>
        /// <param name="notifyId"></param>
        public async static Task<bool> VierfyNotify(string notifyId)
        {
            string uri = "https://mapi.alipay.com/gateway.do?service=notify_verify&partner=" + AlipayConfig.partner + "&notify_id=" + notifyId;
            HttpClient client = new HttpClient();
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody == "true";
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 退款
        /// </summary>
        /// <param name="money"></param>
        /// <param name="trade_No"></param>
        /// <param name="batch_no"></param>
        /// <param name="batch_num"></param>
        /// <param name="reason"></param>
        public static string ReimburseOrder(string money, string trade_No, string batch_no, string batch_num = "1", string reason = "协商退款")
        {
            string detail_data = trade_No + "^" + money + "^" + reason;
            //
            SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            sParaTemp.Add("service", AlipayConfig.service);
            sParaTemp.Add("partner", AlipayConfig.partner);
            sParaTemp.Add("_input_charset", AlipayConfig.input_charset.ToLower());
            sParaTemp.Add("notify_url", AlipayConfig.notify_url);
            sParaTemp.Add("seller_user_id", AlipayConfig.seller_user_id);
            sParaTemp.Add("refund_date", AlipayConfig.refund_date);
            sParaTemp.Add("batch_no", batch_no);
            sParaTemp.Add("batch_num", batch_num);
            sParaTemp.Add("detail_data", detail_data);
            string sHtmlText = AlipaySubmit.BuildRequest(sParaTemp, "post", "确认");
            return sHtmlText;
        }
    }
}
