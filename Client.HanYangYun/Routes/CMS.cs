using ServiceStack;
using ServiceStack.Web;

namespace Client.HanYangYun.Routes
{
    /// <summary>
    /// 用户注册
    /// </summary>
    [Route("/cms/register", "POST")]
    public class CMSRegister: IRequiresRequestStream
    {
        public System.IO.Stream RequestStream { get; set; }
    }

    [Api("用户登录(POST），")]
    [Route("/cms/login", "POST")]
    public class CMSLogin: IRequiresRequestStream
    {
        public System.IO.Stream RequestStream { get; set; }
    }

    /// <summary>
    /// 创建用户组
    /// </summary>
    [Route("/cms/creategroup/{userName}/{token}/{groupName}/{groupDesc}/{groupLevel}", "GET")]
    public class CMSCreateGroup
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string userName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string token { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string groupName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string groupDesc { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int groupLevel { get; set; }
    }

    [Route("/cms/deletegroup/{userName}/{token}/{groupObjectId}", "GET")]
    public class CMSDeleteGroup
    {
        public string userName { get; set; }

        public string token { get; set; }

        public string groupObjectId { get; set; }
    }

    [Route("/cms/getgrouplist/{userName}/{token}", "GET")]
    public class CMSGetGroupList
    {
        public string userName { get; set; }

        public string token { get; set; }
    }

    [Route("/cms/searchcustomerbyname/{userName}/{token}/{searchword}", "GET")]
    public class CMSSearchCustomerByName
    {
        public string userName { get; set; }

        public string token { get; set; }

        public string searchword { get; set; }
    }

    [Route("/cms/setcustomergroup/{userName}/{token}/{customerObjectId}/{groupObjectId}", "GET")]
    public class CMSSetCustomerGroup
    {
        public string userName { get; set; }

        public string token { get; set; }

        public string customerObjectId { get; set; }

        public string groupObjectId { get; set; }
    }

}
