using Engine.Facility.EWxpay;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Engine.Facility.Helper
{
    /// <summary>
    /// 微信支付功能集合
    /// </summary>
    public class WxpayHelper
    {
        /// <summary>
        /// 微信线上线下统一订单创建
        /// </summary>
        /// <param name="orderName">订单名称</param>
        /// <param name="tradeNo">订单编号</param>
        /// <param name="totalPayPrice">订单金额</param>
        /// <param name="unionId">用户id，微信登录unionId</param>
        /// <param name="createIp">用户设备ip</param>
        /// <param name="tradeType">发起支付设备类型</param>
        /// <returns>创建错误时，返回null</returns>
        public static WxpayPreOrder CreateUinfiedOrder(string orderName,string tradeNo,double totalPayPrice, string unionId, string createIp, int tradeType =(int)EWxpayMethod.APP)
        {
            //判断支付途径是否有效
            if (!Enum.IsDefined(typeof(EWxpayMethod), tradeType))
                return null;
            //创建微信统一下单
            WxUnifiedOrder unifiedOrder = new WxUnifiedOrder()
            {
                //config
                appid = WxpayConfig.AppId,
                mch_id = WxpayConfig.MerchantId,
                notify_url = WxpayConfig.NotifyUrl,
                //购买者id
                openid = unionId != null && unionId.Length > 0 ? unionId : null,
                body = WxpayConfig.AppName + "-" + orderName,
                nonce_str = WxpayCore.GetNoncestr(),//
                out_trade_no = tradeNo,
                spbill_create_ip = createIp,
                total_fee = Convert.ToInt32(totalPayPrice * 100),
                trade_type = ((EWxpayMethod)tradeType).ToString(),
                //null
                attach = null,
                device_info = null,
            };
            string proOrder = WxpayCore.GetUnifiedOrderXml(unifiedOrder, WxpayConfig.AppKey);
            var result = WxpayCore.PostXmlToUrl(WxpayConfig.UnifiedPayUrl, proOrder);
            var xmlInfo = WxpayCore.GetInfoFromXml(result);
            //解析成功后返回支付订单
            if (xmlInfo.ContainsKey("return_code"))
                if (xmlInfo["return_code"] == "SUCCESS")
                {
                    SortedDictionary<string, string> sParams = new SortedDictionary<string, string>();
                    sParams.Add("appid", WxpayConfig.AppId);
                    sParams.Add("noncestr", WxpayCore.GetNoncestr());
                    sParams.Add("package", "Sign=WXPay");
                    sParams.Add("partnerid", WxpayConfig.MerchantId);
                    sParams.Add("prepayid", xmlInfo["prepay_id"]);
                    sParams.Add("timestamp", WxpayCore.GetTimestamp());
                    string sign = WxpayCore.GetSign(sParams, WxpayConfig.AppKey);
                    sParams.Add("sign", sign);
                    //写入订单
                    return new WxpayPreOrder()
                    {
                        appid = sParams["appid"],
                        noncestr = sParams["noncestr"],
                        package = sParams["package"],
                        partnerid = sParams["partnerid"],
                        prepayid = sParams["prepayid"],
                        timestamp = sParams["timestamp"],
                        sign = sParams["sign"],
                    };
                }
            //失败必然返回错误信息
            return null;
        }
        /// <summary>
        /// 微信退款
        /// </summary>
        /// <param name="total_fee">退费总金额</param>
        /// <param name="tradeNo">订单编号</param>
        /// <param name="batch_no">退款批次</param>
        /// <param name="mobileNotify">微信支付成功的返回信息</param>
        /// <returns></returns>
        public static WxpayRefundNotify ReimburseOrder(double total_fee, string tradeNo, string batch_no, WxpayMobileNotify mobileNotify)
        {
            string postXmlData = WxpayCore.GetRefundOrderXml(mobileNotify, total_fee, batch_no, WxpayConfig.AppKey);
            //}{这里需要证书验证才可以申请退款操作
            var result = WxpayCore.PostCertXmlToUrl(WxpayConfig.RefundOrderUrl, postXmlData, WxpayConfig.AppCert);
            var xmlInfo = WxpayCore.GetInfoFromXml(result);
            string serializeStr = Newtonsoft.Json.JsonConvert.SerializeObject(xmlInfo);
            WxpayRefundNotify wxpayRefundNotify = Newtonsoft.Json.JsonConvert.DeserializeObject<WxpayRefundNotify>(serializeStr);
            return wxpayRefundNotify;
        }
        /// <summary>
        /// 把XML数据转换为SortedDictionary<string, string>集合
        /// </summary>
        /// <param name="strxml"></param>
        public static SortedDictionary<string, string> GetInfoFromXml(string xmlstring)
        {
            SortedDictionary<string, string> sParams = new SortedDictionary<string, string>();
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlstring);
                XmlElement root = doc.DocumentElement;
                int len = root.ChildNodes.Count;
                for (int i = 0; i < len; i++)
                {
                    string name = root.ChildNodes[i].Name;
                    if (!sParams.ContainsKey(name))
                    {
                        sParams.Add(name.Trim(), root.ChildNodes[i].InnerText.Trim());
                    }
                }
            }
            catch { }
            return sParams;
        }
    }
}
