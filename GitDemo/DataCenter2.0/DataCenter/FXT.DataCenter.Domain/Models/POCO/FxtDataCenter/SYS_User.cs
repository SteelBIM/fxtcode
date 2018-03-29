using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
   public class SYS_User
    {
        private string _username;
        /// <summary>
        /// 用户名（带后缀）
        /// </summary>
        public string username
        {
            get { return _username; }
            set { _username = value; }
        }
        private int _companyid;
        /// <summary>
        /// 机构ID
        /// </summary>
        public int companyid
        {
            get { return _companyid; }
            set { _companyid = value; }
        }
        private DateTime _createdate = DateTime.Now;
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
        private int _valid = 1;
        /// <summary>
        /// 有效
        /// </summary>
        public int valid
        {
            get { return _valid; }
            set { _valid = value; }
        }

        /// <summary>
        /// 密码
        /// </summary>
        public string userpwd { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string emailstr { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public string mobile { get; set; }

        /// <summary>
        /// 微信号
        /// </summary>
        public string wxopenid { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public string updateDate { get; set; }

        private string _truename;
        /// <summary>
        /// 真实名称
        /// </summary>
        public string truename
        {
            get { return _truename; }
            set { _truename = value; }
        }
    }
}
