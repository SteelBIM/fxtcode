using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity
{
    [Serializable]
    [TableAttribute("dbo.SYS_ReportNoBuildRule")]
    public class SYSReportNoBuildRule : BaseTO
    {
        private int _ruleid;
        [SQLField("ruleid", EnumDBFieldUsage.PrimaryKey, true)]
        public int ruleid
        {
            get { return _ruleid; }
            set { _ruleid = value; }
        }
        private int _companyid;
        /// <summary>
        /// 公司id
        /// </summary>
        public int companyid
        {
            get { return _companyid; }
            set { _companyid = value; }
        }
        private string _baseid;
        /// <summary>
        /// 管理编号基础数据表  多个以，号分隔
        /// </summary>
        public string baseid
        {
            get { return _baseid; }
            set { _baseid = value; }
        }
        private string _rulename;
        /// <summary>
        /// 规则名称
        /// </summary>
        public string rulename
        {
            get { return _rulename; }
            set { _rulename = value; }
        }
        private bool _ismoreoption;
        /// <summary>
        /// 是否多条件 多选项
        /// </summary>
        public bool ismoreoption
        {
            get { return _ismoreoption; }
            set { _ismoreoption = value; }
        }
        private int? _datetype;
        /// <summary>
        /// 数据类型0基础数据,固定格式 1取关联表简称  2日期类型 3随即自增号000001
        /// </summary>
        public int? datetype
        {
            get { return _datetype; }
            set { _datetype = value; }
        }
        private string _defaultvalue;
        /// <summary>
        /// 默认值
        /// </summary>
        public string defaultvalue
        {
            get { return _defaultvalue; }
            set { _defaultvalue = value; }
        }
        private int? _orderid;
        /// <summary>
        /// 排序
        /// </summary>
        public int? orderid
        {
            get { return _orderid; }
            set { _orderid = value; }
        }
        private int? _biztype;
        /// <summary>
        /// 报告类型 预评或者报告 2018005,2018006  为0表示通用
        /// </summary>
        public int? biztype
        {
            get { return _biztype; }
            set { _biztype = value; }
        }
        private int? _valid;
        /// <summary>
        /// 是否有效
        /// </summary>
        public int? valid
        {
            get { return _valid; }
            set { _valid = value; }
        }

        private string _formatdata;
        /// <summary>
        /// 格式数据 
        /// </summary>
        public string formatdata
        {
            get { return _formatdata; }
            set { _formatdata = value; }
        }

    }
}