namespace CAS.Entity.GJBEntity
{
    public class MessageTemp
    {
        /// <summary>
        /// 业务类型，预评、业务、报告、询价等
        /// </summary>
        public string businessType { get; set; }
        /// <summary>
        /// 发起人
        /// </summary>
        public string fromUserName { get; set; }
        /// <summary>
        /// 完成人
        /// </summary>
        public string toUserName { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string projectName { get; set; }
        /// <summary>
        /// 委托类型 对公、个人或者询价类型(住宅、商业、办公等)
        /// </summary>
        public string typecodename { get; set; }
        /// <summary>
        /// 消息模板类型
        /// </summary>
        public string messagetype { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public decimal unitprice { get; set; }
        /// <summary>
        /// 总价
        /// </summary>
        public decimal totalprice { get; set; }
        /// <summary>
        /// 业务员
        /// </summary>
        public string saleman { get; set; }
        /// <summary>
        /// 业务员联系方式
        /// </summary>
        public string salemantelephone { get; set; }
        /// <summary>
        /// 面积
        /// </summary>
        public decimal buildarea { get; set; }
        /// <summary>
        /// 税费
        /// </summary>
        public decimal? tax { get; set; }
        /// <summary>
        /// 净值
        /// </summary>
        public decimal? netprice { get; set; }
        /// <summary>
        /// 多个税费和净值
        /// </summary>
        public string moretaxandnetprice { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        public string  queryid { get; set; }
        /// <summary>
        /// 业务联系人+电话
        /// </summary>
        public string bankcontactdetail { get; set; }
        /// <summary>
        /// 委托客户
        /// </summary>
        public string bankName { get; set; }
        private string _loanpernumname;
        /// <summary>
        /// 贷款成数
        /// </summary>
        public string loanpernumname
        {
            get { return _loanpernumname; }
            set { _loanpernumname = value; }
        }
        private decimal? _loanablenum;
        /// <summary>
        /// 可贷金额
        /// </summary>
        public decimal? loanablenum
        {
            get { return _loanablenum; }
            set { _loanablenum = value; }
        }
        /// <summary>
        /// 价格说明
        /// </summary>
        public string priceremark { get; set; }
    }
}
