using CAS.Entity.BaseDAModels;

namespace CAS.Entity.CASEntity
{
    public class ProjectList : BaseTO
    {
        /// <summary>
        /// 楼盘ID
        /// </summary>
        public int projectid { get; set; }
        /// <summary>
        /// 楼盘名称
        /// </summary>
        public string projectname { get; set; }
        /// <summary>
        /// 是否可估
        /// </summary>
        public int isevalue { get; set; }
        /// <summary>
        /// 总数
        /// </summary>
        public int recordcount { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// 坐标X
        /// </summary>
        public decimal? x { get; set; }
        /// <summary>
        /// 坐标Y
        /// </summary>
        public decimal? y { get; set; }
        /// <summary>
        /// 区域ID
        /// </summary>
        public string areaid { get; set; }
        /// <summary>
        /// 区域名称
        /// </summary>
        public string areaname { get; set; }
        /// <summary>
        /// 系数
        /// </summary>
        public decimal weight { get; set; }

    }
}
