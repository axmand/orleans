using ServiceStack;
using ServiceStack.Web;

namespace Client.HanYangYun.Routes
{
    /// <summary>
    /// 用户注册
    /// </summary>
    [Route("/customer/register", "POST")]
    public class Register: IRequiresRequestStream
    {
        public System.IO.Stream RequestStream { get; set; }
    }

    [Api("用户登录(POST），")]
    [Route("/customer/login", "POST")]
    public class Login: IRequiresRequestStream
    {
        public System.IO.Stream RequestStream { get; set; }
    }

    /// <summary>
    /// 创建用户组
    /// </summary>
    [Route("/customer/creategroup/{userName}/{token}/{groupName}/{groupDesc}/{groupLevel}", "GET")]
    public class CreateGroup
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
