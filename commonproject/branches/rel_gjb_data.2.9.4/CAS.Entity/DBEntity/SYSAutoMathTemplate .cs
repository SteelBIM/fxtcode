using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.SYS_AutoMathTemplate")]
    public class SYSAutoMathTemplate : BaseTO
    {
        private int _mtid;
        /// <summary>
        /// 测算模板ID
        /// </summary>
        [SQLField("mtid", EnumDBFieldUsage.PrimaryKey, true)]
        public int mtid
        {
            get { return _mtid; }
            set { _mtid = value; }
        }
        private string _mtname;
        /// <summary>
        /// 测算模板名称
        /// </summary>
        public string mtname
        {
            get { return _mtname; }
            set { _mtname = value; }
        }
        private int _objecttype;
        /// <summary>
        /// 物业类型1031
        /// </summary>
        public int objecttype
        {
            get { return _objecttype; }
            set { _objecttype = value; }
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
        private long _fileid = 0;
        /// <summary>
        /// 模板附件ID
        /// </summary>
        public long fileid
        {
            get { return _fileid; }
            set { _fileid = value; }
        }
        private long? _wordfileid;
        /// <summary>
        /// WORD模板附件ID
        /// </summary>
        public long? wordfileid
        {
            get { return _wordfileid; }
            set { _wordfileid = value; }
        }
        private int _type = 1;
        /// <summary>
        /// 类型：1为测算表、2为查勘表
        /// </summary>
        public int type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }
    }
}