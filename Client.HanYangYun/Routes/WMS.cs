using ServiceStack;

namespace Client.HanYangYun.Routes
{
    [Route("/wms/layer/tms/{z}/{x}/{y}", "GET")]
    public class WMSTMSLayer
    {
        public int z { get; set; }
        public int x { get; set; }
        public int y { get; set; }
    }
}
