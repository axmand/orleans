using Engine.Facility.ECMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Engine.Facility.Helper
{
    /// <summary>
    /// post数据带有校验需要的数据结构
    /// </summary>
    public class CMSPostData
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string userName { get; set; }

        /// <summary>
        /// 令牌
        /// </summary>
        public string token { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string content { get; set; }
    }

    public class CMSHelper
    {

        /// <summary>
        /// 辅助判断接口是否需要配置用户权限
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool CheckAPIConfigurable(Type t)
        {
            var result = Attribute.GetCustomAttributes(t).Where(o => o is CMSAttribute && ((CMSAttribute)o).Configurable);
            return result.Any();
        }

        /// <summary>
        /// 可配置的接口
        /// </summary>
        /// <returns></returns>
        public static Task<Dictionary<string, string>> GetConfigurableAPIList()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            System.Reflection.Assembly.GetEntryAssembly().GetExportedTypes().ToList().ForEach(p=> {
                Attribute.GetCustomAttributes(p).ToList().ForEach(o =>
                {
                    if (o is CMSAttribute && ((CMSAttribute)o).Configurable)
                        if (!dict.ContainsKey(p.FullName))
                            dict.Add(p.FullName, ((CMSAttribute)o).Description);
                });
            });
            return Task.FromResult(dict);
        }

        /// <summary>
        /// 解析CMS校验数据结构
        /// </summary>
        /// <param name="rawText"></param>
        /// <returns></returns>
        public static (string userName, string token, string content) SerializeText(string rawText)
        {
            CMSPostData d = Newtonsoft.Json.JsonConvert.DeserializeObject<CMSPostData>(rawText);
            return (d?.userName, d?.token, d?.content);
        }
    }
}
