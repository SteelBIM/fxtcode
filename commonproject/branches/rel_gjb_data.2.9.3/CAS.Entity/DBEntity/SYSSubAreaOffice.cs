using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.SYS_SubArea_Office")]
    public class SYSSubAreaOffice : BaseTO
    {
        private int _subareaid;
        [SQLField("subareaid", EnumDBFieldUsage.PrimaryKey, true)]
        public int subareaid
        {
            get { return _subareaid; }
            set { _subareaid = value; }
        }
        private string _subareaname;

        public string subareaname
        {
            get { return _subareaname; }
            set { _subareaname = value; }
        }
        private int _areaid;
        public int areaid
        {
            get { return _areaid; }
            set { _areaid = value; }
        }
        private decimal? _x;

        public decimal? x
        {
            get { return _x; }
            set { _x = value; }
        }
        private decimal? _y;

        public decimal? y
        {
            get { return _y; }
            set { _y = value; }
        }
        private int? _orderid;
        /// <summary>
        /// 排序
        /// </summary>
        public int? orderid
        {
            get { return _orderid; }
            set { _orderid = value; }
        }

        private int _fxtcompanyid;
        public int fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
        }
    }
}
