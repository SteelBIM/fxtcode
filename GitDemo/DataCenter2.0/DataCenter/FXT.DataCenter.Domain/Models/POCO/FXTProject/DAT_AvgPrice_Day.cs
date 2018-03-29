using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class DAT_AvgPrice_Day
    {
        private int _id;
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
        private int _areaid = 0;
        public int areaid
        {
            get { return _areaid; }
            set { _areaid = value; }
        }
        private int _subareaid = 0;
        public int subareaid
        {
            get { return _subareaid; }
            set { _subareaid = value; }
        }
        private int _projectid = 0;
        public int projectid
        {
            get { return _projectid; }
            set { _projectid = value; }
        }
        private DateTime _avgpricedate;
        public DateTime avgpricedate
        {
            get { return _avgpricedate; }
            set { _avgpricedate = value; }
        }
        private int _avgprice;
        public int avgprice
        {
            get { return _avgprice; }
            set { _avgprice = value; }
        }
        private int? _buildingareatype;
        public int? buildingareatype
        {
            get { return _buildingareatype; }
            set { _buildingareatype = value; }
        }

    }
}
