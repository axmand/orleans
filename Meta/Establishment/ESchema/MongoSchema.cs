using System;

namespace Establishment.ESchema
{
    /// <summary>
    /// mongodb 基础数据类型
    /// </summary>
    public class MongoSchema
    {
        public string objectId { get; set; }

        public string date { get; set; }

        public string time { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        public MongoSchema()
        {
            objectId = Guid.NewGuid().ToString().Replace("-", "");
            date = DateTime.Now.ToLongDateString();
            time = DateTime.Now.ToLongTimeString();
        }

        /// <summary>
        /// 对象校验
        /// </summary>
        /// <returns></returns>
        public virtual bool Verify()
        {
            return true;
        }

        /// <summary>
        /// 设置类型属性
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        /// <param name="value">属性值</param>
        public bool SetValue(string propertyName, object value)
        {
            try
            {
                Type t = this.GetType();
                t.GetProperty(propertyName).SetValue(this, value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// copy类，只copy public 类型，且为浅拷贝
        /// 拷贝成功则为新的对象，失败则为 null
        /// </summary>
        public T Copy<T>() where T : MongoSchema, new()
        {
            var result = new T();
            try
            {
                Type t = typeof(T);
                foreach (var element in t.GetProperties())
                {
                    if (element.CanWrite)
                    {
                        element.SetValue(result, element.GetValue(this));
                    }
                }
                return result;
            }
            catch
            {
                return null;
            }
        }
    }
}
