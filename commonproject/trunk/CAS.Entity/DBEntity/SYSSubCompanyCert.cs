using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.SYS_SubCompanyCert")]
    public class SYSSubCompanyCert : BaseTO
    {
        private int _subcompanyid;
        /// <summary>
        /// 分支机构ID
        /// </summary>
        [SQLField("subcompanyid", EnumDBFieldUsage.PrimaryKey)]
        public int subcompanyid
        {
            get { return _subcompanyid; }
            set { _subcompanyid = value; }
        }
        private string _companyfullname;
        /// <summary>
        /// 分支机构全称
        /// </summary>
        public string companyfullname
        {
            get { return _companyfullname; }
            set { _companyfullname = value; }
        }
        private string _companyregno;
        /// <summary>
        /// 工商注册号
        /// </summary>
        public string companyregno
        {
            get { return _companyregno; }
            set { _companyregno = value; }
        }
        private string _companyorgno;
        /// <summary>
        /// 组织机构代码
        /// </summary>
        public string companyorgno
        {
            get { return _companyorgno; }
            set { _companyorgno = value; }
        }
        private string _companyregfilepath;
        /// <summary>
        /// 营业执照附件
        /// </summary>
        public string companyregfilepath
        {
            get { return _companyregfilepath; }
            set { _companyregfilepath = value; }
        }
        private int? _legaluserid;
        /// <summary>
        /// 法人代表
        /// </summary>
        public int? legaluserid
        {
            get { return _legaluserid; }
            set { _legaluserid = value; }
        }
        private string _fdccertlevel;
        /// <summary>
        /// 房地产资质等级
        /// </summary>
        public string fdccertlevel
        {
            get { return _fdccertlevel; }
            set { _fdccertlevel = value; }
        }
        private string _fdccertno;
        /// <summary>
        /// 房地产资质编号
        /// </summary>
        public string fdccertno
        {
            get { return _fdccertno; }
            set { _fdccertno = value; }
        }
        private DateTime? _fdccertregdate;
        /// <summary>
        /// 房地产资质生效日期
        /// </summary>
        public DateTime? fdccertregdate
        {
            get { return _fdccertregdate; }
            set { _fdccertregdate = value; }
        }
        private DateTime? _fdccertvaliddate;
        /// <summary>
        /// 房地产资质有效日期
        /// </summary>
        public DateTime? fdccertvaliddate
        {
            get { return _fdccertvaliddate; }
            set { _fdccertvaliddate = value; }
        }
        private string _fdccertfilepath;
        /// <summary>
        /// 房地产资质附件
        /// </summary>
        public string fdccertfilepath
        {
            get { return _fdccertfilepath; }
            set { _fdccertfilepath = value; }
        }
        private string _tdcertlevel;
        /// <summary>
        /// 土地资质等级
        /// </summary>
        public string tdcertlevel
        {
            get { return _tdcertlevel; }
            set { _tdcertlevel = value; }
        }
        private string _tdcertno;
        /// <summary>
        /// 土地资质注册号
        /// </summary>
        public string tdcertno
        {
            get { return _tdcertno; }
            set { _tdcertno = value; }
        }
        private DateTime? _tdcertregdate;
        /// <summary>
        /// 土地资质生效日期
        /// </summary>
        public DateTime? tdcertregdate
        {
            get { return _tdcertregdate; }
            set { _tdcertregdate = value; }
        }
        private DateTime? _tdcertvaliddate;
        /// <summary>
        /// 土地资质有效日期
        /// </summary>
        public DateTime? tdcertvaliddate
        {
            get { return _tdcertvaliddate; }
            set { _tdcertvaliddate = value; }
        }
        private string _tdcertfilepath;
        /// <summary>
        /// 土地资质附件
        /// </summary>
        public string tdcertfilepath
        {
            get { return _tdcertfilepath; }
            set { _tdcertfilepath = value; }
        }
        private string _zccertlevel;
        /// <summary>
        /// 资产资质等级
        /// </summary>
        public string zccertlevel
        {
            get { return _zccertlevel; }
            set { _zccertlevel = value; }
        }
        private string _zccertno;
        /// <summary>
        /// 资产资质编号
        /// </summary>
        public string zccertno
        {
            get { return _zccertno; }
            set { _zccertno = value; }
        }
        private DateTime? _zccertregdate;
        /// <summary>
        /// 资产资质生效日期
        /// </summary>
        public DateTime? zccertregdate
        {
            get { return _zccertregdate; }
            set { _zccertregdate = value; }
        }
        private DateTime? _zccertvaliddate;
        /// <summary>
        /// 资产资质有效日期
        /// </summary>
        public DateTime? zccertvaliddate
        {
            get { return _zccertvaliddate; }
            set { _zccertvaliddate = value; }
        }
        private string _zccertfilepath;
        /// <summary>
        /// 资产资质附件
        /// </summary>
        public string zccertfilepath
        {
            get { return _zccertfilepath; }
            set { _zccertfilepath = value; }
        }
        private int _subcompanytype = 1;
        /// <summary>
        /// 机构类型，1:分支机构,2:办事处
        /// </summary>
        public int subcompanytype
        {
            get { return _subcompanytype; }
            set { _subcompanytype = value; }
        }
        private int? _managerstatus = 1;
        /// <summary>
        /// 管理状态,1:正常,2:筹备中,3:已注销
        /// </summary>
        public int? managerstatus
        {
            get { return _managerstatus; }
            set { _managerstatus = value; }
        }
        private DateTime? _contractsigndate;
        /// <summary>
        /// 合同签订时间
        /// </summary>
        public DateTime? contractsigndate
        {
            get { return _contractsigndate; }
            set { _contractsigndate = value; }
        }
        private DateTime? _contractstartdate;
        /// <summary>
        /// 合同有效起始时间
        /// </summary>
        public DateTime? contractstartdate
        {
            get { return _contractstartdate; }
            set { _contractstartdate = value; }
        }
        private DateTime? _contractenddate;
        /// <summary>
        /// 合同有效结束时间
        /// </summary>
        public DateTime? contractenddate
        {
            get { return _contractenddate; }
            set { _contractenddate = value; }
        }
        private decimal? _cautionmoney;
        /// <summary>
        /// 保证金
        /// </summary>
        public decimal? cautionmoney
        {
            get { return _cautionmoney; }
            set { _cautionmoney = value; }
        }
        private DateTime? _cautionmoneyagreepaydate;
        /// <summary>
        /// 保证金约定缴纳时间
        /// </summary>
        public DateTime? cautionmoneyagreepaydate
        {
            get { return _cautionmoneyagreepaydate; }
            set { _cautionmoneyagreepaydate = value; }
        }
        private string _cautionmoneypayremarks;
        /// <summary>
        /// 保证金实际缴纳情况
        /// </summary>
        public string cautionmoneypayremarks
        {
            get { return _cautionmoneypayremarks; }
            set { _cautionmoneypayremarks = value; }
        }
        private decimal? _managermoney;
        /// <summary>
        /// 管理费
        /// </summary>
        public decimal? managermoney
        {
            get { return _managermoney; }
            set { _managermoney = value; }
        }
        private DateTime? _managermoneyagreepaydate;
        /// <summary>
        /// 管理费约定缴纳时间
        /// </summary>
        public DateTime? managermoneyagreepaydate
        {
            get { return _managermoneyagreepaydate; }
            set { _managermoneyagreepaydate = value; }
        }
        private string _managermoneypayremarks;
        /// <summary>
        /// 管理费实际缴纳情况
        /// </summary>
        public string managermoneypayremarks
        {
            get { return _managermoneypayremarks; }
            set { _managermoneypayremarks = value; }
        }
        private decimal? _socialsecuritymoney;
        /// <summary>
        /// 社保费用代缴
        /// </summary>
        public decimal? socialsecuritymoney
        {
            get { return _socialsecuritymoney; }
            set { _socialsecuritymoney = value; }
        }
        private decimal? _appraiserprice;
        /// <summary>
        /// 估价师费用
        /// </summary>
        public decimal? appraiserprice
        {
            get { return _appraiserprice; }
            set { _appraiserprice = value; }
        }
        private decimal? _realtyassociatorprice;
        /// <summary>
        /// 房产会员费用
        /// </summary>
        public decimal? realtyassociatorprice
        {
            get { return _realtyassociatorprice; }
            set { _realtyassociatorprice = value; }
        }
        private decimal? _landassociatorprice;
        /// <summary>
        /// 土地会员费用
        /// </summary>
        public decimal? landassociatorprice
        {
            get { return _landassociatorprice; }
            set { _landassociatorprice = value; }
        }
        private string _reportappraiseremarks;
        /// <summary>
        /// 报告质量和评估价格情况
        /// </summary>
        public string reportappraiseremarks
        {
            get { return _reportappraiseremarks; }
            set { _reportappraiseremarks = value; }
        }

        private string _businessscopecodes;
        /// <summary>
        /// 业务范围，多个code用逗号分隔
        /// </summary>
        public string businessscopecodes
        {
            get { return _businessscopecodes; }
            set { _businessscopecodes = value; }
        }
        private int? _contractscopeprovinceid;
        /// <summary>
        /// 承包范围省份
        /// </summary>
        public int? contractscopeprovinceid
        {
            get { return _contractscopeprovinceid; }
            set { _contractscopeprovinceid = value; }
        }
        private int? _contractscopecityid;
        /// <summary>
        /// 承包范围城市
        /// </summary>
        public int? contractscopecityid
        {
            get { return _contractscopecityid; }
            set { _contractscopecityid = value; }
        }
        private int? _contractscopeareaid;
        /// <summary>
        /// 承包范围县区
        /// </summary>
        public int? contractscopeareaid
        {
            get { return _contractscopeareaid; }
            set { _contractscopeareaid = value; }
        }
        private string _appraisersituation;
        /// <summary>
        /// 估价师配备情况，多个code用逗号分隔
        /// </summary>
        public string appraisersituation
        {
            get { return _appraisersituation; }
            set { _appraisersituation = value; }
        }
        private string _provideoneselfappraiseruserids;
        /// <summary>
        /// 自备估价师，多个userId用逗号分隔
        /// </summary>
        public string provideoneselfappraiseruserids
        {
            get { return _provideoneselfappraiseruserids; }
            set { _provideoneselfappraiseruserids = value; }
        }
        private string _headquartersappraiseruserids;
        /// <summary>
        /// 总公司分配估价师，多个userId用逗号分隔
        /// </summary>
        public string headquartersappraiseruserids
        {
            get { return _headquartersappraiseruserids; }
            set { _headquartersappraiseruserids = value; }
        }

    }

}
