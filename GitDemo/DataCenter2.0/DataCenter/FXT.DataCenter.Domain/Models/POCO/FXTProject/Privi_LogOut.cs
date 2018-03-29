using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class Privi_LogOut
    {
        private int _id;
        //[SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int _loginid;
        public int loginid
        {
            get { return _loginid; }
            set { _loginid = value; }
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
        private DateTime _logoutdate = DateTime.Now;
        public DateTime logoutdate
        {
            get { return _logoutdate; }
            set { _logoutdate = value; }
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
        public int systypecode
        {
            get { return _systypecode; }
            set { _systypecode = value; }
        }
        private int _cityid;
        public int cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }

    }
}
