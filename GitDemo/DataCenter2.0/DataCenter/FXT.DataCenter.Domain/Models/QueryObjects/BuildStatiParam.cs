using System;
using System.ComponentModel;
using FXT.DataCenter.Infrastructure.Common.NPOI;

namespace FXT.DataCenter.Domain.Models.QueryObjects
{
    /// <summary>
    /// 楼栋信息统计Model
    /// </summary>
    public class BuildStatiParam
    {
        [DisplayName("城市名称")]
        public string CityName { get; set; }
        //[DisplayName("*行政区")]
        //public string AreaName { get; set; }
        //[DisplayName("片区")]
        //public string SubAreaName { get; set; }
        //[DisplayName("*楼盘名称")]
        //public string ProjectName { get; set; }
        [DisplayName("*楼栋名称")]
        public string BuildingName { get; set; }
        [DisplayName("楼栋别名")]
        public string OtherName { get; set; }
        [ExcelExportIgnore]
        public int? PurposeCode { get; set; }
        [DisplayName("*楼栋用途")]
        public string PurposeName
        {
            get
            {
                if (PurposeCode != null)
                {
                    if (PurposeCode == -1)
                    {
                        return "";
                    }
                    switch (PurposeCode)
                    {
                        case 1001001:
                            return "居住";
                        case 1001002:
                            return "居住(别墅)";
                        case 1001003:
                            return "居住(洋房)";
                        case 1001004:
                            return "商业";
                        case 1001005:
                            return "办公";
                        case 1001006:
                            return "工业";
                        case 1001007:
                            return "商业、居住";
                        case 1001008:
                            return "商业、办公";
                        case 1001009:
                            return "办公、居住";
                        case 1001010:
                            return "停车场";
                        case 1001011:
                            return "酒店";
                        case 1001012:
                            return "加油站";
                        case 1001013:
                            return "综合";
                        case 1001014:
                            return "其他";
                        default:
                            return PurposeCode.ToString();
                    }
                }
                return "";
            }
        }
        [ExcelExportIgnore]
        public int? StructureCode { get; set; }
        [DisplayName("建筑结构")]
        public string StructureName
        {
            get
            {
                if (StructureCode != null)
                {
                    if (StructureCode == -1)
                    {
                        return "";
                    }
                    switch (StructureCode)
                    {
                        case 2010001:
                            return "砖木结构";
                        case 2010002:
                            return "砖混结构";
                        case 2010003:
                            return "框架结构";
                        case 2010004:
                            return "框剪结构";
                        case 2010005:
                            return "框筒结构";
                        case 2010006:
                            return "钢结构";
                        case 2010007:
                            return "钢混结构";
                        case 2010008:
                            return "混合结构";
                        case 2010009:
                            return "内浇外砌";
                        case 2010010:
                            return "内浇外挂";
                        default:
                            return StructureCode.ToString();
                    }

                }
                return "";
            }
        }
        [ExcelExportIgnore]
        public int? BuildingTypeCode { get; set; }
        [DisplayName("建筑类型")]
        public string BuildingTypeName
        {
            get
            {
                if (BuildingTypeCode != null)
                {
                    if (BuildingTypeCode == -1)
                    {
                        return "";
                    }
                    switch (BuildingTypeCode)
                    {
                        case 2003001:
                            return "低层";
                        case 2003002:
                            return "多层";
                        case 2003003:
                            return "小高层";
                        case 2003004:
                            return "高层";
                        default:
                            return BuildingTypeCode.ToString();
                    }
                }
                return "";
            }
        }

        [DisplayName("产权形式")]
        [ExcelExportIgnore]
        public int? RightCode { get; set; }
        [DisplayName("产权形式")]
        public string RightCodeName
        {
            get
            {
                if (RightCode != null)
                {
                    if (RightCode == -1)
                    {
                        return "";
                    }
                    switch (RightCode)
                    {
                        case 2007001: return "商品房";
                        case 2007002: return "微利房";
                        case 2007003: return "福利房";
                        case 2007004: return "军产房";
                        case 2007005: return "集资房";
                        case 2007006: return "自建房";
                        case 2007007: return "经济适用房";
                        case 2007008: return "小产权房";
                        case 2007009: return "限价房";
                        case 2007010: return "解困房";
                        case 2007011: return "宅基地";
                        case 2007012: return "房改房";
                        case 2007013: return "平改房";
                        case 2007014: return "回迁房";
                        case 2007015:
                            return "安置房";
                        default:
                            return RightCode.ToString();
                    }
                }
                return "";
            }
        }

        [DisplayName("销售许可证")]
        public string SaleLicence { get; set; }
        [DisplayName("*总层数")]
        public int? TotalFloor { get; set; }
        [DisplayName("*是否确认总层数")]
        [ExcelExportIgnore]
        public int? isTotalFloor { get; set; }
        [DisplayName("*是否确认总层数")]
        public string isTotalFloorName
        {
            get
            {
                if (isTotalFloor != null)
                {
                    if (isTotalFloor == -1)
                    {
                        return "";
                    }
                    switch (isTotalFloor)
                    {
                        case 1:
                            return "是";
                        case 0:
                            return "否";
                        default:
                            return isTotalFloor.ToString();
                    }
                }
                return "";
            }
        }
        [DisplayName("层高")]
        public decimal? FloorHigh { get; set; }
        [DisplayName("单元数")]
        public int? UnitsNumber { get; set; }
        [DisplayName("梯户比")]
        public string ElevatorRate { get; set; }
        [DisplayName("单层户数")]
        public int? FloorHouseNumber { get; set; }
        [DisplayName("总户数")]
        public int? TotalNumber { get; set; }
        [DisplayName("实际总套数")]
        public int HouseNum { get; set; }
        [DisplayName("建筑面积")]
        public decimal? TotalBuildArea { get; set; }
        [DisplayName("建筑时间")]
        public DateTime? BuildDate { get; set; }
        [DisplayName("楼栋均价")]
        public decimal? AveragePrice { get; set; }
        [DisplayName("均价层")]
        public int? AverageFloor { get; set; }
        [DisplayName("销售均价")]
        public decimal? SalePrice { get; set; }
        [DisplayName("销售时间")]
        public DateTime? SaleDate { get; set; }
        [DisplayName("预售时间")]
        public DateTime? LicenceDate { get; set; }
        [DisplayName("入伙时间")]
        public DateTime? JoinDate { get; set; }
        [DisplayName("楼间距")]
        public int? Distance { get; set; }
        [DisplayName("楼间距系数")]
        public decimal? DistanceWeight { get; set; }
        [ExcelExportIgnore]
        public int? Wall { get; set; }
        [DisplayName("外墙装修")]
        public string WallName
        {
            get
            {
                if (Wall != null)
                {
                    if (Wall == -1)
                    {
                        return "";
                    }
                    switch (Wall)
                    {
                        case 6058001:
                            return "涂料";
                        case 6058002:
                            return "马赛克";
                        case 6058003:
                            return "条形砖";
                        case 6058004:
                            return "玻璃幕墙";
                        case 6058005:
                            return "铝复板";
                        case 6058006:
                            return "大理石";
                        case 6058007:
                            return "花岗石";
                        case 6058008:
                            return "瓷片";
                        case 6058009:
                            return "墙砖";
                        case 6058010:
                            return "水刷石";
                        case 6058011:
                            return "清水墙";
                        case 6058012:
                            return "其他";
                        case 6058013:
                            return "水泥砂浆";
                        default:
                            return Wall.ToString();
                    }
                }
                return "";
            }
        }
        [ExcelExportIgnore]
        public int? WallTypeCode { get; set; }
        [DisplayName("墙体类型")]
        public string WallTypeName { get; set; }
        [ExcelExportIgnore]
        public int? innerFitmentCode { get; set; }
        [DisplayName("内部装修")]
        public string innerFitmentName
        {
            get
            {
                if (innerFitmentCode != null)
                {
                    if (innerFitmentCode <= 0)
                    {
                        return "";
                    }
                    switch (innerFitmentCode)
                    {
                        case 6026001:
                            return "豪华";
                        case 6026002:
                            return "高档";
                        case 6026003:
                            return "中档";
                        case 6026004:
                            return "普通";
                        case 6026005:
                            return "简易";
                        case 6026006:
                            return "毛坯";
                        default:
                            return innerFitmentCode.ToString();
                    }
                }
                return "";
            }
        }
        [DisplayName("设备设施")]
        public string Facilities { get; set; }
        [ExcelExportIgnore]
        public int? PipelineGasCode { get; set; }
        [DisplayName("管道燃气")]
        public string PipelineGasName { get; set; }
        [ExcelExportIgnore]
        public int? HeatingModeCode { get; set; }
        [DisplayName("采暖方式")]
        public string HeatingModeName { get; set; }
        [DisplayName("门牌号（地址）")]
        public string Doorplate { get; set; }
        [ExcelExportIgnore]
        public int? IsElevator { get; set; }
        [DisplayName("是否带电梯")]
        public string IsElevatorName
        {
            get
            {
                if (IsElevator != null)
                {
                    if (IsElevator == -1)
                    {
                        return "";
                    }
                    switch (IsElevator)
                    {
                        case 1:
                            return "是";
                        case 0:
                            return "否";
                        default:
                            return IsElevator.ToString();
                    }
                }
                return "";
            }
        }
        [DisplayName("电梯数量")]
        public int? LiftNumber { get; set; }
        [DisplayName("电梯品牌")]
        public string LiftBrand { get; set; }
        [DisplayName("楼层分布")]
        public string FloorSpread { get; set; }
        [DisplayName("裙楼层数")]
        public int? PodiumBuildingFloor { get; set; }
        [DisplayName("裙楼面积")]
        public decimal? PodiumBuildingArea { get; set; }
        [DisplayName("塔楼面积")]
        public decimal? TowerBuildingArea { get; set; }
        [DisplayName("住宅套数")]
        public int? HouseNumber { get; set; }
        [DisplayName("住宅总面积")]
        public decimal? HouseArea { get; set; }
        [DisplayName("非住宅套数")]
        public int? OtherNumber { get; set; }
        [DisplayName("非住宅总面积")]
        public decimal? OtherArea { get; set; }
        [DisplayName("地下室层数")]
        public int? basement { get; set; }
        [DisplayName("地下室总面积")]
        public decimal? BasementArea { get; set; }
        [DisplayName("地下室用途")]
        public string BasementPurpose { get; set; }
        [ExcelExportIgnore]
        public int? IsYard { get; set; }
        [DisplayName("是否带院子")]
        public string IsYardName
        {
            get
            {
                if (IsYard != null)
                {
                    if (IsYard == -1)
                    {
                        return "";
                    }
                    switch (IsYard)
                    {
                        case 1:
                            return "是";
                        case 0:
                            return "否";
                        default:
                            return IsYard.ToString();
                    }
                }
                return "";
            }
        }
        [DisplayName("经度")]
        public decimal? X { get; set; }
        [DisplayName("纬度")]
        public decimal? Y { get; set; }
        [DisplayName("比例尺")]
        public int? XYScale { get; set; }
        [ExcelExportIgnore]
        public int? LocationCode { get; set; }
        [DisplayName("楼栋位置")]
        public string LocationName
        {
            get
            {
                if (LocationCode != null)
                {
                    if (LocationCode <= 0)
                    {
                        return "";
                    }
                    switch (LocationCode)
                    {
                        case 2011001:
                            return "无特别因素";
                        case 2011002:
                            return "临公园、绿地";
                        case 2011003:
                            return "临江、河、湖";
                        case 2011004:
                            return "临噪音源(路、桥、工厂)";
                        case 2011005:
                            return "临垃圾站、医院";
                        case 2011006:
                            return "临变电站、高压线";
                        case 2011007:
                            return "临其他不利因素";
                        case 2011008:
                            return "临小区中庭";
                        default:
                            return LocationCode.ToString();
                    }
                }
                return "";
            }
        }
        [ExcelExportIgnore]
        public int? SightCode { get; set; }
        [DisplayName("楼栋景观")]
        public string SightName
        {
            get
            {
                if (SightCode != null)
                {
                    if (SightCode <= 0)
                    {
                        return "";
                    }
                    switch (SightCode)
                    {
                        case 2006001:
                            return "公园景观";
                        case 2006002:
                            return "绿地景观";
                        case 2006003:
                            return "小区景观";
                        case 2006004:
                            return "街景";
                        case 2006005:
                            return "市景";
                        case 2006006:
                            return "海景";
                        case 2006007:
                            return "山景";
                        case 2006008:
                            return "江景";
                        case 2006009:
                            return "湖景";
                        case 2006010:
                            return "无特别景观";
                        case 2006011:
                            return "小区绿地";
                        case 2006012:
                            return "河景";
                        case 2006013:
                            return "有建筑物遮挡";
                        case 2006014:
                            return "临高架桥";
                        case 2006015:
                            return "临铁路";
                        case 2006016:
                            return "临其他厌恶设施";
                        default:
                            return SightCode.ToString();
                    }
                }
                return "";
            }

        }
        [ExcelExportIgnore]
        public int? FrontCode { get; set; }
        [DisplayName("楼栋朝向")]
        public string FrontName
        {
            get
            {
                if (FrontCode != null)
                {
                    if (FrontCode <= 0)
                    {
                        return "";
                    }
                    switch (FrontCode)
                    {
                        case 2004001:
                            return "东";
                        case 2004002:
                            return "南";
                        case 2004003:
                            return "西";
                        case 2004004:
                            return "北";
                        case 2004005:
                            return "东南";
                        case 2004006:
                            return "东北";
                        case 2004007:
                            return "西南";
                        case 2004008:
                            return "西北";
                        case 2004009:
                            return "南北";
                        case 2004010:
                            return "东西";
                        default:
                            return FrontCode.ToString();
                    }
                }
                return "";
            }

        }
        [DisplayName("楼栋结构修正价格")]
        public decimal? StructureWeight { get; set; }
        [DisplayName("建筑类型修正价格")]
        public decimal? BuildingTypeWeight { get; set; }
        [DisplayName("年期修正价格")]
        public decimal? YearWeight { get; set; }
        [DisplayName("用途修正价格")]
        public decimal? PurposeWeight { get; set; }
        [DisplayName("楼栋位置修正价格")]
        public decimal? LocationWeight { get; set; }
        [DisplayName("景观修正价格")]
        public decimal? SightWeight { get; set; }
        [DisplayName("朝向修正价格")]
        public decimal? FrontWeight { get; set; }
        [DisplayName("梯户比修正价格")]
        public decimal? ElevatorRateWeight { get; set; }
        [ExcelExportIgnore]
        public int? BHouseTypeCode { get; set; }
        [DisplayName("楼栋户型面积修正因素")]
        public string BHouseTypeName
        {
            get
            {
                if (BHouseTypeCode != null)
                {
                    if (BHouseTypeCode == -1)
                    {
                        return "";
                    }
                    switch (BHouseTypeCode)
                    {
                        case 2016001:
                            return "小户型";
                        case 2016002:
                            return "大户型";
                        case 2016003:
                            return "复式户型";
                        case 2016004:
                            return "特殊户型";
                        default:
                            return BHouseTypeCode.ToString();
                    }
                }
                return "";
            }
        }
        [DisplayName("楼栋户型面积修正价格")]
        public decimal? BHouseTypeWeight { get; set; }
        [DisplayName("带院子修正价格")]
        public decimal? YardWeight { get; set; }
        [DisplayName("附属房屋均价")]
        public decimal? SubAveragePrice { get; set; }
        [DisplayName("价格系数说明")]
        public string PriceDetail { get; set; }
        [DisplayName("楼栋系数")]
        public decimal? Weight { get; set; }
        [ExcelExportIgnore]
        public int? IsVirtual { get; set; }
        [DisplayName("是否虚拟楼栋")]
        public string IsVirtualName
        {
            get
            {
                if (IsVirtual != null)
                {
                    if (IsVirtual == -1)
                    {
                        return "";
                    }
                    switch (IsVirtual)
                    {
                        case 1:
                            return "是";
                        case 0:
                            return "否";
                        default:
                            return IsVirtual.ToString();
                    }
                }
                return "";
            }
        }
        [ExcelExportIgnore]
        public int? IsEValue { get; set; }
        [DisplayName("是否可估")]
        public string IsEValueName
        {
            get
            {
                if (IsEValue != null)
                {
                    if (IsEValue == -1)
                    {
                        return "";
                    }
                    switch (IsEValue)
                    {
                        case 1:
                            return "是";
                        case 0:
                            return "否";
                        default:
                            return IsEValue.ToString();
                    }
                }
                return "";
            }
        }
        [DisplayName("备注")]
        public string Remark { get; set; }

        [ExcelExportIgnore]
        public int? MaintenanceCode { get; set; }
        [DisplayName("维护情况")]
        public string MaintenanceCodeName { get; set; }

        #region 扩展字段
        [DisplayName("创建用户")]
        [ExcelExportIgnore]
        public string Creator { get; set; }
        [DisplayName("创建时间")]
        [ExcelExportIgnore]
        public DateTime? CreateTime { get; set; }
        [DisplayName("保存用户")]
        [ExcelExportIgnore]
        public string SaveUser { get; set; }
        [DisplayName("保存时间")]
        [ExcelExportIgnore]
        public DateTime? SaveDateTime { get; set; }

        [ExcelExportIgnore]
        public int? FxtCompanyId { get; set; }

        [DisplayName("数据建设机构")]
        [ExcelExportIgnore]
        public int belongcompanyid { get; set; }
        [ExcelExportIgnore]
        public string FxtCompanyname { get; set; }

        [DisplayName("数据建设机构")]
        public string belongcompanyname { get; set; }
        [ExcelExportIgnore]
        public int CityID { get; set; }
        [ExcelExportIgnore]
        public int BuildingId { get; set; }
        [ExcelExportIgnore]
        public int ProjectId { get; set; }
        [DisplayName("行政区ID")]
        [ExcelExportIgnore]
        public int AreaId { get; set; }
        [DisplayName("片区ID")]
        [ExcelExportIgnore]
        public int SubAreaId { get; set; }
        [DisplayName("楼盘名称")]
        [ExcelExportIgnore]
        public string ProjectName { get; set; }
        [DisplayName("OldId")]
        [ExcelExportIgnore]
        public string OldId { get; set; }
        [ExcelExportIgnore]
        public int? Valid { get; set; }

        [ExcelExportIgnore]
        public int TotalFloorCount { get; set; }

        [DisplayName("创建开始日期")]
        [ExcelExportIgnore]
        public DateTime? BstarTime { get; set; }

        [DisplayName("创建结束日期")]
        [ExcelExportIgnore]
        public DateTime? BendTime { get; set; }

        [DisplayName("系数")]
        [ExcelExportIgnore]
        public decimal? WeightTo { get; set; }

        [DisplayName("总楼层")]
        [ExcelExportIgnore]
        public int TotalFloorCompany_build { get; set; }

        [DisplayName("总户数")]
        [ExcelExportIgnore]
        public int TotalNumberCompany_build { get; set; }

        [DisplayName("楼栋系数")]
        [ExcelExportIgnore]
        public int BuildWeightCompany_build { get; set; }

        [DisplayName("建筑面积")]
        [ExcelExportIgnore]
        public int TotalBuildAreaCompany_build { get; set; }

        [DisplayName("单元数")]
        [ExcelExportIgnore]
        public int UnitsNumberCompany_build { get; set; }

        [DisplayName("地下层数")]
        [ExcelExportIgnore]
        public int basementCompany_build { get; set; }

        [DisplayName("标准高层")]
        [ExcelExportIgnore]
        public int FloorHighCompany_build { get; set; }

        [DisplayName("销售时间截止日")]
        [ExcelExportIgnore]
        public DateTime? BuildSaleDateTo { get; set; }

        [DisplayName("销售时间")]
        [ExcelExportIgnore]
        public DateTime? BuildSaleDate { get; set; }

        [DisplayName("竣工时间截止日")]
        [ExcelExportIgnore]
        public DateTime? BuildDateTo { get; set; }

        [DisplayName("页索引")]
        [ExcelExportIgnore]
        public int? pageindex { get; set; }
        [ExcelExportIgnore]
        public string orderbyName { get; set; }
        [ExcelExportIgnore]
        public int? orderby { get; set; }
        #endregion
    }
}