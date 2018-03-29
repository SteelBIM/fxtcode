using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class TMP_PriceConfiguration
    {
        private long _id;
        //[SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public long id
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
        private int _usetypecode;
        public int usetypecode
        {
            get { return _usetypecode; }
            set { _usetypecode = value; }
        }
        private int _buildingtypecode;
        public int buildingtypecode
        {
            get { return _buildingtypecode; }
            set { _buildingtypecode = value; }
        }
        private int _buildingdatecode;
        public int buildingdatecode
        {
            get { return _buildingdatecode; }
            set { _buildingdatecode = value; }
        }
        private int _yearfrom;
        public int yearfrom
        {
            get { return _yearfrom; }
            set { _yearfrom = value; }
        }
        private int _yearto;
        public int yearto
        {
            get { return _yearto; }
            set { _yearto = value; }
        }
        private int _areacode;
        public int areacode
        {
            get { return _areacode; }
            set { _areacode = value; }
        }
        private int _areafrom;
        public int areafrom
        {
            get { return _areafrom; }
            set { _areafrom = value; }
        }
        private int _areato;
        public int areato
        {
            get { return _areato; }
            set { _areato = value; }
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
        private int _housetypecode;
        public int housetypecode
        {
            get { return _housetypecode; }
            set { _housetypecode = value; }
        }

    }
}
