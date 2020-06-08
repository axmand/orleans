using ServiceStack;

namespace Client.HY.Routes
{
    /// <summary>
    /// 获取数据接口
    /// </summary>
    [Route("/data/{name}", "GET")]
    public class DataProvider
    {
        /// <summary>
        /// 获取指定数据
        /// </summary>
        public string name { get; set; }
    }

    /// <summary>
    /// 文件上传接口
    /// </summary>
    [Route("/data/upload", "GET")]
    public class DataUpload
    {

    }

    /// <summary>
    /// 楼宇详情
    /// Building Management Service
    /// </summary>
    [Route("/bms/{name}", "GET")]
    public class BMSProvider
    {
        public string name { get; set; }
    }

    /// <summary>
    /// 地块管理
    /// Land Management Service
    /// </summary>
    [Route("/lms/{name}", "GET")]
    public class LMSProvider
    {
        public string name { get; set; }
    }
}
