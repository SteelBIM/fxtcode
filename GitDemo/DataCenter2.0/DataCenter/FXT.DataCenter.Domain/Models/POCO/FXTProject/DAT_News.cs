using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class DAT_News
    {
        private int _id;
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _title;
        public string title
        {
            get { return _title; }
            set { _title = value; }
        }
        private int? _newstype;
        public int? newstype
        {
            get { return _newstype; }
            set { _newstype = value; }
        }
        private string _cityname;
        public string cityname
        {
            get { return _cityname; }
            set { _cityname = value; }
        }
        private string _source;
        public string source
        {
            get { return _source; }
            set { _source = value; }
        }
        private string _webaddress;
        public string webaddress
        {
            get { return _webaddress; }
            set { _webaddress = value; }
        }
        private DateTime? _sourcetime;
        public DateTime? sourcetime
        {
            get { return _sourcetime; }
            set { _sourcetime = value; }
        }
        private string _keyword;
        public string keyword
        {
            get { return _keyword; }
            set { _keyword = value; }
        }
        private string _detail;
        public string detail
        {
            get { return _detail; }
            set { _detail = value; }
        }
        private string _writer;
        public string writer
        {
            get { return _writer; }
            set { _writer = value; }
        }
        private string _userdept;
        public string userdept
        {
            get { return _userdept; }
            set { _userdept = value; }
        }
        private DateTime? _casedate;
        public DateTime? casedate
        {
            get { return _casedate; }
            set { _casedate = value; }
        }
        private bool? _ischoose;
        public bool? ischoose
        {
            get { return _ischoose; }
            set { _ischoose = value; }
        }

    }
}
