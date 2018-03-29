using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class Dat_SysMessage_Read
    {
        private int _id;
        //[SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int _mid;
        /// <summary>
        /// 系统消息ID
        /// </summary>
        public int mid
        {
            get { return _mid; }
            set { _mid = value; }
        }
        private string _userid;
        /// <summary>
        /// 读取用户
        /// </summary>
        public string userid
        {
            get { return _userid; }
            set { _userid = value; }
        }
        private DateTime _readdate = DateTime.Now;
        /// <summary>
        /// 读取时间
        /// </summary>
        public DateTime readdate
        {
            get { return _readdate; }
            set { _readdate = value; }
        }

    }
}
