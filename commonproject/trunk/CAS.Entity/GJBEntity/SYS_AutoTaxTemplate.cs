using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class SYS_AutoTaxTemplate : SYSAutoTaxTemplate
    {
        /// <summary>
        /// 创建人
        /// </summary>
        [SQLReadOnly]
        public string createusername { get; set; }
        /// <summary>
        /// 物业类型
        /// </summary>
        [SQLReadOnly]
        public string objecttypename { get; set; }
        /// <summary>
        /// gui
        /// </summary>
        [SQLReadOnly]
        public string guid { get; set; }
        [SQLReadOnly]
        public string cityname { get; set; }
        /// <summary>
        /// 路径
        /// </summary>
        [SQLReadOnly]
        public string path { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        [SQLReadOnly]
        public int provinceid { get; set; }
    }
}
