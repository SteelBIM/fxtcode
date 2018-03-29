using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class DAT_InformationExtra
    {
        private int _id;
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _mobile;
        /// <summary>
        /// 提取号码
        /// </summary>
        public string mobile
        {
            get { return _mobile; }
            set { _mobile = value; }
        }
        private string _extracode;
        /// <summary>
        /// 提取码
        /// </summary>
        public string extracode
        {
            get { return _extracode; }
            set { _extracode = value; }
        }
        private DateTime _extradate;
        /// <summary>
        /// 提取时间
        /// </summary>
        public DateTime extradate
        {
            get { return _extradate; }
            set { _extradate = value; }
        }
        private string _ip;
        /// <summary>
        /// 用户IP
        /// </summary>
        public string ip
        {
            get { return _ip; }
            set { _ip = value; }
        }

    }
}
