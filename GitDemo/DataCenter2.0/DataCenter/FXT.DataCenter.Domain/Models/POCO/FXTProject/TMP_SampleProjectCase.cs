using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class TMP_SampleProjectCase
    {
        private int _id;
        //[SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
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
        private int _projectid;
        public int projectid
        {
            get { return _projectid; }
            set { _projectid = value; }
        }
        private double _unitprice;
        public double unitprice
        {
            get { return _unitprice; }
            set { _unitprice = value; }
        }
        private int _dateweekflag;
        public int dateweekflag
        {
            get { return _dateweekflag; }
            set { _dateweekflag = value; }
        }
        private int _datemonthflag;
        public int datemonthflag
        {
            get { return _datemonthflag; }
            set { _datemonthflag = value; }
        }
        private int _purposecode;
        public int purposecode
        {
            get { return _purposecode; }
            set { _purposecode = value; }
        }
        private int _areaid;
        public int areaid
        {
            get { return _areaid; }
            set { _areaid = value; }
        }
        private int _subareaid;
        public int subareaid
        {
            get { return _subareaid; }
            set { _subareaid = value; }
        }
        private int _buildingyear;
        public int buildingyear
        {
            get { return _buildingyear; }
            set { _buildingyear = value; }
        }
        private int _buildingyearcode;
        public int buildingyearcode
        {
            get { return _buildingyearcode; }
            set { _buildingyearcode = value; }
        }
        private int _buildingtypecode;
        public int buildingtypecode
        {
            get { return _buildingtypecode; }
            set { _buildingtypecode = value; }
        }
        private int _buildingarea;
        public int buildingarea
        {
            get { return _buildingarea; }
            set { _buildingarea = value; }
        }
        private int _buildingareacode;
        public int buildingareacode
        {
            get { return _buildingareacode; }
            set { _buildingareacode = value; }
        }
        private int _housetypecode;
        public int housetypecode
        {
            get { return _housetypecode; }
            set { _housetypecode = value; }
        }
        private int _casenumber = 100;
        public int casenumber
        {
            get { return _casenumber; }
            set { _casenumber = value; }
        }

    }
}
