using CAS.Entity.BaseDAModels;

namespace CAS.Entity.CASEntity
{
    public class Building : BaseTO
    {
        /// <summary>
        /// 楼栋ID
        /// </summary>
        public int buildingid { get; set; }
        /// <summary>
        /// 楼栋名
        /// </summary>
        public string buildingname { get; set; }
        /// <summary>
        /// 是否可估
        /// </summary>
        public int isevalue { get; set; }
        /// <summary>
        /// 系数
        /// </summary>
        public decimal weight { get; set; }
        /// <summary>
        /// 总楼层
        /// </summary>
        public int? totalfloor { get; set; }
        /// <summary>
        /// 总单元数
        /// </summary>
        public int? unitsnumber { get; set; }
        /// <summary>
        /// 总户数
        /// </summary>
        public int? totalnumber { get; set; }
        /// <summary>
        /// 楼栋门牌号地址 Alex 2016-10-19
        /// </summary>
        public string doorplate { get; set; }
        /// <summary>
        /// 楼栋地址
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// 总楼层数（接收商业总楼层字段）
        /// </summary>
        public int? bizfloor { get; set; }
    }
}
