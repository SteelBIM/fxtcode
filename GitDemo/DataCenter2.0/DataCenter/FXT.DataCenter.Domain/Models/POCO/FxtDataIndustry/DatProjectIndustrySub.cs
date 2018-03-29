using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
//Nhibernate Code Generation Template 1.0
//author:MythXin
//blog:www.cnblogs.com/MythXin
//Entity Code Generation Template
using FXT.DataCenter.Infrastructure.Common.NPOI;

namespace FXT.DataCenter.Domain.Models
{
    public class DatProjectIndustrySub
    {
        /// <summary>
        /// 工业园区
        /// </summary>
        public long ProjectId { get; set; }
        /// <summary>
        /// CityId
        /// </summary>
        public int CityId { get; set; }
        /// <summary>
        /// 行政区ID
        /// </summary>
        public int AreaId { get; set; }
        /// <summary>
        /// 工业片区ID
        /// </summary>
        public int SubAreaId { get; set; }
        /// <summary>
        /// 与片区的关联度
        /// </summary>
        public int? CorrelationType { get; set; }
        /// <summary>
        /// 主用途1001
        /// </summary>
        public int? PurposeCode { get; set; }
        /// <summary>
        /// 楼盘名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 别名
        /// </summary>
        public string OtherName { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 宗地号
        /// </summary>
        public string FieldNo { get; set; }
        /// <summary>
        /// 土地面积_平方米
        /// </summary>
        public decimal? LandArea { get; set; }
        /// <summary>
        /// 土地起始日期
        /// </summary>
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// 土地终止日期
        /// </summary>
        public DateTime? StartEndDate { get; set; }
        /// <summary>
        /// 土地使用年限_年
        /// </summary>
        public int? UsableYear { get; set; }
        /// <summary>
        /// 总建筑面积_平方米
        /// </summary>
        public decimal? BuildingArea { get; set; }
        /// <summary>
        /// 容积率
        /// </summary>
        public decimal? CubageRate { get; set; }
        /// <summary>
        /// 绿化率_百分比
        /// </summary>
        public decimal? GreenRate { get; set; }
        /// <summary>
        /// 总栋数
        /// </summary>
        public int? BuildingNum { get; set; }
        /// <summary>
        /// 建筑类型2003
        /// </summary>
        public int? BuildingType { get; set; }
        /// <summary>
        /// 工业类型6013
        /// </summary>
        public int? IndustryType { get; set; }
        /// <summary>
        /// 竣工时间
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// 销售时间
        /// </summary>
        public DateTime? SaleDate { get; set; }
        /// <summary>
        /// 办公面积_平方米
        /// </summary>
        public decimal? OfficeArea { get; set; }
        /// <summary>
        /// 商业面积_平方米
        /// </summary>
        public decimal? BizArea { get; set; }
        /// <summary>
        /// 工业面积_平方米
        /// </summary>
        public decimal? IndustryArea { get; set; }
        /// <summary>
        /// 管理费
        /// </summary>
        public string ManagerPrice { get; set; }
        /// <summary>
        /// 管理处电话
        /// </summary>
        public string ManagerTel { get; set; }
        /// <summary>
        /// 交通便捷度1141
        /// </summary>
        public int? TrafficType { get; set; }
        /// <summary>
        /// 交通便捷度描述
        /// </summary>
        public string TrafficDetails { get; set; }
        /// <summary>
        /// 停车便捷度1141
        /// </summary>
        public int? ParkingLevel { get; set; }
        /// <summary>
        /// 车位类型1116
        /// </summary>
        public int? ParkingType { get; set; }
        /// <summary>
        /// 停车费
        /// </summary>
        public string ParkingPrice { get; set; }
        /// <summary>
        /// 租售方式1127
        /// </summary>
        public int? RentSaleType { get; set; }
        /// <summary>
        /// 空调系统类型1142
        /// </summary>
        public int? AirConditionType { get; set; }
        /// <summary>
        /// 项目概况
        /// </summary>
        public string Details { get; set; }
        /// <summary>
        /// 住宅楼盘库ID
        /// </summary>
        public long? ZZProjectId { get; set; }
        /// <summary>
        /// 四至东
        /// </summary>
        public string East { get; set; }
        /// <summary>
        /// 四至南
        /// </summary>
        public string south { get; set; }
        /// <summary>
        /// 四至西
        /// </summary>
        public string west { get; set; }
        /// <summary>
        /// 四至北
        /// </summary>
        public string north { get; set; }
        /// <summary>
        /// 拼音简写
        /// </summary>
        public string PinYin { get; set; }
        /// <summary>
        /// 楼盘名称全拼
        /// </summary>
        public string PinYinAll { get; set; }
        /// <summary>
        /// X
        /// </summary>
        public decimal? X { get; set; }
        /// <summary>
        /// Y
        /// </summary>
        public decimal? Y { get; set; }
        /// <summary>
        /// Remarks
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 楼盘均价
        /// </summary>
        public decimal? AveragePrice { get; set; }
        /// <summary>
        /// 创建人ID
        /// </summary>
        public string Creator { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 最后保存时间
        /// </summary>
        public DateTime? SaveDateTime { get; set; }
        /// <summary>
        /// 最后修改人ID
        /// </summary>
        public string SaveUser { get; set; }
        /// <summary>
        /// FxtCompanyId
        /// </summary>
        public int FxtCompanyId { get; set; }
        /// <summary>
        /// 是否有效
        /// </summary>
        public int Valid { get; set; }    
    }
}
