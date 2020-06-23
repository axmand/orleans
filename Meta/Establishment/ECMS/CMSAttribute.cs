using System;

namespace Engine.Facility.ECMS
{
    /// <summary>
    /// 定义一个CMS属性，提供
    /// 1. 是否需要确认权限
    /// 2. 是否放入API列表
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class CMSAttribute : Attribute
    {
        /// <summary>
        /// 默认可配置，不需要权限可访问
        /// </summary>
        /// <param name="configurable">默认可配置</param>
        /// <param name="permissionRequire">默认不需要权限可访问</param>
        public CMSAttribute(bool configurable = true, bool permissionRequire = false)
        {
            Configurable = configurable;
            PermissionRequire = permissionRequire;
        }

        /// <summary>
        /// Adds a list of configurable interfacess
        /// 是否加入可配置列表
        /// </summary>
        public bool Configurable { get; private set; }

        /// <summary>
        /// 是否需要权限配置才可调用
        /// </summary>
        public bool PermissionRequire { get; private set; }
    }
}
