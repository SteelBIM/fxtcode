namespace CAS.Entity.BonaServiceEntity.Request
{
    /// <summary>
    ///  补充微信委托业务
    /// </summary>
    public class ReplenishEntrustEntity : BaseRequestEntity
    {
        /// <summary>
        /// 业务单号
        /// </summary>
        public string SERIALNO { get; set; }

        public string PROPERTYNO { get; set; }

        /// <summary>
        ///  补充人
        /// </summary>
        public string UPDATEUSER { get; set; }

        /// <summary>
        /// 房产编号
        /// </summary>
        public string HOUSENO { get; set; }

        #region 业务单数据

        public string BUSINESSTYPE { get; set; }
        public string CUSTOMERNAME { get; set; }
        public string CUSTOMERIDNUM { get; set; }

        public string BASEINFOSTATUS { get; set; }
        public string RIGHTSTATUS { get; set; }
        public string AUDITSTATUS { get; set; }
        public string CERTPROGRESS { get; set; }
        public string ENTRUSTDATE { get; set; }
        public string CLIENTMAN { get; set; }
        public string CLIENT { get; set; }
        public string CLIENTNAME { get; set; }
        public string CLIENTPHONE { get; set; }

        public string CLIENTCONTACT { get; set; }
        #endregion

        #region  房产数据

        public string CERTIFICATENUM { get; set; }
        public string BUILDINGAREA { get; set; }
        public string UNITPRICE { get; set; }
        public string REGISTEDATE { get; set; }
        public string BUILDINGDATE { get; set; }
        public string USAGE { get; set; }

        /// <summary>
        /// 拟贷金额
        /// </summary>
        public string PREPARELOANAMOUNT { get; set; }

        /// <summary>
        /// 首付金额
        /// </summary>
        public string DOWNPAYMENT { get; set; }
        public string ROOMNUM { get; set; }
        public string SURVEYENDTIME { get; set; }
        public string APPRAISETOTALPRICE { get; set; }
        public string AREACODE { get; set; }
        public string PROJECTNAME { get; set; }
        public string ADDRESS { get; set; }
        public string LENDINGBANK { get; set; }
        public string OTHERLENDINGBANK { get; set; }

        //public string HOUSEAREA { get; set; }
        //public string CLIENTNAME { get; set; }
        //public string CLIENTCONTACT { get; set; }
        //public string CLIENTPHONE { get; set; }

        #endregion

        #region  产权信息

        public string PERSONNAME { get; set; }
        //public string AREA { get; set; }
        public string PERSONCITY { get; set; }
        public string CONTRACTPHONE { get; set; }
        public string IDNUM { get; set; }
        public string OWNERCENSUSREGISTER { get; set; }
        public string MARITALSTATUS { get; set; }
        public string HASCHILDREN { get; set; }

        #endregion

        #region 买房人
        public string BUYERNAME { get; set; }
        public string BUYERPHONE { get; set; }
        public string BUYERIDNUM { get; set; }
        public string BUYERCENSUSREGISTER { get; set; }
        #endregion



        /// <summary>
        /// 开发商
        /// </summary>
        public string DEVELOPERS { get; set; }
        /// <summary>
        /// 贷款人角色
        /// </summary>
        public string LENDERROLE { get; set; }
    }
}
