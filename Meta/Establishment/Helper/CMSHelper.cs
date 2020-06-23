using Engine.Facility.ECMS;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Engine.Facility.Helper
{
    public class CMSHelper
    {
        /// <summary>
        /// 可配置的接口
        /// </summary>
        /// <returns></returns>
        public static Task<Type[]> GetConfigurableAPIList()
        {
            Type[] types = System.Reflection.Assembly.GetEntryAssembly().GetExportedTypes();
            Func<Attribute[], bool> IsMyAttribute = o =>
            {
                foreach (Attribute a in o)
                    if (a is CMSAttribute)
                        return ((CMSAttribute)a).Configurable;
                return false;
            };
            Type[] cosType = types.Where(o =>
                IsMyAttribute(System.Attribute.GetCustomAttributes(o, true))
            ).ToArray();
            return Task.FromResult(cosType);
        }
    }
}
