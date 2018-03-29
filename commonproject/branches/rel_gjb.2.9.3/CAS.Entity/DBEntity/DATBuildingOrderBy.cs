using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.DAT_Building")]
    public class DATBuildingOrderBy : BaseTO
    {
        public int buildingid { get; set; }
        public int floortotal { get; set; }
        public int housetotal { get; set; }
        public string buildingname { get; set; }
        public DateTime builddate { get; set; }
        public int averageprice { get; set; }
        public int isevalue { get; set; }
        public decimal weight { get; set; }
        public decimal totalbuildarea { get; set; }
        public string codename { get; set; }
        public int purposecode { get; set; }
        public int buildingtypecode { get; set; }
        /// <summary>
        /// 用于转换：别名
        /// </summary>
        [SQLReadOnly]
        public string ob_othername { get; set; }
        /// <summary>
        /// 用于排序：开头数字
        /// </summary>
        [SQLReadOnly]
        public int ob_startnum { get; set; }
        /// <summary>
        /// 用于排序：开头字母
        /// </summary>
        [SQLReadOnly]
        public string ob_starletter { get; set; }
        /// <summary>
        /// 用于排序：文字
        /// </summary>
        [SQLReadOnly]
        public string ob_text { get; set; }
        /// <summary>
        /// 用于排序：数字
        /// </summary>
        [SQLReadOnly]
        public int ob_number { get; set; }
    }
}
