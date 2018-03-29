namespace CAS.Entity.BonaServiceEntity.Request
{
    /// <summary>
    /// 微信委托业务对象
    /// </summary>
    public class EntrustEntity : BaseRequestEntity
    {

        /// <summary>
        ///ID
        /// </summary>
        public string BUSINESSNO { get; set; }

        /// <summary>
        /// A: 按揭，D:抵押 or 空
        /// </summary>
        public string BUSINESSTYPE { get; set; }

        /// <summary>
        /// 客户姓名
        /// </summary>
        public string CUSTOMERNAME { get; set; }

        /// <summary>
        /// 客户编号
        /// </summary>
        public string CUSTOMERIDNUM { get; set; }

        /// <summary>
        /// 产权人姓名
        /// </summary>
        public string PERSONNAME { get; set; }
        /// <summary>
        /// 产权人姓名所在城市
        /// </summary>
        public string PERSONCITY { get; set; }
        /// <summary>
        /// 产权人联系方式
        /// </summary>
        public string CONTRACTPHONE { get; set; }

        /// <summary>
        ///  产权人身份证号
        /// </summary>
        public string IDNUM { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string PROJECTNAME { get; set; }

        /// <summary>
        /// 拟货金额
        /// </summary>
        public string PREPARELOANAMOUNT { get; set; }

        /// <summary>
        /// 首付金额
        /// </summary>
        public string DOWNPAYMENT { get; set; }
        /// <summary>
        /// 行政区码
        /// </summary>
        public string AREACODE { get; set; }

        /// <summary>
        /// 房产地址
        /// </summary>
        public string ADDRESS { get; set; }

        #region 以下按揭必填
        /// <summary>
        /// 按揭类型
        /// </summary>
        public string MORTGAGETYPE { get; set; }

        /// <summary>
        /// 买房人姓名
        /// </summary>
        public string BUYERNAME { get; set; }

        /// <summary>
        /// 买房人户籍
        /// </summary>
        public string BUYERCENSUSREGISTER { get; set; }
        /// <summary>
        /// 买房人联系方式
        /// </summary>
        public string BUYERPHONE { get; set; }

        /// <summary>
        /// 买房人身份证
        /// </summary>
        public string BUYERIDNUM { get; set; }
        #endregion

        #region 非必填
        /// <summary>
        /// 贷款银行
        /// </summary>
        public string LENDINGBANK { get; set; }

        /// <summary>
        /// 其它银行
        /// </summary>
        public string OTHERLENDINGBANK { get; set; }
        /// <summary>
        /// 委托人
        /// </summary>
        public string CLIENTMAN { get; set; }

        /// <summary>
        /// 委托人电话
        /// </summary>
        public string CLIENTPHONE { get; set; }

        /// <summary>
        /// 委托时间
        /// </summary>
        public string ENTRUSTDATE { get; set; }

        /// <summary>
        /// 委托机构编码
        /// </summary>
        public string CLIENT { get; set; }

        /// <summary>
        /// 委托机构名
        /// </summary>
        public string CLIENTNAME { get; set; }

        /// <summary>
        /// 房讯通业务编号
        /// </summary>
        public string FXTNO { get; set; }

        #endregion

        /// <summary>
        /// 开发商
        /// </summary>
        public string DEVELOPERS { get; set; }
        /// <summary>
        /// 贷款人角色
        /// </summary>
        public string LENDERROLE { get; set; }
        /// <summary>
        /// 录入人名称
        /// </summary>
        public string INPUTUSERNAME { get; set; }
        /// <summary>
        /// 产权人户籍
        /// </summary>
        public string OWNERCENSUSREGISTER { get; set; }
    }
}
