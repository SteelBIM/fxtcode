using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.SurveyDBEntity
{
    /// <summary>
    /// 查勘库模版可用字段表 caoq 2013-11-11
    /// </summary>
    [Serializable]
    [TableAttribute("dbo.SYS_BusinessTableSetup")]
    public class SYSBusinessTableSetup : BaseTO
    {
        private int _btsid;
        /// <summary>
        /// 自增ID
        /// </summary>
        [SQLField("btsid", EnumDBFieldUsage.PrimaryKey, true)]
        public int btsid
        {
            get { return _btsid; }
            set { _btsid = value; }
        }       
        private int _fxtcompanyid;
        /// <summary>
        /// 机构ID
        /// </summary>
        public int fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
        }
        private int _subcompanyid = 0;
        /// <summary>
        /// 分支机构ID
        /// </summary>
        public int subcompanyid
        {
            get { return _subcompanyid; }
            set { _subcompanyid = value; }
        }
        private int _typecode = 2018004;
        /// <summary>
        /// 业务类型，询价、查勘、初评、报告
        /// </summary>
        public int typecode
        {
            get { return _typecode; }
            set { _typecode = value; }
        }
        private int _subtypecode = 1031001;
        /// <summary>
        /// 业务子类型（委估对象类型、报告类型）
        /// </summary>
        public int subtypecode
        {
            get { return _subtypecode; }
            set { _subtypecode = value; }
        }
        private string _fieldscontent;
        /// <summary>
        /// 字段配置，网页HTML
        /// </summary>
        public string fieldscontent
        {
            get { return _fieldscontent; }
            set { _fieldscontent = value; }
        }
        private string _excelpath;
        /// <summary>
        /// 查勘EXCEL模板地址
        /// </summary>
        public string excelpath
        {
            get { return _excelpath; }
            set { _excelpath = value; }
        }
        private DateTime _createtime = DateTime.Now;
        public DateTime createtime
        {
            get { return _createtime; }
            set { _createtime = value; }
        }
        private string _createuserid;
        public string createuserid
        {
            get { return _createuserid; }
            set { _createuserid = value; }
        }

        public int fieldgrouptype { get; set; }
        /// <summary>
        /// 模板名称
        /// </summary>
        public string btsname { get; set; }

        /// <summary>
        /// 是否默认模板
        /// </summary>
        private int? _istabledefault;
        public int? istabledefault
        {
            get { return _istabledefault; }
            set { _istabledefault = value; }
        }
        /// <summary>
        /// 需要自动绑定字段数据
        /// </summary>
        private string _fielddataautobind;
        public string fielddataautobind
        {
            get { return _fielddataautobind; }
            set { _fielddataautobind = value; }
        }
    }
}