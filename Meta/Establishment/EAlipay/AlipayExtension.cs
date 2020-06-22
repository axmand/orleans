using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Engine.Facility.EAlipay
{
    /// <summary>
    /// 根据alipay需求，提供string序列化扩展方法
    /// </summary>
    public static class AlipayStringExtension
    {
        /// <summary>
        ///  文本序列化成AlipayBaseNotify对象
        /// </summary>
        /// <param name="context">待序列化文本（字符串）</param>
        /// <returns>AlipayBaseNotify对象，序列化失败返回null</returns>
        public static AlipayBaseNotify ConvertToBaseNotify(this string context)
        {
            try
            {
                return JsonConvert.DeserializeObject<AlipayBaseNotify>(context);
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        ///  文本序列化成AlipayMobileNotify对象
        /// </summary>
        /// <param name="context">待序列化文本（字符串）</param>
        /// <returns>AlipayMobileNotify对象，序列化失败返回null</returns>
        public static AlipayMobileNotify ConvertToMobileNotify(this string context)
        {
            try
            {
                return JsonConvert.DeserializeObject<AlipayMobileNotify>(context);
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 文本序列化成AlipayRefundNotify对象
        /// </summary>
        /// <param name="context">待序列化文本（字符串）</param>
        /// <returns>AlipayRefundNotify对象，序列化失败返回null</returns>
        public static AlipayRefundNotify ConvertToRefundNotify(this string context)
        {
            try
            {
                return JsonConvert.DeserializeObject<AlipayRefundNotify>(context);
            }
            catch
            {
                return null;
            }
        }
    }
}
