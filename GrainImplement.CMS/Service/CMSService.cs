using Customer.Entity;
using Establishment.EResponse;
using GrainImplement.CMS.Persistence;
using GrainInterface.CMS;
using Newtonsoft.Json;
using Orleans.Runtime;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrainImplement.CMS.Service
{
    public class CMSService : Orleans.Grain, ICMS
    {

        readonly IPersistentState<CustomerManager> _customerManager;

        readonly IPersistentState<GroupManager> _groupManager;

        public CMSService(
            [PersistentState("CustomerManager", "CustomerManagerCache")] IPersistentState<CustomerManager> customerManager,
            [PersistentState("GroupManager", "GroupManagerCache")] IPersistentState<GroupManager> groupManager)
        {
            _customerManager = customerManager;
            _groupManager = groupManager;
        }

        Task<bool> ICMS.CreateGroup(string userName, string token, string groupName, string groupDesc, int groupLevel)
        {
            return Task.FromResult(true);
        }

        Task<bool> ICMS.DeleteGroup(string userName, string token, string groupObjectId)
        {
            return Task.FromResult(true);
        }

        Task<List<Group>> ICMS.GetGroupList(string userName, string token)
        {
            return Task.FromResult(new List<Group>());
        }

        Task<User> ICMS.Login(string rawData)
        {
            throw new System.NotImplementedException();
        }

        Task<string> ICMS.Register(string raw)
        {
            User user = JsonConvert.DeserializeObject<User>(raw);
            //1. 检查字段是否合理
            if (!user.Verify()) return Task.FromResult(new FailResponse("用户名/密码不符合规则").ToString());
            //2. 检查用户名是否已注册
            _customerManager.ReadStateAsync();
            User result = _customerManager.State.CustomerCollection.Find(p => string.Equals(p.userName, user.userName));
            if (result != null)
                return Task.FromResult(new FailResponse("用户已存在").ToString());
            else
                _customerManager.State.CustomerCollection.Add(user);
            //3.同步多端
            _customerManager.WriteStateAsync();
            return Task.FromResult(new OkResponse("用户注册成功").ToString());
        }

        Task<bool> ICMS.SearchCustomerByName(string searchWord, string userName, string token)
        {
            return Task.FromResult(true);
        }

        Task<bool> ICMS.SetCustomerGroup(string userName, string token, string customerObjectId, string groupObjectId)
        {
            return Task.FromResult(true);
        }
    }
}
