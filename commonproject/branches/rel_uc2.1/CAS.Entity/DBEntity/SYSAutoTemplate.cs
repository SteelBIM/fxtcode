using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.SYS_AutoTemplate")]
    public class SYSAutoTemplate : BaseTO
    {
        private int _atid;
        [SQLField("atid", EnumDBFieldUsage.PrimaryKey, true)]
        public int atid
        {
            get { return _atid; }
            set { _atid = value; }
        }
        private string _atname;
        public string atname
        {
            get { return _atname; }
            set { _atname = value; }
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
        private int _code;
        /// <summary>
        /// 跟Files表中的FlieTypeCode一致
        /// </summary>
        public int code
        {
            get { return _code; }
            set { _code = value; }
        }
        private int _templatecode;
        /// <summary>
        /// 跟Files表中的FileSubTypeCode一致
        /// </summary>
        public int templatecode
        {
            get { return _templatecode; }
            set { _templatecode = value; }
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
        private bool _isvalid = true;
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool isvalid
        {
            get { return _isvalid; }
            set { _isvalid = value; }
        }
        private long _fileid = 0;
        /// <summary>
        /// word模板附件ID
        /// </summary>
        public long fileid
        {
            get { return _fileid; }
            set { _fileid = value; }
        }
        /// <summary>
        /// excel模板附件ID
        /// </summary>
        public long excelfileid
        {
            get;
            set;
        }
    }
}