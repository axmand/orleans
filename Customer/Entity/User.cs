using Establishment.ESchema;

namespace Customer.Entity
{
    public class User : MongoSchema
    {
        /// <summary>
        /// 登录用户名
        /// </summary>
        public string userName { get; set; }

        /// <summary>
        /// 注册密码
        /// </summary>
        public string userPwd { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string token { get; set; }

        /// <summary>
        /// 第三方登录openId存留
        /// </summary>
        public string openId { get; set; }
    }
}
