using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    class Privi_Login
    {
        private int _id;
        //[SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _userid;
        public string userid
        {
            get { return _userid; }
            set { _userid = value; }
        }
        private int _fxtcompanyid;
        public int fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
        }
        private DateTime _logindate = DateTime.Now;
        public DateTime logindate
        {
            get { return _logindate; }
            set { _logindate = value; }
        }
        private string _ipaddress;
        public string ipaddress
        {
            get { return _ipaddress; }
            set { _ipaddress = value; }
        }
        private string _pascdoe;
        public string pascdoe
        {
            get { return _pascdoe; }
            set { _pascdoe = value; }
        }
        private int _systypecode = 1003001;
        /// <summary>
        /// 系统类型
        /// </summary>
        public int systypecode
        {
            get { return _systypecode; }
            set { _systypecode = value; }
        }
        private int _cityid;
        /// <summary>
        /// 城市ID
        /// </summary>
        public int cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        private string _browsertype;
        /// <summary>
        /// 浏览器类型
        /// </summary>
        public string browsertype
        {
            get { return _browsertype; }
            set { _browsertype = value; }
        }

    }
}
