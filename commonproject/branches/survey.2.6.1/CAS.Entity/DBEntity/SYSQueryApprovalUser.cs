using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.SYS_QueryApprovalUser")]
    ///潘锦发-20150702
    public class SYSQueryApprovalUser : BaseTO
    {
        private int _id;
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int _subcompanyid;
        /// <summary>
        /// 分支机构
        /// </summary>
        public int subcompanyid
        {
            get { return _subcompanyid; }
            set { _subcompanyid = value; }
        }
        private int _entrusttypecode;
        /// <summary>
        /// 类型(对公/个人)
        /// </summary>
        public int entrusttypecode
        {
            get { return _entrusttypecode; }
            set { _entrusttypecode = value; }
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
        private DateTime _createtime;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createtime
        {
            get { return _createtime; }
            set { _createtime = value; }
        }
        private bool? _isApproval;
        /// <summary>
        /// 是否回价审批-- 潘锦发
        /// </summary>
        public bool? isApproval
        {
            get { return _isApproval; }
            set { _isApproval = value; }
        }
        private int _businesstypeid;
        /// <summary>
        /// 技术团队id-- 潘锦发
        /// </summary>
        public int businesstypeid
        {
            get { return _businesstypeid; }
            set { _businesstypeid = value; }
        }
    }
}
