using ServiceStack;
using ServiceStack.Web;

namespace Client.HanYangYun.Routes
{
    /// <summary>
    /// 
    /// </summary>
    [Api("提供楼宇和土地信息的geo格式数据，输入字段name,值为 'TDXX' 或者 'LYXX' ")]
    [Route("/bms/geoprovider/{name}", "GET")]
    public class BMSGeoDataProvider
    {
        public string name { get; set; }
    }

    [Api("修改土地信息")]
    [Route("/bms/geoprovider/{name}", "POST")]
    public class BMSTDXXGeoDataUpdate: IRequiresRequestStream
    {
        public System.IO.Stream RequestStream { get; set; }
    }

    [Api("修改楼宇信息")]
    [Route("/bms/geoprovider/{name}", "POST")]
    public class BMSLYXXGeoDataUpdate : IRequiresRequestStream
    {
        public System.IO.Stream RequestStream { get; set; }
    }

}
