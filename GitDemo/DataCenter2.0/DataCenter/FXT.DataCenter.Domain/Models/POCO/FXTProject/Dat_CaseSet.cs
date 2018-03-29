using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class Dat_CaseSet
    {
        private int _setid;
        public int setid
        {
            get { return _setid; }
            set { _setid = value; }
        }
        private int _casenum;
        public int casenum
        {
            get { return _casenum; }
            set { _casenum = value; }
        }
        private int _buildingareatype;
        public int buildingareatype
        {
            get { return _buildingareatype; }
            set { _buildingareatype = value; }
        }
        private int? _casepriceup;
        public int? casepriceup
        {
            get { return _casepriceup; }
            set { _casepriceup = value; }
        }
        private int? _casepricedown;
        public int? casepricedown
        {
            get { return _casepricedown; }
            set { _casepricedown = value; }
        }
        private int? _cityid;
        public int? cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        private int? _fxtcompanyid;
        public int? fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
        }
        private string _userid;
        public string userid
        {
            get { return _userid; }
            set { _userid = value; }
        }

    }
}
