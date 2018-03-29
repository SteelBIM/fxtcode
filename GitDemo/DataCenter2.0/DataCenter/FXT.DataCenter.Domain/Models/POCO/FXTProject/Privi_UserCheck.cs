using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class Privi_UserCheck
    {
        private int _id;
        /// <summary>
        /// 评估机构注册申请审核、用户注册申请审核表
        /// </summary>
        //[SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _userid;
        /// <summary>
        /// 申请用户ID
        /// </summary>
        public string userid
        {
            get { return _userid; }
            set { _userid = value; }
        }
        private int _fxtcompanyid;
        /// <summary>
        /// 申请评估机构ID
        /// </summary>
        public int fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
        }
        private int? _cityid;
        /// <summary>
        /// 城市ID
        /// </summary>
        public int? cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        private int _iscompany = 0;
        /// <summary>
        /// 是否评估机构申请审核:1是，0:否
        /// </summary>
        public int iscompany
        {
            get { return _iscompany; }
            set { _iscompany = value; }
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
        private int _checktype = 9011001;
        /// <summary>
        /// 审核结果类型.
        /// </summary>
        public int checktype
        {
            get { return _checktype; }
            set { _checktype = value; }
        }
        private DateTime? _checkdate;
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? checkdate
        {
            get { return _checkdate; }
            set { _checkdate = value; }
        }
        private string _checkremark;
        /// <summary>
        /// 审核内容
        /// </summary>
        public string checkremark
        {
            get { return _checkremark; }
            set { _checkremark = value; }
        }
        private string _checkuserid;
        /// <summary>
        /// 审核人
        /// </summary>
        public string checkuserid
        {
            get { return _checkuserid; }
            set { _checkuserid = value; }
        }

    }
}
