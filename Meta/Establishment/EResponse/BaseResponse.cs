namespace Establishment.EResponse
{
    public class BaseResponse
    {
        /// <summary>
        /// 错误码
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 错误内容
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 请求结果
        /// </summary>
        public string status { get; set; }

        public BaseResponse(string _status, string _content, string _code = "-1")
        {
            content = _content;
            status = _status;
            if (_code == "-1" && _status == "ok")
                code = "200";
            else if (_code == "-1" && _status == "fail")
                code = "400";
        }

        public BaseResponse(string _status, object _content, string _code = "-1")
        {
            content = Newtonsoft.Json.JsonConvert.SerializeObject(_content);
            status = _status;
            if (_code == "-1" && _status == "ok")
                code = "200";
            else if (_code == "-1" && _status == "fail")
                code = "400";
        }

        /// <summary>
        /// 重写toString，使用newtonsoft序列化对象
        /// </summary>
        public override string ToString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
    }
}
