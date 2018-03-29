using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class DAT_HouseCoefficient
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
        private int _typecode = 0;
        public int typecode
        {
            get { return _typecode; }
            set { _typecode = value; }
        }
        private decimal _price = 0M;
        public decimal price
        {
            get { return _price; }
            set { _price = value; }
        }
        private int? _param1;
        public int? param1
        {
            get { return _param1; }
            set { _param1 = value; }
        }
        private int? _param2;
        public int? param2
        {
            get { return _param2; }
            set { _param2 = value; }
        }
        private string _remark;
        public string remark
        {
            get { return _remark; }
            set { _remark = value; }
        }

    }
}
