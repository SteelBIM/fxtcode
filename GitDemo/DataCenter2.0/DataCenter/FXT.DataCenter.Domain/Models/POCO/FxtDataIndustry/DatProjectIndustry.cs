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
   public class DatProjectIndustry
   {
       /// <summary>
       /// 工业园区
       /// </summary>
       [ExcelExportIgnore]
       public long ProjectId { get; set; }
       [ExcelExportIgnore]
       public int CityId { get; set; }
       [ExcelExportIgnore]
       public int AreaId { get; set; }
       [ExcelExportIgnore]
       public int SubAreaId { get; set; }

       [DisplayName("*行政区")]
       public string AreaName { get; set; }

       [DisplayName("工业片区")]
       public string SubAreaName { get; set; }

       [DisplayName("*工业楼盘")]
       public string ProjectName { get; set; }

       [DisplayName("别名")]
       public string OtherName { get; set; }

       [ExcelExportIgnore]
       [DisplayName("与片区的关联度1106")]
       public int? CorrelationType { get; set; }
       [DisplayName("与片区的关联度")]
       public string CorrelationTypeName { get; set; }

       [ExcelExportIgnore]
       [DisplayName("主用途1001")]
       public int? PurposeCode { get; set; }
       [DisplayName("主用途")]
       public string PurposeCodeName { get; set; }

       [DisplayName("地址")]
       public string Address { get; set; }

       [DisplayName("宗地号")]
       public string FieldNo { get; set; }

       [DisplayName("土地面积_平方米")]
       [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
       public decimal? LandArea { get; set; }

       [DisplayName("土地起始日期")]
       public DateTime? StartDate { get; set; }
       [DisplayName("土地终止日期")]
       public DateTime? StartEndDate { get; set; }

       [DisplayName("土地使用年限_年")]
       [RegularExpression(@"^[0-9]+$", ErrorMessage = "{0}必须是整型数字类型")]
       public int? UsableYear { get; set; }

       [DisplayName("总建筑面积_平方米")]
       [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
       public decimal? BuildingArea { get; set; }

       [DisplayName("容积率")]
       [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
       public decimal? CubageRate { get; set; }

       [DisplayName("绿化率_百分比")]
       [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
       public decimal? GreenRate { get; set; }

       [DisplayName("总栋数_栋")]
       [RegularExpression(@"^[0-9]+$", ErrorMessage = "{0}必须是整型数字类型")]
       public int? BuildingNum { get; set; }

       [ExcelExportIgnore]
       [DisplayName("建筑类型2003")]
       public int? BuildingType { get; set; }
       [DisplayName("建筑类型")]
       public string BuildingTypeName { get; set; }

       [ExcelExportIgnore]
       [DisplayName("工业类型6013")]
       public int? IndustryType { get; set; }
       [DisplayName("工业类型")]
       public string IndustryTypeName { get; set; }

       [DisplayName("竣工时间")]
       public DateTime? EndDate { get; set; }

       [DisplayName("销售时间")]
       public DateTime? SaleDate { get; set; }

       [DisplayName("办公面积_平方米")]
       [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
       public decimal? OfficeArea { get; set; }

       [DisplayName("商业面积_平方米")]
       [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
       public decimal? BizArea { get; set; }

       [DisplayName("工业面积_平方米")]
       [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
       public decimal? IndustryArea { get; set; }

       [DisplayName("管理费")]
       public string ManagerPrice { get; set; }

       [DisplayName("管理处电话")]
       public string ManagerTel { get; set; }

       [ExcelExportIgnore]
       [DisplayName("交通便捷度1141")]
       public int? TrafficType { get; set; }
       [DisplayName("交通便捷度")]
       public string TrafficTypeName { get; set; }

       [DisplayName("交通便捷度描述")]
       public string TrafficDetails { get; set; }

       [ExcelExportIgnore]
       [DisplayName("停车便捷度1141")]
       public int? ParkingLevel { get; set; }
       [DisplayName("停车便捷度")]
       public string ParkingLevelName { get; set; }

       [ExcelExportIgnore]
       [DisplayName("车位类型1116")]
       public int? ParkingType { get; set; }
       [DisplayName("车位类型")]
       public string ParkingTypeName { get; set; }

       [DisplayName("停车费")]
       public string ParkingPrice { get; set; }

       [ExcelExportIgnore]
       [DisplayName("租售方式1127")]
       public int? RentSaleType { get; set; }
       [DisplayName("租售方式")]
       public string RentSaleTypeName { get; set; }

       [ExcelExportIgnore]
       [DisplayName("空调系统类型1142")]
       public int? AirConditionType { get; set; }
       [DisplayName("空调系统类型")]
       public string AirConditionTypeName { get; set; }

       [DisplayName("项目概况")]
       public string Details { get; set; }

       [ExcelExportIgnore]
       public long? ZZProjectId { get; set; }

       [DisplayName("四至东")]
       public string East { get; set; }

       [DisplayName("四至南")]
       public string south { get; set; }

       [DisplayName("四至西")]
       public string west { get; set; }

       [DisplayName("四至北")]
       public string north { get; set; }

       [ExcelExportIgnore]
       [DisplayName("拼音简写")]
       public string PinYin { get; set; }

       [ExcelExportIgnore]
       [DisplayName("楼盘名称全拼")]
       public string PinYinAll { get; set; }

       [DisplayName("经度")]
       public decimal? X { get; set; }

       [DisplayName("纬度")]
       public decimal? Y { get; set; }

       [DisplayName("备注")]
       public string Remarks { get; set; }

       [DisplayName("楼盘均价_元每平方米")]
       [RegularExpression(@"^[0-9]+\.[0-9]+|[0-9]+$", ErrorMessage = "{0}必须是数字类型")]
       public decimal? AveragePrice { get; set; }

       [ExcelExportIgnore]
       public string Creator { get; set; }
       [ExcelExportIgnore]
       public DateTime CreateTime { get; set; }
       [ExcelExportIgnore]
       public DateTime? SaveDateTime { get; set; }
       [ExcelExportIgnore]
       public string SaveUser { get; set; }
       [ExcelExportIgnore]
       public int FxtCompanyId { get; set; }
       [ExcelExportIgnore]
       public int Valid { get; set; }         
    }
}
