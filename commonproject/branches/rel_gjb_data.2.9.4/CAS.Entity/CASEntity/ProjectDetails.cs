using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.CASEntity
{
    public class ProjectDetails : BaseTO
    {
        /// <summary>
        /// 楼盘ID
        /// </summary>
        public int projectid { get; set; }
         /// <summary>
        /// 楼盘名
        /// </summary>
        public string projectname { get; set; }
         /// <summary>
        /// 区域名
        /// </summary>
        public string areaname { get; set; }
         /// <summary>
        /// 是否可估
        /// </summary>
        public int isevalue { get; set; }
         /// <summary>
        ///停车位
        /// </summary>
        public int? parkingnumber { get; set; }
         /// <summary>
        /// 物业管理费
        /// </summary>
        public string managerprice { get; set; }
         /// <summary>
        /// 地址
        /// </summary>
        public string address { get; set; }
         /// <summary>
        /// 区域ID
        /// </summary>
        public int areaid { get; set; }
         /// <summary>
        /// 开发商
        /// </summary>
        public string developcompanyname { get; set; }
         /// <summary>
        /// 物业管理公司
        /// </summary>
        public string managercompanyname { get; set; }
        /// <summary>
        /// 物业管理公司
        /// </summary>
        public DateTime? enddate { get; set; }
    }
}
