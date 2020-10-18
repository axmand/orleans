using ServiceStack.ServiceHost;

namespace Client.HanYangYun.Routes
{
    /// <summary>
    /// 用户注册
    /// </summary>
    [Api("用户注册（POST，传入的json对象包含两个属性字段：userName 和 userPwd")]
    [Route("/cms/register", "POST")]
    public class CMSRegister: IRequiresRequestStream
    {
        public System.IO.Stream RequestStream { get; set; }
    }

    [Api("用户登录（POST），传入的json对象包含两个属性字段：userName 和 userPwd")]
    [Route("/cms/login", "POST")]
    public class CMSLogin: IRequiresRequestStream
    {
        public System.IO.Stream RequestStream { get; set; }
    }

    /// <summary>
    /// 创建用户组
    /// </summary>
    [Api("创建用户组")]
    [Route("/cms/creategroup/{userName}/{token}/{groupName}/{groupDesc}/{groupLevel}", "GET")]
    public class CMSCreateGroup
    {
        /// <summary>
        /// 用户名
        /// 
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

    [Api("删除用户组")]
    [Route("/cms/deletegroup/{userName}/{token}/{groupObjectId}", "GET")]
    public class CMSDeleteGroup
    {
        public string userName { get; set; }

        public string token { get; set; }

        public string groupObjectId { get; set; }
    }

    [Api("获取用户组列表")]
    [Route("/cms/getgrouplist/{userName}/{token}", "GET")]
    public class CMSGetGroupList
    {
        public string userName { get; set; }

        public string token { get; set; }
    }

    [Api("设置用户分组")]
    [Route("/cms/searchcustomerbyname/{userName}/{token}/{searchWord}", "GET")]
    public class CMSSearchCustomerByName
    {
        public string userName { get; set; }

        public string token { get; set; }

        public string searchWord { get; set; }
    }

    [Api("设置用户分组")]
    [Route("/cms/setcustomergroup/{userName}/{token}/{customerObjectId}/{groupObjectId}", "GET")]
    public class CMSSetCustomerGroup
    {
        public string userName { get; set; }

        public string token { get; set; }

        public string customerObjectId { get; set; }

        public string groupObjectId { get; set; }
    }

    [Api("获取CMS可配置支持的接口列表，用于赋权限")]
    [Route("/cms/getconfigureableapilist", "GET")]
    public class CMSConfigureableAPIList
    {
    }

    [Api("授权用户组API访问权限")]
    [Route("/cms/authorizegroupapi/{userName}/{token}/{groupObjectId}/{APIFullname}", "GET")]
    public class CMSAPIAuthorize
    {
        public string userName { get; set; }

        public string token { get; set; }

        public string groupObjectId { get; set; }

        public string APIFullname { get; set; }
    }

    [Api("解除用户组API访问权限")]
    [Route("/cms/withdrawgroupapi/{userName}/{token}/{groupObjectId}/{APIFullname}", "GET")]
    public class CMSAPIWithdraw
    {
        public string userName { get; set; }

        public string token { get; set; }

        public string groupObjectId { get; set; }

        public string APIFullname { get; set; }
    }

}
