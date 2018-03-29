using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.SYS_AutoTaxTemplate")]
    public class SYSAutoTaxTemplate : BaseTO
    {
        private int _id;
        /// <summary>
        /// 税费模板ID
        /// </summary>
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _name;
        /// <summary>
        /// 税费模板名称
        /// </summary>
        public string name
        {
            get { return _name; }
            set { _name = value; }
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

        private int _cityid;
        public int cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        
    }
}