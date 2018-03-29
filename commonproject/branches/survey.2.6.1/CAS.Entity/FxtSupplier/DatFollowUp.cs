using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.FxtSupplier
{
    [Serializable]
    [TableAttribute("dbo.Dat_FollowUp")]
    public class DatFollowUp : BaseTO
    {
        private long _followupid;
        [SQLField("followupid", EnumDBFieldUsage.PrimaryKey, true)]
        public long followupid
        {
            get { return _followupid; }
            set { _followupid = value; }
        }
        private long _objectid;
        /// <summary>
        /// 录入跟进的对象ID 
        /// </summary>
        public long objectid
        {
            get { return _objectid; }
            set { _objectid = value; }
        }
        private long _followtype;
        /// <summary>
        /// 跟进的类型: 1-业务跟进
        /// </summary>
        public long followtype
        {
            get { return _followtype; }
            set { _followtype = value; }
        }
        private string _content;
        /// <summary>
        /// 内容
        /// </summary>
        public string content
        {
            get { return _content; }
            set { _content = value; }
        }
        private DateTime _createdate= DateTime.Now;
        public DateTime createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
        private string _creator;
        /// <summary>
        /// 操作人姓名
        /// </summary>
        public string creator
        {
            get { return _creator; }
            set { _creator = value; }
        }
        private string _createusername;
        /// <summary>
        /// 操作者账号
        /// </summary>
        public string createusername
        {
            get { return _createusername; }
            set { _createusername = value; }
        }
        private int? _createuserid;
        /// <summary>
        /// 操作人：0--房讯通管理员
        /// </summary>
        public int? createuserid
        {
            get { return _createuserid; }
            set { _createuserid = value; }
        }
        private bool _isread = true;
        /// <summary>
        /// 是否阅读
        /// </summary>
        public bool isread
        {
            get { return _isread; }
            set { _isread = value; }
        }
        private int? _followupdetailstype=0;
        /// <summary>
        /// 跟进细类:1--催办跟进;0--普通跟进
        /// </summary>
        public int? followupdetailstype
        {
            get { return _followupdetailstype; }
            set { _followupdetailstype = value; }
        }

    }
}
