namespace CAS.Entity.BonaServiceEntity.Response
{
    /// <summary>
    /// 委托业务表单
    /// </summary>
    public class EntrustFormEntity
    {
        public string BUYERIDNUM { get; set; }
        public string HOUSENO { get; set; }
        public string PROPERTYNO { get; set; }
        public string SERIALNO { get; set; }
        public string PERSONNAME { get; set; }
        public string PERSONCITY { get; set; }
        public string BUYERPHONE { get; set; }
        public string BUSINESSTYPE { get; set; }
        public string ADDRESS { get; set; }
        public string MORTGAGETYPE { get; set; }
        public string CONTRACTPHONE { get; set; }
        public string PROJECTNAME { get; set; }
        public string LENDINGBANK { get; set; }
        public string OTHERLENDINGBANK { get; set; }
        public string BUYERCENSUSREGISTER { get; set; }
        /// <summary>
        /// 拟贷金额
        /// </summary>
        public string PREPARELOANAMOUNT { get; set; }

        /// <summary>
        /// 首付金额
        /// </summary>
        public string DOWNPAYMENT { get; set; }
        public string BUYERNAME { get; set; }
        public string INPUTDATE { get; set; }
        public string IDNUM { get; set; }
        public string AREACODE { get; set; }

        /// <summary>
        /// 开发商
        /// </summary>
        public string DEVELOPERS { get; set; }
        /// <summary>
        /// 贷款人角色
        /// </summary>
        public string LENDERROLE { get; set; }

        public string OWNERCENSUSREGISTER { get; set; }
    }
}
