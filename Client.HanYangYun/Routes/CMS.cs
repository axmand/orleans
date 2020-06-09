using ServiceStack;

namespace Client.HanYangYun.Routes
{
    /// <summary>
    /// 用户注册
    /// </summary>
    [Route("/customer/register/{userName}/{pwd}", "GET")]
    public class CustomerRegister
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string userName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string pwd { get; set; }
    }
}
