using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [Table("dbo.SYS_PrintSealUserSetting")]
    public class SYSPrintSealUserSetting : BaseTO
    {
        private int _id;
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int? _fxtcompanyid;
        /// <summary>
        /// 公司id
        /// </summary>
        public int? fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
        }
        private string _branchid;
        /// <summary>
        /// 分支机构ID
        /// </summary>
        public string branchid
        {
            get { return _branchid; }
            set { _branchid = value; }
        }
        private string _branchname;
        /// <summary>
        /// 分支机构名称
        /// </summary>
        public string branchname
        {
            get { return _branchname; }
            set { _branchname = value; }
        }
        private string _userid;
        /// <summary>
        /// 用户ID  -1代表报告撰写人  0代表业务员
        /// </summary>
        public string userid
        {
            get { return _userid; }
            set { _userid = value; }
        }
        private string _username;
        /// <summary>
        /// 用户的名称
        /// </summary>
        public string username
        {
            get { return _username; }
            set { _username = value; }
        }
        private int? _createuserid;
        /// <summary>
        /// 创建用户
        /// </summary>
        public int? createuserid
        {
            get { return _createuserid; }
            set { _createuserid = value; }
        }
        private DateTime? _createdate;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
        private bool? _isprint;
        /// <summary>
        /// 打印还是盖章 1是打印 0是盖章
        /// </summary>
        public bool? isprint
        {
            get { return _isprint; }
            set { _isprint = value; }
        }
        private bool _valid = true;
        public bool valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
    }

}
