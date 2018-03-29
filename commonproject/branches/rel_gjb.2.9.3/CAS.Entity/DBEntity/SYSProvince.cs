using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.SYS_Province")]
    public class SYSProvince : BaseTO
    {
        private int _provinceid;
        [SQLField("provinceid", EnumDBFieldUsage.PrimaryKey, true)]
        public int provinceid
        {
            get { return _provinceid; }
            set { _provinceid = value; }
        }
        private string _provincename;
        /// <summary>
        /// 省名称
        /// </summary>
        public string provincename
        {
            get { return _provincename; }
            set { _provincename = value; }
        }
        private string _alias;
        public string alias
        {
            get { return _alias; }
            set { _alias = value; }
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

        private string _zipcode;
        public string zipcode
        {
            get { return _zipcode; }
            set { _zipcode = value; }
        }
    }
}