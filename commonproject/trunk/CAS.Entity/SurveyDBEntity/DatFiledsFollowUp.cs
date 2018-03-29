using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.SurveyDBEntity
{
    /// <summary>
    /// 查勘字段修改跟进表 
    /// yinpc 
    /// 2014-04-25
    /// </summary>
    [Serializable]
    [TableAttribute("dbo.Dat_FiledsFollowUp")]
    public class DatFiledsFollowUp : BaseTO
    {
        private int _filedsid;
        /// <summary>
        /// 自增ID
        /// </summary>
        [SQLField("filedsid", EnumDBFieldUsage.PrimaryKey, true)]
        public int filedsid
        {
            get { return _filedsid; }
            set { _filedsid = value; }
        }
        private long _sid;
        /// <summary>
        /// 查勘ID
        /// </summary>
        public long sid
        {
            get { return _sid; }
            set { _sid = value; }
        }
        private int _fxtcompanyid;
        /// <summary>
        /// 公司ID
        /// </summary>
        public int fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
        }
        private string _filedscode;
        /// <summary>
        /// 字段编号
        /// </summary>
        public string filedscode
        {
            get { return _filedscode; }
            set { _filedscode = value; }
        }
        private string _oldfiledsname;
        /// <summary>
        /// 字段值
        /// </summary>
        public string oldfiledsname
        {
            get { return _oldfiledsname; }
            set { _oldfiledsname = value; }
        }
        private string _newfiledsname;
        /// <summary>
        /// 字段修改值
        /// </summary>
        public string newfiledsname
        {
            get { return _newfiledsname; }
            set { _newfiledsname = value; }
        }
        private DateTime? _createtime;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? createtime
        {
            get { return _createtime; }
            set { _createtime = value; }
        }
        private int? _valid;
        /// <summary>
        /// 有效
        /// </summary>
        public int? valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
        private string _createusername;
        /// <summary>
        /// 创建人名字
        /// </summary>
        public string createusername
        {
            get { return _createusername; }
            set { _createusername = value; }
        }
        private string _createuserid;
        /// <summary>
        /// 创建人Id
        /// </summary>
        public string createuserid
        {
            get { return _createuserid; }
            set { _createuserid = value; }
        }
        private string _filedsname;
        /// <summary>
        /// 字段名称
        /// </summary>
        public string filedsname
        {
            get { return _filedsname; }
            set { _filedsname = value; }
        }

        
    }

}
