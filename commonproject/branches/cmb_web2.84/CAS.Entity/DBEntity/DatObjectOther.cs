using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_Object_Other")]
    public class DatObjectOther : BaseTO
    {
        private long _objectid;
        /// <summary>
        /// 土地基础委估对象ID
        /// </summary>
        [SQLField("objectid", EnumDBFieldUsage.PrimaryKey)]
        public long objectid
        {
            get { return _objectid; }
            set { _objectid = value; }
        }
        private int _cityid;
        /// <summary>
        /// 城市ID
        /// </summary>
        public int cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        private int _fxtcompanyid;
        /// <summary>
        /// 评估机构ID
        /// </summary>
        public int fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
        }
        private decimal _subhousealltotalprice = 0;
        /// <summary>
        /// 所有附属房屋总价
        /// </summary>
        public decimal subhousealltotalprice
        {
            get { return _subhousealltotalprice; }
            set { _subhousealltotalprice = value; }
        }
        private decimal? _landunitprice;
        /// <summary>
        /// 土地单价
        /// </summary>
        public decimal? landunitprice
        {
            get { return _landunitprice; }
            set { _landunitprice = value; }
        }
        private decimal? _landtotalprice;
        /// <summary>
        /// 土地总价
        /// </summary>
        public decimal? landtotalprice
        {
            get { return _landtotalprice; }
            set { _landtotalprice = value; }
        }
        private decimal _landarea;
        /// <summary>
        /// 土地面积
        /// </summary>
        public decimal landarea
        {
            get { return _landarea; }
            set { _landarea = value; }
        }
    }
}