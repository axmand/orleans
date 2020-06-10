namespace Establishment.EResponse
{
    public class OkResponse : BaseResponse
    {
        public OkResponse(object _content, string _code = "-1") : base("ok", _content, _code) { }
    }
}
