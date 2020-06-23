using Engine.Facility.ESchema;

namespace Engine.Facility.ECMS
{
    public class Group : MongoSchema
    {
        /// <summary>
        /// 用户组权限层级
        /// </summary>
        public int level { get; set; }

        /// <summary>
        /// 用户组描述
        /// </summary>
        public string description { get; set; }

        /// <summary>
        /// 用户组名称
        /// </summary>
        public string groupName { get; set; }
    }
}
