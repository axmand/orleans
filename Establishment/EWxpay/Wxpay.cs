namespace Establishment.EWxpay
{
    /// <summary>
    /// 微信支付方式枚举
    /// </summary>
    public enum EWxpayMethod
    {
        /// <summary>
        /// app调取微信收银台
        /// </summary>
        APP=1,
        /// <summary>
        /// h5支付
        /// </summary>
        JSAPI=2,
        /// <summary>
        /// native支付
        /// </summary>
        NATIVE=3,
    }
    /// <summary>
    ///  微信订单明细实体对象 
    /// </summary>
    public class WxOrderDetail
    {
        /// <summary>
        /// 返回状态码，SUCCESS/FAIL 此字段是通信标识，非交易标识，交易是否成功需要查看trade_state来判断
        /// </summary>
        public string return_code = "";

        /// <summary>
        /// 返回信息返回信息，如非空，为错误原因 签名失败 参数格式校验错误
        /// </summary>
        public string return_msg = "";

        /// <summary>
        /// 公共号ID(微信分配的公众账号 ID)
        /// </summary>
        public string appid = "";

        /// <summary>
        /// 商户号(微信支付分配的商户号)
        /// </summary>
        public string mch_id = "";

        /// <summary>
        /// 随机字符串，不长于32位
        /// </summary>
        public string nonce_str = "";

        /// <summary>
        /// 签名
        /// </summary>
        public string sign = "";

        /// <summary>
        /// 业务结果,SUCCESS/FAIL
        /// </summary>
        public string result_code = "";

        /// <summary>
        /// 错误代码
        /// </summary>
        public string err_code = "";

        /// <summary>
        /// 错误代码描述
        /// </summary>
        public string err_code_des = "";

        /// <summary>
        /// 交易状态
        ///SUCCESS—支付成功
        ///REFUND—转入退款
        ///NOTPAY—未支付
        ///CLOSED—已关闭
        ///REVOKED—已撤销
        ///USERPAYING--用户支付中
        ///NOPAY--未支付(输入密码或确认支付超时) PAYERROR--支付失败(其他原因，如银行返回失败)
        /// </summary>
        public string trade_state = "";

        /// <summary>
        /// 微信支付分配的终端设备号
        /// </summary>
        public string device_info = "";

        /// <summary>
        /// 用户在商户appid下的唯一标识
        /// </summary>
        public string openid = "";

        /// <summary>
        /// 用户是否关注公众账号，Y-关注，N-未关注，仅在公众账号类型支付有效
        /// </summary>
        public string is_subscribe = "";

        /// <summary>
        /// 交易类型,JSAPI、NATIVE、MICROPAY、APP
        /// </summary>
        public string trade_type = "";

        /// <summary>
        /// 银行类型，采用字符串类型的银行标识
        /// </summary>
        public string bank_type = "";

        /// <summary>
        /// 订单总金额，单位为分
        /// </summary>
        public string total_fee = "";

        /// <summary>
        /// 现金券支付金额<=订单总金额，订单总金额-现金券金额为现金支付金额
        /// </summary>
        public string coupon_fee = "";

        /// <summary>
        /// 货币类型，符合ISO 4217标准的三位字母代码，默认人民币：CNY
        /// </summary>
        public string fee_type = "";

        /// <summary>
        /// 微信支付订单号
        /// </summary>
        public string transaction_id = "";

        /// <summary>
        /// 商户系统的订单号，与请求一致。
        /// </summary>
        public string out_trade_no = "";

        /// <summary>
        /// 商家数据包，原样返回
        /// </summary>
        public string attach = "";

        /// <summary>
        /// 支付完成时间，格式为yyyyMMddhhmmss，如2009年12月27日9点10分10秒表示为20091227091010。
        /// 时区为GMT+8 beijing。该时间取自微信支付服务器
        /// </summary>
        public string time_end = "";

    }
    /// <summary>
    /// 微信订单查询接口请求实体对象
    /// </summary>
    public class WxQueryOrder
    {
        /// <summary>
        /// 公共号ID(微信分配的公众账号 ID)
        /// </summary>
        public string appid = "";

        /// <summary>
        /// 商户号(微信支付分配的商户号)
        /// </summary>
        public string mch_id = "";

        /// <summary>
        /// 微信订单号，优先使用
        /// </summary>
        public string transaction_id = "";

        /// <summary>
        /// 商户系统内部订单号
        /// </summary>
        public string out_trade_no = "";

        /// <summary>
        /// 随机字符串，不长于 32 位
        /// </summary>
        public string nonce_str = "";

        /// <summary>
        /// 签名，参与签名参数：appid，mch_id，transaction_id，out_trade_no，nonce_str，key
        /// </summary>
        public string sign = "";
    }
    /// <summary>
    /// 微信统一接口请求实体对象
    /// </summary>
    public class WxUnifiedOrder
    {
        /// <summary>
        /// 公共号ID(微信分配的公众账号 ID)
        /// </summary>
        public string appid = "";
        /// <summary>
        /// 商户号(微信支付分配的商户号)
        /// </summary>
        public string mch_id = "";
        /// <summary>
        /// 微信支付分配的终端设备号
        /// </summary>
        public string device_info = "";
        /// <summary>
        /// 随机字符串，不长于 32 位
        /// </summary>
        public string nonce_str = "";
        /// <summary>
        /// 签名
        /// </summary>
        public string sign = "";
        /// <summary>
        /// 商品描述
        /// </summary>
        public string body = "";
        /// <summary>
        /// 附加数据，原样返回
        /// </summary>
        public string attach = "";
        /// <summary>
        /// 商户系统内部的订单号,32个字符内、可包含字母,确保在商户系统唯一,详细说明
        /// </summary>
        public string out_trade_no = "";
        /// <summary>
        /// 订单总金额，单位为分，不能带小数点
        /// </summary>
        public int total_fee = 0;
        /// <summary>
        /// 终端IP
        /// </summary>
        public string spbill_create_ip = "";
        /// <summary>
        /// 订 单 生 成 时 间 ， 格 式 为yyyyMMddHHmmss，如 2009 年12 月 25 日 9 点 10 分 10 秒表示为 20091225091010。时区为 GMT+8 beijing。该时间取自商户服务器
        /// </summary>
        public string time_start = "";
        /// <summary>
        /// 交易结束时间
        /// </summary>
        public string time_expire = "";
        /// <summary>
        /// 商品标记 商品标记，该字段不能随便填，不使用请填空，使用说明详见第 5 节
        /// </summary>
        public string goods_tag = "";
        /// <summary>
        /// 接收微信支付成功通知
        /// </summary>
        public string notify_url = "";
        /// <summary>
        /// JSAPI、NATIVE、APP
        /// </summary>
        public string trade_type = "";
        /// <summary>
        /// 用户标识 trade_type 为 JSAPI时，此参数必传
        /// </summary>
        public string openid = "";
        /// <summary>
        /// 只在 trade_type 为 NATIVE时需要填写。
        /// </summary>
        public string product_id = "";
    }
    /// <summary>
    /// 微信统一下单数据存储
    /// </summary>
    public class WxpayPreOrder
    {
        public string appid { get; set; }
        public string noncestr { get; set; }
        public string package { get; set; }
        public string partnerid { get; set; }
        public string prepayid { get; set; }
        public string timestamp { get; set; }
        public string sign { get; set; }
    }
    /// <summary>
    /// 微信支付通知
    /// </summary>
    public class WxpayMobileNotify
    {
        public string return_code { get; set; }
        public string return_msg { get; set; }
        public string appid { get; set; }
        //
        public string mch_id { get; set; }
        public string device_info { get; set; }
        public string nonce_str { get; set; }
        public string sign { get; set; }
        public string result_code { get; set; }
        public string err_code { get; set; }
        public string err_code_des { get; set; }
        public string openid { get; set; }
        public string is_subscribe { get; set; }
        public string trade_type { get; set; }
        public string bank_type { get; set; }
        public string total_fee { get; set; }
        public string fee_type { get; set; }
        public string cash_fee { get; set; }
        public string cash_fee_type { get; set; }
        public string coupon_fee { get; set; }
        public string coupon_count { get; set; }
        //public string coupon_id_$n { get; set; }
        //public string coupon_fee_$n { get; set; }
        public string transaction_id { get; set; }
        public string out_trade_no { get; set; }
        public string attach { get; set; }
        public string time_end { get; set; }
    }
    /// <summary>
    /// 微信退款通知
    /// </summary>
    public class WxpayRefundNotify
    {
        public string return_code { get; set; }
        public string return_msg { get; set; }

        public string err_code { get; set; }
        public string err_code_des { get; set; }

        public string appid { get; set; }
        public string mch_id { get; set; }
        public string nonce_str { get; set; }
        public string sign { get; set; }
        public string transaction_id { get; set; }
        public string out_trade_no { get; set; }
        public string out_refund_no { get; set; }
        public string refund_id { get; set; }
        public string refund_fee { get; set; }
        public string total_fee { get; set; }
        public string cash_fee { get; set; }
    }
}
