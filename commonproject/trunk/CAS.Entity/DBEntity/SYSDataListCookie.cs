using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.SYS_DataListCookie")]
    public class SYSDataListCookie : BaseTO
    {
        private int _id;
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _name;
        /// <summary>
        /// cookie名称
        /// </summary>
        public string name
        {
            get { return _name; }
            set { _name = value; }
        }
        private string _value;
        /// <summary>
        /// cookie值
        /// </summary>
        public string value
        {
            get { return _value; }
            set { _value = value; }
        }
        private bool? _valid;
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool? valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
    }
}