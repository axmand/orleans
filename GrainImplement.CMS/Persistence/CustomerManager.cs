using Customer.Entity;
using System.Collections.Generic;

namespace GrainImplement.CMS.Persistence
{
    public class CustomerManager
    {
        public List<User> CustomerCollection { get; set; } = new List<User>();
    }
}
