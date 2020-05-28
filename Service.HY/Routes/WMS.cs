using ServiceStack;

namespace Client.HY.Routes
{
    [Route("/layer/google/{z}/{x}/{y}", "GET")]
    public class GoogleLayer
    {
        public int z { get; set; }
        public int x { get; set; }
        public int y { get; set; }
    }
}
