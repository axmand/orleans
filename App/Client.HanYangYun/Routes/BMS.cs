using Engine.Facility.ECMS;
using ServiceStack.ServiceHost;

namespace Client.HanYangYun.Routes
{
    [Api("汉阳区遥感影像TMS数据服务")]
    [Route("/bms/tmslayer/{z}/{x}/{y}", "GET")]
    public class BMSTMSHYLayer
    {
        public int z { get; set; }
        public int x { get; set; }
        public int y { get; set; }
    }

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
    [Route("/bms/geodatatdxxupdate", "POST")]
    [CMS(desc: "修改土地信息接口，需要用户权限")]
    public class BMSTDXXGeoDataUpdate: IRequiresRequestStream
    {
        public System.IO.Stream RequestStream { get; set; }
    }

    [Api("修改楼宇信息")]
    [Route("/bms/geodatalyxxupdate", "POST")]
    [CMS(desc: "修改楼宇信息接口，需要用户权限")]
    public class BMSLYXXGeoDataUpdate : IRequiresRequestStream
    {
        public System.IO.Stream RequestStream { get; set; }
    }

}
