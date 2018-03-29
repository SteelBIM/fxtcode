using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class Privi_UnLogin
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
        private int _systypecode;
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
        private string _pascdoe;
        public string pascdoe
        {
            get { return _pascdoe; }
            set { _pascdoe = value; }
        }

    }
}
