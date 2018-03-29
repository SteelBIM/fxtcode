using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_Object_SubHouse")]
    public class DatObjectSubHouse : BaseTO
    {
        private int _id;
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private long? _objectid;
        /// <summary>
        /// 对应的委估对象Id
        /// </summary>
        public long? objectid
        {
            get { return _objectid; }
            set { _objectid = value; }
        }
        private string _subhousearea;
        /// <summary>
        /// 附属房屋面积
        /// </summary>
        public string subhousearea
        {
            get { return _subhousearea; }
            set { _subhousearea = value; }
        }
        private decimal? _subhouseunitprice;
        /// <summary>
        /// 附属房屋单价
        /// </summary>
        public decimal? subhouseunitprice
        {
            get { return _subhouseunitprice; }
            set { _subhouseunitprice = value; }
        }
        private decimal? _subhousetotalprice;
        /// <summary>
        /// 附属房屋总价
        /// </summary>
        public decimal? subhousetotalprice
        {
            get { return _subhousetotalprice; }
            set { _subhousetotalprice = value; }
        }
        private int _subhousetype = 0;
        public int subhousetype
        {
            get { return _subhousetype; }
            set { _subhousetype = value; }
        }
        private int _valid = 1;
        public int valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
        private string _newaddflag;
        /// <summary>
        /// 新增标识
        /// </summary>
        public string newaddflag
        {
            get { return _newaddflag; }
            set { _newaddflag = value; }
        }
        private int _createuserid = 0;
        /// <summary>
        /// 创建人
        /// </summary>
        public int createuserid
        {
            get { return _createuserid; }
            set { _createuserid = value; }
        }
        private DateTime _createdate = DateTime.Now;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
    }
}
