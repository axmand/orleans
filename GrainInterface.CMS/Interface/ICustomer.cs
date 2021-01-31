using Orleans;
using System.Threading.Tasks;

namespace GrainInterface.CMS
{
    /// <summary>
    /// 用户相关功能
    /// </summary>
    public interface ICustomer : IGrainWithIntegerKey
    {
        /// <summary>
        /// 系统初始化校验 
        /// </summary>
        /// <returns></returns>
        Task<bool> InitialCheck();

        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        Task<string> Register(string rawText);

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        Task<string> Login(string rawText);

        /// <summary>
        /// 通过用户名搜索用户，有权限要求
        /// </summary>
        /// <param name="searchWord"></param>
        /// <param name="userName"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<string> SearchCustomerByName(string searchWord, string userName, string token);
    }
}
