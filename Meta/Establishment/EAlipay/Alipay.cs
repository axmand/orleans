/// <summary>
/// 定义支付宝支付字符串信息中可序列化对象类型
/// </summary>
namespace Engine.Facility.EAlipay
{
    /// <summary>
    /// 基础通知对象
    /// </summary>
    public class AlipayBaseNotify
    {
        /// <summary>
        /// 通知时间
        /// </summary>
        public string notify_time { get; set; }
        /// <summary>
        /// 通知类型
        /// </summary>
        public string notify_type { get; set; }
        /// <summary>
        /// 通知id
        /// </summary>
        public string notify_id { get; set; }
        /// <summary>
        /// 标签加密
        /// </summary>
        public string sign_type { get; set; }
        /// <summary>
        /// 密文
        /// </summary>
        public string sign { get; set; }
        /// <summary>
        /// 构造函数，兼容MongoDB存储与修改
        /// </summary>
        public AlipayBaseNotify()
        {
            notify_time = "";
            notify_type = "";
            notify_id = "";
            sign_type = "";
            sign = "";
        }
    }
    /// <summary>
    /// 移动支付通知对象
    /// </summary>
    public class AlipayMobileNotify : AlipayBaseNotify
    {
        /// <summary>
        /// 商户网站唯一订单号
        /// </summary>
        public string out_trade_no { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string subject { get; set; }
        /// <summary>
        /// 支付类型
        /// </summary>
        public string payment_type { get; set; }
        /// <summary>
        /// 支付宝交易号
        /// </summary>
        public string trade_no { get; set; }
        /// <summary>
        /// 取值状态
        /// </summary>
        public string trade_status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string seller_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string seller_email { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string buyer_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string buyer_email { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string total_fee { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string quantity { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string price { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string body { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string gmt_create { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string gmt_payment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string is_total_fee_adjust { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string use_coupon { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string discount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string refund_status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string gmt_refund { get; set; }
        /// <summary>
        /// 构造函数，兼容MongoDB存储与修改
        /// </summary>
        public AlipayMobileNotify() : base()
        {
            notify_time = "";
            notify_type = "";
            notify_id = "";
            sign_type = "";
            sign = "";
            out_trade_no = "";
            subject = "";
            payment_type = "";
            trade_no = "";
            trade_status = "";
            seller_id = "";
            seller_email = "";
            buyer_id = "";
            buyer_email = "";
            total_fee = "";
            quantity = "";
            price = "";
            body = "";
            gmt_create = "";
            gmt_payment = "";
            is_total_fee_adjust = "";
            use_coupon = "";
            discount = "";
            refund_status = "";
            gmt_refund = "";
        }
    }
    /// <summary>
    /// 有密退款对象
    /// </summary>
    public class AlipayRefundNotify : AlipayBaseNotify
    {
        /// <summary>
        /// 退款批次
        /// </summary>
        public string batch_no { get; set; }
        /// <summary>
        /// 成功退款数目
        /// </summary>
        public string success_num { get; set; }
        /// <summary>
        /// 退款细节
        /// </summary>
        public string result_details { get; set; }
        /// <summary>
        /// 构造函数，兼容MongoDB存储与修改
        /// </summary>
        public AlipayRefundNotify() : base()
        {
            batch_no = "";
            success_num = "";
            result_details = "";
        }
    }
}
