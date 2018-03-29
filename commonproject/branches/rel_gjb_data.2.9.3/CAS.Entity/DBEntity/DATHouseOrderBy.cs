using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.DAT_Building")]
    public class DATHouseOrderBy : BaseTO
    {
        public int houseid { get; set; }
        public string housename { get; set; }
        public string unitno { get; set; }
        public decimal buildarea { get; set; }
        public int isevalue { get; set; }
        public int subhousetype { get; set; }
        public decimal subhousearea { get; set; }
        public int floorno { get; set; }
        public int frontcode { get; set; }
        public int sightcode { get; set; }
        public int purposecode { get; set; }
        public int rownum { get; set; }
        public int recordcount { get; set; }
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
