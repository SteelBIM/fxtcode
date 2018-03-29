namespace CAS.Entity.BonaServiceEntity.Response
{
    /// <summary>
    /// 业务单审核详情信息
    /// </summary>
    public class RecordDetailEntity
    {
        /// <summary>
        /// 业务类型
        /// </summary>
        public string BUSINESSTYPE { get; set; }

        /// <summary>
        /// 按揭类型
        /// </summary>
        public string MORTGAGETYPE { get; set; }

        /// <summary>
        /// 业务类型名字
        /// </summary>
        public string BUSINESSTYPENNAME
        {

            get
            {
                var re = "";
                if (this.BUSINESSTYPE == "A")
                {
                    re = "按揭";
                }
                else if (this.BUSINESSTYPE == "D")
                {
                    re = "抵押";
                }
                return re;

            }
        }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string PROJECTNAME { get; set; }

        /// <summary>
        /// 贷款银行
        /// </summary>
        public string LENDINGBANK { get; set; }

        /// <summary>
        /// 权证信息状态
        /// </summary>
        public string RIGHTSTATUS { get; set; }

        public string AMOUNT { get; set; }

        /// <summary>
        /// 拟贷金额
        /// </summary>
        public string PREPARELOANAMOUNT { get; set; }

        /// <summary>
        /// 首付金额
        /// </summary>
        public string DOWNPAYMENT { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LOANSTATUS { get; set; }

        /// <summary>
        /// 基础信息状态
        /// </summary>
        public string BASEINFOSTATUS { get; set; }

        /// <summary>
        /// 认证进度
        /// </summary>
        public string CERTPROGRESS { get; set; }

        /// <summary>
        /// 客户姓名
        /// </summary>
        public string CUSTOMERNAME { get; set; }



        /// <summary>
        /// 还款状态
        /// </summary>
        public string PAYSTATUS { get; set; }


        /// <summary>
        /// 业务单号
        /// </summary>
        public string SERIALNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LOANAMOUNT { get; set; }

        /// <summary>
        /// 委托人
        /// </summary>
        public string CLIENTMAN { get; set; }

        /// <summary>
        /// 委托人联系电话
        /// </summary>
        public string CONTRACTPHONE
        { get; set; }
        /// <summary>
        /// 委托公司名称
        /// </summary>
        public string CLIENTNAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LOANTIME { get; set; }

        /// <summary>
        /// 委托人电话
        /// </summary>
        public string CLIENTPHONE { get; set; }

        /// <summary>
        /// 录入时间
        /// </summary>
        public string INPUTDATE { get; set; }

        /// <summary>
        /// 身份证
        /// </summary>
        public string IDNUM { get; set; }

        public string CUSTOMERIDNUM { get; set; }


        public string BUYERPHONE { get; set; }

        /// <summary>
        /// 公共
        /// </summary>
        public string INFOPHONE { get; set; }
    }
}
