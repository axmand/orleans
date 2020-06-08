using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;

namespace Establishment.EWxpay
{
    public class WxpayCore
    {
        /// <summary>
        /// 获取大写的MD5签名结果
        /// </summary>
        /// <param name="encypStr"></param>
        /// <param name="charset"></param>
        public static string GetMD5(string encypStr, string charset)
        {
            string retStr;
            MD5CryptoServiceProvider m5 = new MD5CryptoServiceProvider();
            //创建md5对象
            byte[] inputBye;
            byte[] outputBye;
            //使用GB2312编码方式把字符串转化为字节数组．
            try
            {
                inputBye = Encoding.GetEncoding(charset).GetBytes(encypStr);
            }
            catch
            {
                inputBye = Encoding.GetEncoding("GB2312").GetBytes(encypStr);
            }
            outputBye = m5.ComputeHash(inputBye);
            retStr = System.BitConverter.ToString(outputBye);
            retStr = retStr.Replace("-", "").ToUpper();
            return retStr;
        }
        /// <summary>
        /// 生成随机字符串
        /// </summary>
        /// <returns></returns>
        public static string GetNoncestr()
        {
            Random random = new Random();
            return GetMD5(random.Next(1000).ToString(), "GBK").ToLower().Replace("s", "S");
        }
        /// <summary>
        /// 时间截，自1970年以来的秒数
        /// </summary>
        public static string GetTimestamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }
        /// <summary>
        /// 获取微信签名
        /// </summary>
        /// <param name="sParams"></param>
        public static string GetSign(SortedDictionary<string, string> sParams, string key)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, string> temp in sParams)
            {
                if (temp.Value == "" || temp.Value == null || temp.Key.ToLower() == "sign")
                    continue;
                sb.Append(temp.Key.Trim() + "=" + temp.Value.Trim() + "&");
            }
            sb.Append("key=" + key.Trim() + "");
            string signkey = sb.ToString();
            byte[] ipwd = Encoding.UTF8.GetBytes(signkey);
            byte[] outPwd = md5.ComputeHash(ipwd);
            string sign = BitConverter.ToString(outPwd).Replace("-", "");
            return sign;
        }
        /// <summary>
        /// post数据到指定接口并返回数据
        /// </summary>
        public static string PostXmlToUrl(string url, string postData)
        {
            //formData用于保存提交的信息
            //string formData = HttpUtility.UrlEncode(postData);
            string formData = postData;
            //把提交的信息转码（post提交必须转码）
            byte[] data = Encoding.UTF8.GetBytes(formData);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";    //提交方式：post
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            Stream newStream = request.GetRequestStream();
            newStream.Write(data, 0, data.Length);//将请求的信息写入request
            newStream.Close();
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Stream s = response.GetResponseStream();
                    StreamReader sr = new StreamReader(s, Encoding.GetEncoding("utf-8"));
                    string strResult = sr.ReadToEnd();
                    sr.Close();
                    return strResult;
                }
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 带有证书认证的提交方式
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        public static string PostCertXmlToUrl(string url, string postData, byte[] cert)
        {
            string password = WxpayConfig.MerchantId;
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            X509Certificate cer = new X509Certificate(cert, password);
            //formData用于保存提交的信息
            //string formData = HttpUtility.UrlEncode(postData);
            string formData = postData;
            //把提交的信息转码（post提交必须转码）
            byte[] data = Encoding.UTF8.GetBytes(formData);
            //
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ClientCertificates.Add(cer);
            request.Method = "POST";    //提交方式：post
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            Stream newStream = request.GetRequestStream();
            newStream.Write(data, 0, data.Length);//将请求的信息写入request
            newStream.Close();
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Stream s = response.GetResponseStream();
                    StreamReader sr = new StreamReader(s, Encoding.GetEncoding("utf-8"));
                    string strResult = sr.ReadToEnd();
                    sr.Close();
                    return strResult;
                }
                //向服务器发送请求
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        /// <summary>
        /// 校验
        /// </summary>
        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            if (errors == SslPolicyErrors.None)
                return true;
            return false;
        }
        /// <summary>
        /// 获取prepay_id
        /// </summary>
        public string GetPrepayId(WxUnifiedOrder order, string key)
        {
            string prepay_id = "";
            string post_data = GetUnifiedOrderXml(order, key);
            string request_data = PostXmlToUrl(WxpayConfig.UnifiedPayUrl, post_data);
            SortedDictionary<string, string> requestXML = GetInfoFromXml(request_data);
            foreach (KeyValuePair<string, string> k in requestXML)
            {
                if (k.Key == "prepay_id")
                {
                    prepay_id = k.Value;
                    break;
                }
            }
            return prepay_id;
        }
        /// <summary>
        /// 装配xml格式文本
        /// </summary>
        /// <param name="sParams"></param>
        public static string BuildXmlString(SortedDictionary<string, string> sParams)
        {
            StringBuilder sbPay = new StringBuilder();
            foreach (KeyValuePair<string, string> k in sParams)
                sbPay.Append("<" + k.Key + ">" + k.Value + "</" + k.Key + ">");
            string xmlData = string.Format("<xml>{0}</xml>", sbPay.ToString());
            return Encoding.GetEncoding("utf-8").GetString(Encoding.UTF8.GetBytes(xmlData));
        }
        /// <summary>
        /// 组织退款xml请求参数
        /// </summary>
        /// <param name="notifyOrder"></param>
        /// <param name="totol_fee"></param>
        /// <param name="batchNo"></param>
        /// <param name="key"></param>
        public static string GetRefundOrderXml(WxpayMobileNotify notifyOrder, double totol_fee, string batchNo, string key)
        {
            SortedDictionary<string, string> sParams = new SortedDictionary<string, string>();
            sParams.Add("appid", notifyOrder.appid);
            sParams.Add("mch_id", notifyOrder.mch_id);
            sParams.Add("nonce_str", GetNoncestr());
            sParams.Add("op_user_id", notifyOrder.mch_id);
            sParams.Add("out_refund_no", batchNo);
            sParams.Add("out_trade_no", notifyOrder.out_trade_no);
            sParams.Add("refund_fee", totol_fee.ToString());
            sParams.Add("total_fee", notifyOrder.total_fee);
            sParams.Add("refund_fee_type", notifyOrder.fee_type);
            sParams.Add("sign", GetSign(sParams, key));
            return BuildXmlString(sParams);
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
        /// <summary>
        /// 微信统一下单接口xml参数整理
        /// </summary>
        /// <param name="order">微信支付参数实例</param>
        /// <param name="key">密钥</param>
        public static string GetUnifiedOrderXml(WxUnifiedOrder order, string key)
        {
            return WxpayCore.GetUnifiedOrderXml(order, key);
        }
    }
}
