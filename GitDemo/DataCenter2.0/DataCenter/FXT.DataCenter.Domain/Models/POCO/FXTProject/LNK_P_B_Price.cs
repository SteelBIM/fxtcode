using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class LNK_P_B_Price
    {
        private int _objectid;
        public int objectid
        {
            get { return _objectid; }
            set { _objectid = value; }
        }
        private decimal _price;
        public decimal price
        {
            get { return _price; }
            set { _price = value; }
        }
        private DateTime _adjustdate = DateTime.Now;
        public DateTime adjustdate
        {
            get { return _adjustdate; }
            set { _adjustdate = value; }
        }
        private string _username;
        public string username
        {
            get { return _username; }
            set { _username = value; }
        }
        private int _type = 1;
        /// <summary>
        /// 1楼盘；2楼宇
        /// </summary>
        public int type
        {
            get { return _type; }
            set { _type = value; }
        }
        private int? _cityid;
        public int? cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }

    }
}
