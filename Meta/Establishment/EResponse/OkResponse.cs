namespace Engine.Facility.EResponse
{
    public class OkResponse : ServiceResponse
    {
        public OkResponse(object _content, string _code = "-1") : base("ok", _content, _code) { }
    }
}
