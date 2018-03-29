using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }

}
