using ServiceStack;

namespace Client.HanYangYun.Routes
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
    /// 
    /// </summary>
    [Api("提供楼宇和土地信息的geo格式数据，输入字段name,值为 'TDXX' 或者 'LYXX' ")]
    [Route("/bms/geoprovider/{name}", "GET")]
    public class BMSGeoProvider
    {
        public string name { get; set; }
    }
}
