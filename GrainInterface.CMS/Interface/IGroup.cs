using Orleans;
using System;
using System.Threading.Tasks;

namespace GrainInterface.CMS
{
    /// <summary>
    /// 用户组相关功能
    /// </summary>
    public interface IGroup: IGrainWithIntegerKey
    {
        /// <summary>
        /// 获取权限等级
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<int> GetGroupLevel(string userName, string token);

        /// <summary>
        ///  检查Api是否允许使用
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="token"></param>
        /// <param name="typeFullname"></param>
        /// <returns></returns>
        Task<bool> CheckAPIPermession(string userName, string token, Type t);

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
        /// 设置用户进不同用户组
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="token"></param>
        /// <param name="customerObjectId"></param>
        /// <param name="groupObjectId"></param>
        /// <returns></returns>
        Task<string> SetCustomerGroup(string userName, string token, string customerObjectId, string groupObjectId);

        /// <summary>
        /// 授权组访问APIAPI
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="token"></param>
        /// <param name="APIFullname"></param>
        /// <returns></returns>
        Task<string> AuthorizeAPI(string userName, string token, string groupObjectId,string APIFullname);

        /// <summary>
        /// 收回API访问授权
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="token"></param>
        /// <param name="groupObjectId"></param>
        /// <param name="APIFullname"></param>
        /// <returns></returns>
        Task<string> WithdrawAPI(string userName, string token, string groupObjectId, string APIFullname);
    }
}
