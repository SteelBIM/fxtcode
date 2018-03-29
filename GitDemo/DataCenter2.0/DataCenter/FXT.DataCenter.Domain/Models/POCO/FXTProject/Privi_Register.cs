using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class Privi_Register
    {
        private int _id;
        //[SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _linkman;
        public string linkman
        {
            get { return _linkman; }
            set { _linkman = value; }
        }
        private string _mobilephone;
        public string mobilephone
        {
            get { return _mobilephone; }
            set { _mobilephone = value; }
        }
        private string _officephone;
        public string officephone
        {
            get { return _officephone; }
            set { _officephone = value; }
        }
        private string _address;
        public string address
        {
            get { return _address; }
            set { _address = value; }
        }
        private string _email;
        public string email
        {
            get { return _email; }
            set { _email = value; }
        }
        private string _fax;
        public string fax
        {
            get { return _fax; }
            set { _fax = value; }
        }
        private DateTime _createdate = DateTime.Now;
        public DateTime createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
        private string _organcode;
        public string organcode
        {
            get { return _organcode; }
            set { _organcode = value; }
        }
        private string _companyname;
        public string companyname
        {
            get { return _companyname; }
            set { _companyname = value; }
        }

    }
}
