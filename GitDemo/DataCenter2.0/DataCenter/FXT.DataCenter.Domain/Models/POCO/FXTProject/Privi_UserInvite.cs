using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class Privi_UserInvite
    {
        private int _id;
        //[SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _fromuserid;
        /// <summary>
        /// 邀请码生成者
        /// </summary>
        public string fromuserid
        {
            get { return _fromuserid; }
            set { _fromuserid = value; }
        }
        private string _invitecode;
        /// <summary>
        /// 邀请码
        /// </summary>
        public string invitecode
        {
            get { return _invitecode; }
            set { _invitecode = value; }
        }
        private string _touserid;
        /// <summary>
        /// 邀请码接收使用者
        /// </summary>
        public string touserid
        {
            get { return _touserid; }
            set { _touserid = value; }
        }
        private DateTime _createdate = DateTime.Now;
        /// <summary>
        /// 邀请码创建时间
        /// </summary>
        public DateTime createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
        private DateTime? _usedate;
        /// <summary>
        /// 邀请码使用时间
        /// </summary>
        public DateTime? usedate
        {
            get { return _usedate; }
            set { _usedate = value; }
        }
        private int _valid = 1;
        /// <summary>
        /// 是否有效
        /// </summary>
        public int valid
        {
            get { return _valid; }
            set { _valid = value; }
        }

    }
}
