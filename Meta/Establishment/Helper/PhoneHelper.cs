using Establishment.Phone;
using System;
using System.Text.RegularExpressions;

namespace Establishment.Helper
{
    /// <summary>
    /// 短信服务类
    /// </summary>
    public class PhoneHelper
    {
        /// <summary>
        /// 发送短信功能
        /// </summary>
        /// <param name="messageTo">被发送对象</param>
        /// <param name="content"></param>
        public static bool SendMessage(string messageTo, string content)
        {
            if (!MatchPhoneNumber(messageTo))
            {
                Console.Write("短信发送失败，号码格式错误{0}", messageTo);
                return false;
            }
            string[] data = new string[] { content };
            var result = PhoneConfig.api.SendTemplateSMS(messageTo, PhoneConfig.phoneTepmplate, data);
            return true;
        }
        /// <summary>
        /// 验证手机号码-正则表达式
        /// </summary>
        /// <param name="phoneNumber"></param>
        public static bool MatchPhoneNumber(string phoneNumber)
        {
            Regex regexMail = new Regex(@"((\d{11})|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$)");
            return regexMail.IsMatch(phoneNumber);
        }
    }
}
