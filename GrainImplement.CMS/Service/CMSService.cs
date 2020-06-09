using Customer.Entity;
using GrainImplement.CMS.Persistence;
using GrainInterface.CMS;
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

        Task<User> ICMS.Login(string userName, string pwd)
        {
            return Task.FromResult(new User());
        }

        Task<bool> ICMS.Register(string userName, string pwd)
        {
            return Task.FromResult(true);
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
