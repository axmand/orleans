namespace Establishment.EResponse
{
    public class FailResponse : BaseResponse
    {
        public FailResponse(object _content, string _code = "-1") : base("fail", _content, _code) { }
    }
}
