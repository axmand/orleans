using Orleans;
using System.Threading.Tasks;

namespace GrainInterface.CMS
{
    public interface ICMS : IGrainWithIntegerKey
    {

        /// <summary>
        /// 系统初始化校验 
        /// </summary>
        /// <returns></returns>
        Task<bool> InitialCheck();

        /// <summary>
        /// 获取权限等级
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<int> GetAccessLevel(string userName, string token);

        /// <summary>
        /// 创建group，有权限要求
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="token"></param>
        /// <param name="groupName"></param>
        /// <param name="groupDesc"></param>
        /// <param name="groupLevel"></param>
        /// <returns></returns>
        Task<string> CreateGroup(string userName, string token, string groupName, string groupDesc, int groupLevel);

        /// <summary>
        /// 删除group，有权限要求
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="token"></param>
        /// <param name="groupObjectId"></param>
        /// <returns></returns>
        Task<string> DeleteGroup(string userName, string token, string groupObjectId);

        /// <summary>
        /// 获取列表，有权限要求
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<string> GetGroupList(string userName, string token);

        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        Task<string> Register(string raw);

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        Task<string> Login(string rawData);

        /// <summary>
        /// 通过用户名搜索用户，有权限要求
        /// </summary>
        /// <param name="searchWord"></param>
        /// <param name="userName"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<string> SearchCustomerByName(string searchWord, string userName, string token);

        /// <summary>
        /// 设置用户进不同用户组
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="token"></param>
        /// <param name="customerObjectId"></param>
        /// <param name="groupObjectId"></param>
        /// <returns></returns>
        Task<string> SetCustomerGroup(string userName, string token, string customerObjectId, string groupObjectId);
    }
}
