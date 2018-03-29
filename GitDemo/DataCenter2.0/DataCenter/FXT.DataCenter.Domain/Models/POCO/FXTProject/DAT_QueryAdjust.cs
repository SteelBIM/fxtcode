using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class DAT_QueryAdjust
    {
        private int _id;
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int _fk_qid;
        public int fk_qid
        {
            get { return _fk_qid; }
            set { _fk_qid = value; }
        }
        private decimal? _oldprice;
        public decimal? oldprice
        {
            get { return _oldprice; }
            set { _oldprice = value; }
        }
        private decimal? _oldtax;
        public decimal? oldtax
        {
            get { return _oldtax; }
            set { _oldtax = value; }
        }
        private string _olduserid;
        public string olduserid
        {
            get { return _olduserid; }
            set { _olduserid = value; }
        }
        private DateTime? _olddatetime;
        public DateTime? olddatetime
        {
            get { return _olddatetime; }
            set { _olddatetime = value; }
        }
        private decimal _newprice;
        public decimal newprice
        {
            get { return _newprice; }
            set { _newprice = value; }
        }
        private decimal? _newtax;
        public decimal? newtax
        {
            get { return _newtax; }
            set { _newtax = value; }
        }
        private string _newuserid;
        public string newuserid
        {
            get { return _newuserid; }
            set { _newuserid = value; }
        }
        private DateTime _newdatetime = DateTime.Now;
        public DateTime newdatetime
        {
            get { return _newdatetime; }
            set { _newdatetime = value; }
        }
        private string _remark;
        public string remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        private int _cityid;
        public int cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        private int _fxtcompanyid;
        public int fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
        }
        private string _checkuser1;
        public string checkuser1
        {
            get { return _checkuser1; }
            set { _checkuser1 = value; }
        }
        private string _checkremark1;
        public string checkremark1
        {
            get { return _checkremark1; }
            set { _checkremark1 = value; }
        }
        private DateTime? _checkdate1;
        public DateTime? checkdate1
        {
            get { return _checkdate1; }
            set { _checkdate1 = value; }
        }
        private int? _checkcode1;
        public int? checkcode1
        {
            get { return _checkcode1; }
            set { _checkcode1 = value; }
        }
        private string _checkuser2;
        public string checkuser2
        {
            get { return _checkuser2; }
            set { _checkuser2 = value; }
        }
        private string _checkremark2;
        public string checkremark2
        {
            get { return _checkremark2; }
            set { _checkremark2 = value; }
        }
        private DateTime? _checkdate2;
        public DateTime? checkdate2
        {
            get { return _checkdate2; }
            set { _checkdate2 = value; }
        }
        private int? _checkcode2;
        public int? checkcode2
        {
            get { return _checkcode2; }
            set { _checkcode2 = value; }
        }

    }
}
