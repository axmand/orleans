using Customer.Entity;
using Orleans;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrainInterface.CMS
{
    public interface ICMS : IGrainWithIntegerKey
    {
        /// <summary>
        /// 创建group，有权限要求
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="token"></param>
        /// <param name="groupName"></param>
        /// <param name="groupDesc"></param>
        /// <param name="groupLevel"></param>
        /// <returns></returns>
        Task<bool> CreateGroup(string userName, string token, string groupName, string groupDesc, int groupLevel);

        /// <summary>
        /// 删除group，有权限要求
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="token"></param>
        /// <param name="groupObjectId"></param>
        /// <returns></returns>
        Task<bool> DeleteGroup(string userName, string token, string groupObjectId);

        /// <summary>
        /// 获取列表，有权限要求
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<List<Group>> GetGroupList(string userName, string token);

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
        Task<User> Login(string rawData);

        /// <summary>
        /// 通过用户名搜索用户，有权限要求
        /// </summary>
        /// <param name="searchWord"></param>
        /// <param name="userName"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<bool> SearchCustomerByName(string searchWord, string userName, string token);

        /// <summary>
        /// 设置用户进不同用户组
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="token"></param>
        /// <param name="customerObjectId"></param>
        /// <param name="groupObjectId"></param>
        /// <returns></returns>
        Task<bool> SetCustomerGroup(string userName, string token, string customerObjectId, string groupObjectId);
    }
}
