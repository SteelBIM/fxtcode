using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
