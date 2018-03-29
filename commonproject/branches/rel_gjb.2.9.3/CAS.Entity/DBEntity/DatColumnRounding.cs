using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    /// <summary>
    /// 坏账业务表
    /// </summary>
    [Serializable]
    [TableAttribute("dbo.Dat_ColumnRounding")]
    public class DatColumnRounding : BaseTO
    {
        private long _id;
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public long id
        {
            get { return _id; }
            set { _id = value; }
        }
        private long _objectid;
        /// <summary>
        /// 数据ID
        /// </summary>
        public long objectid
        {
            get { return _objectid; }
            set { _objectid = value; }
        }
        private int _codetype;
        /// <summary>
        /// 数据类型code(id:2018)
        /// </summary>
        public int codetype
        {
            get { return _codetype; }
            set { _codetype = value; }
        }
        private int _columntype;
        /// <summary>
        /// 字段类型(1:单价, 2:总价,3:净值,4:评估总值(主房+土地+附属))
        /// </summary>
        public int columntype
        {
            get { return _columntype; }
            set { _columntype = value; }
        }
        private int _roundingvalue;
        /// <summary>
        /// 精确到哪位(0:不取整,1:个位,10:十位,100:百位,1000:千位,10000:万位)
        /// </summary>
        public int roundingvalue
        {
            get { return _roundingvalue; }
            set { _roundingvalue = value; }
        }
        private int _datanodetype = 0;
        /// <summary>
        /// 委估对象价格阶段类型:预评、报告、询价，0为公共值
        /// </summary>
        public int datanodetype
        {
            get { return _datanodetype; }
            set { _datanodetype = value; }
        }
    }
}
