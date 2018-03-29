using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class SYS_Step_User
    {
        private int _stepuserid;
        /// <summary>
        /// 主键
        /// </summary>
        //[SQLField("stepuserid", EnumDBFieldUsage.PrimaryKey, true)]
        public int stepuserid
        {
            get { return _stepuserid; }
            set { _stepuserid = value; }
        }
        private int _stepid;
        public int stepid
        {
            get { return _stepid; }
            set { _stepid = value; }
        }
        private byte _usertype;
        /// <summary>
        /// 此步骤的审核类型, 个人/用户组
        /// </summary>
        public byte usertype
        {
            get { return _usertype; }
            set { _usertype = value; }
        }
        private int? _groupid;
        /// <summary>
        /// 审核的用户组Id
        /// </summary>
        public int? groupid
        {
            get { return _groupid; }
            set { _groupid = value; }
        }
        private string _userid;
        /// <summary>
        /// 审核的用户Id
        /// </summary>
        public string userid
        {
            get { return _userid; }
            set { _userid = value; }
        }

    }
}
