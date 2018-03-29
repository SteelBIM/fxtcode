using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.SYS_AutoReportTemplate")]
    public class SYSAutoReportTemplate : BaseTO
    {
        private int _rtid;
        /// <summary>
        /// 报告模板ID
        /// </summary>
        [SQLField("rtid", EnumDBFieldUsage.PrimaryKey, true)]
        public int rtid
        {
            get { return _rtid; }
            set { _rtid = value; }
        }
        private string _rtname;
        /// <summary>
        /// 报告模板名称
        /// </summary>
        public string rtname
        {
            get { return _rtname; }
            set { _rtname = value; }
        }
        private int _businesstype;
        /// <summary>
        /// 业务大类（房地产/土地/资产）10010
        /// </summary>
        public int businesstype
        {
            get { return _businesstype; }
            set { _businesstype = value; }
        }
        private int _reporttype;
        /// <summary>
        /// 报告类型（预评/正式报告）2018
        /// </summary>
        public int reporttype
        {
            get { return _reporttype; }
            set { _reporttype = value; }
        }
        private string _remark;
        /// <summary>
        /// 模板说明
        /// </summary>
        public string remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        private int _createuserid;
        /// <summary>
        /// 创建人
        /// </summary>
        public int createuserid
        {
            get { return _createuserid; }
            set { _createuserid = value; }
        }
        private DateTime _createdate = DateTime.Now;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
        private DateTime _lastmodiftime;
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime lastmodiftime
        {
            get { return _lastmodiftime; }
            set { _lastmodiftime = value; }
        }
        private bool _isvalid = true;
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool isvalid
        {
            get { return _isvalid; }
            set { _isvalid = value; }
        }
        private bool _issingle = true;
        /// <summary>
        /// 是否单套模板
        /// </summary>
        public bool issingle
        {
            get { return _issingle; }
            set { _issingle = value; }
        }
        private bool _isattachment = false;
        /// <summary>
        /// 是否包含附件
        /// </summary>
        public bool isattachment
        {
            get { return _isattachment; }
            set { _isattachment = value; }
        }
        private long _wordfileid = 0;
        /// <summary>
        /// word模板文件
        /// </summary>
        public long wordfileid
        {
            get { return _wordfileid; }
            set { _wordfileid = value; }
        }
        private long _excelfileid = 0;
        /// <summary>
        /// excel模板文件
        /// </summary>
        public long excelfileid
        {
            get { return _excelfileid; }
            set { _excelfileid = value; }
        }
        private int _codeid;
        /// <summary>
        /// 子类型codeid
        /// </summary>
        public int codeid
        {
            get { return _codeid; }
            set { _codeid = value; }
        }
    }
}