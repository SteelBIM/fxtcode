using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.Domain.DTO
{
    public class BuildingDto
    {
        [JsonProperty(PropertyName = "id")]
        public Guid? AppId { get; set; }

        [JsonProperty(PropertyName = "buildingid")]
        public int BuildingId { get; set; }
        /// <summary>
        /// 楼栋名称
        /// </summary>
        [JsonProperty(PropertyName = "buildingname")]
        public string BuildingName { get; set; }

        [JsonProperty(PropertyName = "projectid")]
        public int ProjectId { get; set; }
        /// <summary>
        /// 楼栋用途
        /// </summary>
        [JsonProperty(PropertyName = "purposecode")]
        public int? PurposeCode { get; set; }
        /// <summary>
        /// 建筑结构
        /// </summary>
        [JsonProperty(PropertyName = "structurecode")]
        public int? StructureCode { get; set; }
        /// <summary>
        /// 建筑类型
        /// </summary>
        [JsonProperty(PropertyName = "buildingtypecode")]
        public int? BuildingTypeCode { get; set; }
        /// <summary>
        /// 总层数
        /// </summary>
        [JsonProperty(PropertyName = "totalfloor")]
        public int? TotalFloor { get; set; }
        /// <summary>
        /// 层高
        /// </summary>
        [JsonProperty(PropertyName = "floorhigh")]
        public decimal? FloorHigh { get; set; }
        /// <summary>
        /// 销售许可证
        /// </summary>
        [JsonProperty(PropertyName = "salelicence")]
        public string SaleLicence { get; set; }
        /// <summary>
        /// 梯户比
        /// </summary>
        [JsonProperty(PropertyName = "elevatorrate")]
        public string ElevatorRate { get; set; }
        /// <summary>
        /// 单元数
        /// </summary>
        [JsonProperty(PropertyName = "unitsnumber")]
        public int? UnitsNumber { get; set; }
        /// <summary>
        /// 总户数
        /// </summary>
        [JsonProperty(PropertyName = "totalnumber")]
        public int? TotalNumber { get; set; }
        /// <summary>
        /// 建筑面积
        /// </summary>
        [JsonProperty(PropertyName = "totalbuildarea")]
        public decimal? TotalBuildArea { get; set; }
        /// <summary>
        /// 建筑时间
        /// </summary>
        [JsonProperty(PropertyName = "builddate")]
        public DateTime? BuildDate { get; set; }
        /// <summary>
        /// 销售时间
        /// </summary>
        [JsonProperty(PropertyName = "saledate")]
        public DateTime? SaleDate { get; set; }
        /// <summary>
        /// 楼栋均价
        /// </summary>
        [JsonProperty(PropertyName = "averageprice")]
        public decimal? AveragePrice { get; set; }
        /// <summary>
        /// 均价层
        /// </summary>
        [JsonProperty(PropertyName = "averagefloor")]
        public int? AverageFloor { get; set; }
        /// <summary>
        /// 入伙时间
        /// </summary>
        [JsonProperty(PropertyName = "joindate")]
        public DateTime? JoinDate { get; set; }
        /// <summary>
        /// 预售时间
        /// </summary>
        [JsonProperty(PropertyName = "licencedate")]
        public DateTime? LicenceDate { get; set; }
        /// <summary>
        /// 楼栋别名
        /// </summary>
        [JsonProperty(PropertyName = "othername")]
        public string OtherName { get; set; }
        /// <summary>
        /// 价格系数
        /// </summary>
        [JsonProperty(PropertyName = "weight")]
        public decimal? Weight { get; set; }
        /// <summary>
        /// 是否可估
        /// </summary>
        [JsonProperty(PropertyName = "isevalue")]
        public int? IsEValue { get; set; }

        [JsonProperty(PropertyName = "cityid")]
        public int CityID { get; set; }

        /// <summary>
        /// 销售均价
        /// </summary>
        [JsonProperty(PropertyName = "saleprice")]
        public decimal? SalePrice { get; set; }

        /// <summary>
        /// 位置
        /// </summary>
        [JsonProperty(PropertyName = "locationcode")]
        public int? LocationCode { get; set; }
        /// <summary>
        /// 景观
        /// </summary>
        [JsonProperty(PropertyName = "sightcode")]
        public int? SightCode { get; set; }
        /// <summary>
        /// 朝向
        /// </summary>
        [JsonProperty(PropertyName = "frontcode")]
        public int? FrontCode { get; set; }

        [JsonProperty(PropertyName = "x")]
        public decimal? X { get; set; }

        [JsonProperty(PropertyName = "y")]
        public decimal? Y { get; set; }

        /// <summary>
        /// 外墙装修
        /// </summary>
        [JsonProperty(PropertyName = "wall")]
        public int? Wall { get; set; }
        /// <summary>
        /// 是否带电梯
        /// </summary>
        [JsonProperty(PropertyName = "iselevator")]
        public int? IsElevator { get; set; }
        /// <summary>
        /// 附属房屋均价
        /// </summary>
        [JsonProperty(PropertyName = "subaverageprice")]
        public decimal? SubAveragePrice { get; set; }
        /// <summary>
        /// 价格系数说明
        /// </summary>
        [JsonProperty(PropertyName = "pricedetail")]
        public string PriceDetail { get; set; }
        /// <summary>
        /// 户型面积
        /// </summary>
        [JsonProperty(PropertyName = "bhousetypecode")]
        public int? BHouseTypeCode { get; set; }
        /// <summary>
        /// 楼间距
        /// </summary>
        [JsonProperty(PropertyName = "distance")]
        public int? Distance { get; set; }
        /// <summary>
        /// 地下层数
        /// </summary>
        [JsonProperty(PropertyName = "basement")]
        public int? Basement { get; set; }
        /// <summary>
        /// 门牌号（地址）
        /// </summary>
        [JsonProperty(PropertyName = "doorplate")]
        public string Doorplate { get; set; }
        /// <summary>
        /// 产权形式
        /// </summary>
        [JsonProperty(PropertyName = "rightcode")]
        public int? RightCode { get; set; }
        /// <summary>
        /// 是否虚拟楼栋
        /// </summary>
        [JsonProperty(PropertyName = "isvirtual")]
        public int? IsVirtual { get; set; }
        /// <summary>
        /// 楼层分布
        /// </summary>
        [JsonProperty(PropertyName = "floorspread")]
        public string FloorSpread { get; set; }
        /// <summary>
        /// 裙楼层数
        /// </summary>
        [JsonProperty(PropertyName = "podiumbuildingfloor")]
        public int? PodiumBuildingFloor { get; set; }
        /// <summary>
        /// 裙楼面积
        /// </summary>
        [JsonProperty(PropertyName = "podiumbuildingarea")]
        public decimal? PodiumBuildingArea { get; set; }
        /// <summary>
        /// 塔楼面积
        /// </summary>
        [JsonProperty(PropertyName = "towerbuildingarea")]
        public decimal? TowerBuildingArea { get; set; }
        /// <summary>
        /// 地下室总面积
        /// </summary>
        [JsonProperty(PropertyName = "basementarea")]
        public decimal? BasementArea { get; set; }
        /// <summary>
        /// 地下室用途
        /// </summary>
        [JsonProperty(PropertyName = "basementpurpose")]
        public string BasementPurpose { get; set; }
        /// <summary>
        /// 住宅套数
        /// </summary>
        [JsonProperty(PropertyName = "housenumber")]
        public int? HouseNumber { get; set; }
        /// <summary>
        /// 住宅总面积
        /// </summary>
        [JsonProperty(PropertyName = "housearea")]
        public decimal? HouseArea { get; set; }
        /// <summary>
        /// 非住宅套数
        /// </summary>
        [JsonProperty(PropertyName = "othernumber")]
        public int? OtherNumber { get; set; }
        /// <summary>
        /// 非住宅面积
        /// </summary>
        [JsonProperty(PropertyName = "otherarea")]
        public decimal? OtherArea { get; set; }
        /// <summary>
        /// 内部装修
        /// </summary>
        [JsonProperty(PropertyName = "innerfitmentcode")]
        public int? InnerFitmentCode { get; set; }
        /// <summary>
        /// 单层户数
        /// </summary>
        [JsonProperty(PropertyName = "floorhousenumber")]
        public int? FloorHouseNumber { get; set; }
        /// <summary>
        /// 电梯数量
        /// </summary>
        [JsonProperty(PropertyName = "liftnumber")]
        public int? LiftNumber { get; set; }
        /// <summary>
        /// 电梯品牌
        /// </summary>
        [JsonProperty(PropertyName = "liftbrand")]
        public string LiftBrand { get; set; }
        /// <summary>
        /// 设备设施
        /// </summary>
        [JsonProperty(PropertyName = "facilities")]
        public string Facilities { get; set; }
        /// <summary>
        /// 管道燃气
        /// </summary>
        [JsonProperty(PropertyName = "pipelinegascode")]
        public int? PipelineGasCode { get; set; }
        /// <summary>
        /// 采暖方式
        /// </summary>
        [JsonProperty(PropertyName = "heatingmodecode")]
        public int? HeatingModeCode { get; set; }
        /// <summary>
        /// 墙体类型
        /// </summary>
        [JsonProperty(PropertyName = "walltypecode")]
        public int? WallTypeCode { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [JsonProperty(PropertyName = "remark")]
        public string Remark { get; set; }
        /// <summary>
        /// 是否带院子
        /// </summary>
        [JsonProperty(PropertyName = "isyard")]
        public int? IsYard { get; set; }
        /// <summary>
        /// 维护情况
        /// </summary>
        [JsonProperty(PropertyName = "maintenancecode")]
        public int? MaintenanceCode { get; set; }

        [JsonProperty(PropertyName = "valid")]
        public int Valid { get; set; }
        /// <summary>
        /// 楼栋图片数量
        /// </summary>
        [JsonProperty(PropertyName = "buildimagecount")]
        public int BuildImageCount { get; set; }


        [JsonProperty(PropertyName = "fxtbuildingid")]
        public int? FxtBuildingId { get; set; }

        [JsonProperty(PropertyName = "fxtcompanyid")]
        public int? FxtCompanyId { get; set; }

        [JsonProperty(PropertyName = "aaveuser")]
        public string SaveUser { get; set; }

        [JsonProperty(PropertyName = "creator")]
        public string Creator { get; set; }

        [JsonProperty(PropertyName = "houselist")]
        public List<HouseDto> HouseDtolist { get; set; }

    }
}
