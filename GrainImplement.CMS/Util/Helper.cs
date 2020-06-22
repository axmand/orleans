using Engine.Facility.EResponse;

namespace GrainImplement.CMS.Util
{
    public class Helper
    {
        /// <summary>
        /// 用户权限
        /// </summary>
        public static class LEVEL
        {
            /// <summary>
            /// 最高权限
            /// </summary>
            public static int ADMINLEVEL = 99;

            /// <summary>
            /// 可自行定义的权限上限
            /// </summary>
            public static int CUSTOMLEVEL = 90;

            /// <summary>
            /// 权限错误反馈值
            /// </summary>
            public static int NOPERMISSIONLEVEL = -1;
        }

        public static string PermissionDeniedResponse = new FailResponse("权限不足").ToString();
    }
}
