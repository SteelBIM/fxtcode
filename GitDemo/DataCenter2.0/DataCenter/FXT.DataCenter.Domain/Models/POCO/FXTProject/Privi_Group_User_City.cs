using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class Privi_Group_User_City
    {
        private string _fk_userid;
        //[SQLField("fk_userid", EnumDBFieldUsage.PrimaryKey)]
        public string fk_userid
        {
            get { return _fk_userid; }
            set { _fk_userid = value; }
        }
        private int _fk_groupid;
        public int fk_groupid
        {
            get { return _fk_groupid; }
            set { _fk_groupid = value; }
        }
        private int _fk_cityid;
        //[SQLField("fk_cityid", EnumDBFieldUsage.PrimaryKey)]
        public int fk_cityid
        {
            get { return _fk_cityid; }
            set { _fk_cityid = value; }
        }
        private int _fk_fxt_companyid;
        //[SQLField("fk_fxt_companyid", EnumDBFieldUsage.PrimaryKey)]
        public int fk_fxt_companyid
        {
            get { return _fk_fxt_companyid; }
            set { _fk_fxt_companyid = value; }
        }
        private int _fk_systypecode = 1003001;
       // [SQLField("fk_systypecode", EnumDBFieldUsage.PrimaryKey)]
        public int fk_systypecode
        {
            get { return _fk_systypecode; }
            set { _fk_systypecode = value; }
        }
        private DateTime? _overdate;
        public DateTime? overdate
        {
            get { return _overdate; }
            set { _overdate = value; }
        }
        private int _valid = 1;
        public int valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
        private int _online = 0;
        public int online
        {
            get { return _online; }
            set { _online = value; }
        }
        private DateTime _lastonlinedate = DateTime.Now;
        public DateTime lastonlinedate
        {
            get { return _lastonlinedate; }
            set { _lastonlinedate = value; }
        }

    }
}
