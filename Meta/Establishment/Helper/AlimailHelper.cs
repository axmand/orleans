using Engine.Facility.EAlimail;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace Engine.Facility.Helper
{
    /// <summary>
    /// 阿里云企业邮箱
    /// </summary>
    public class AlimailHelper
    {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="mailTo">客户邮箱地址</param>
        /// <param name="content">邮件内容</param>
        public static bool SendMail(string mailTo,string content)
        {
            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential(AlimailConfig.mailServerName, AlimailConfig.mailServerPwd);
            client.Host = AlimailConfig.mailServerSmtpUrl;
            client.Port = AlimailConfig.mailServerPort;
            //
            MailAddress fromAddress = new MailAddress(AlimailConfig.mailServerName, AlimailConfig.serverTitle + "安全服务中心", Encoding.UTF8);
            MailAddress toAddress = new MailAddress(mailTo);
            //
            MailMessage mailObj = new MailMessage(fromAddress, toAddress);
            mailObj.SubjectEncoding = Encoding.UTF8;
            mailObj.IsBodyHtml = true;
            mailObj.Priority = MailPriority.High;
            mailObj.To.Add(toAddress);
            mailObj.Subject = AlimailConfig.serverTitle + "验证码";
            mailObj.Body = "尊敬的" + AlimailConfig.serverTitle + "用户，您好 ：<br/>" + "您的验证码是：" + content + "<br/>验证码将在使用一次后失效。" + "</br>此邮件为系统邮件，请勿回复";
            try
            {
                client.Send(mailObj);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 正则匹配邮箱
        /// </summary>
        /// <param name="mailAddress"></param>
        public static bool MatchMailAddress(string mailAddress)
        {
            Regex regexMail = new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
            return regexMail.IsMatch(mailAddress);
        }
    }
}
