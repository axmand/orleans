using Engine.Facility.ESchema;

namespace Engine.Facility.ECMS
{
    public class Customer : MongoSchema
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
        /// 用户所在群组 id
        /// </summary>
        public string groupObjectId { get; set; }

        /// <summary>
        /// 第三方登录openId存留
        /// </summary>
        public string openId { get; set; }

        public override bool Verify()
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(userPwd))
                return false;
            return base.Verify();
        }

    }
}
