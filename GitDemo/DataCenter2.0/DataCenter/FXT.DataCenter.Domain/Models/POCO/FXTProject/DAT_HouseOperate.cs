using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    /// <summary>
    /// 房号操作Model
    /// </summary>
    public class DAT_HouseOperate
    {
        /// <summary>
        /// 房号ID
        /// </summary>
        public string HouseId { get; set; }
        /// <summary>
        /// 楼栋ID
        /// </summary>
        public string BuildingId { get; set; }
        /// <summary>
        /// 房号名称
        /// </summary>
        public string HouseName { get; set; }
        /// <summary>
        /// 面积
        /// </summary>
        public string BuildArea { get; set; }
        /// <summary>
        /// 用途
        /// </summary>
        public string PurposeCode { get; set; }
        /// <summary>
        /// 附属房屋类型
        /// </summary>
        public string SubHouseType { get; set; }
        /// <summary>
        /// 附属房屋面积
        /// </summary>
        public string SubHouseArea { get; set; }
        /// <summary>
        /// 户型
        /// </summary>
        public string HouseTypeCode { get; set; }
        /// <summary>
        /// 户型结构
        /// </summary>
        public string StructureCode { get; set; }
        /// <summary>
        /// 初始单价
        /// </summary>
        public string UnitPrice { get; set; }
        /// <summary>
        /// 价格系数
        /// </summary>
        public string Weight { get; set; }
        /// <summary>
        /// 朝向
        /// </summary>
        public string FrontCode { get; set; }
        /// <summary>
        /// 景观
        /// </summary>
        public string SightCode { get; set; }
        /// <summary>
        /// 是否可估
        /// </summary>
        public string IsEValue { get; set; }
        /// <summary>
        /// 面积确认
        /// </summary>
        public string IsShowBuildingArea { get; set; }
        /// <summary>
        /// 通风采光
        /// </summary>
        public string VDCode { get; set; }
        /// <summary>
        /// 装修
        /// </summary>
        public string FitmentCode { get; set; }
        /// <summary>
        /// 是否有厨房
        /// </summary>
        public string Cookroom { get; set; }
        /// <summary>
        /// 阳台数
        /// </summary>
        public string Balcony { get; set; }
        /// <summary>
        /// 洗手间数
        /// </summary>
        public string Toilet { get; set; }
        /// <summary>
        /// 单元
        /// </summary>
        public string UnitNo { get; set; }
        /// <summary>
        /// 城市ID
        /// </summary>
        public string CityID { get; set; }
        /// <summary>
        /// 是否有效
        /// </summary>
        public string Valid { get; set; }
        /// <summary>
        /// 评估机构Id
        /// </summary>
        public string FxtCompanyId { get; set; }
        /// <summary>
        /// 物理楼层
        /// </summary>
        public string FloorNo { get; set; }
        /// <summary>
        /// 实际层
        /// </summary>
        public string NominalFloor { get; set; }
        /// <summary>
        /// 噪音情况
        /// </summary>
        public string NoiseCode { get; set; }
    }
}
