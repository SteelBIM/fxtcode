using Newtonsoft.Json;
using System;

/**
 * 作者: 曾智磊
 * 时间: 2014.03.03
 * 摘要: 新建实体类
 * **/
namespace FxtDataAcquisition.NHibernate.Entities
{
    //DAT_Building
    public class DATBuilding
    {

        [JsonProperty(PropertyName = "buildingid")]
        /// <summary>
        /// ID
        /// </summary>
        public virtual int BuildingId
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "buildingname")]
        /// <summary>
        /// 楼宇名称
        /// </summary>
        public virtual string BuildingName
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "doorplate")]
        /// <summary>
        /// 门牌号
        /// </summary>
        public virtual string Doorplate
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "projectid")]
        /// <summary>
        /// 楼盘ID
        /// </summary>
        public virtual int ProjectId
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "purposecode")]
        /// <summary>
        /// 楼宇用途
        /// </summary>
        public virtual int? PurposeCode
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "structurecode")]
        /// <summary>
        /// 建筑结构
        /// </summary>
        public virtual int? StructureCode
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "buildingtypecode")]
        /// <summary>
        /// 建筑类型
        /// </summary>
        public virtual int? BuildingTypeCode
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "totalfloor")]
        /// <summary>
        /// 总层数
        /// </summary>
        public virtual int? TotalFloor
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "floorhigh")]
        /// <summary>
        /// 层高
        /// </summary>
        public virtual decimal? FloorHigh
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "salelicence")]
        /// <summary>
        /// 销售许可证
        /// </summary>
        public virtual string SaleLicence
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "elevatorrate")]
        /// <summary>
        /// 梯户比
        /// </summary>
        public virtual string ElevatorRate
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "unitsnumber")]
        /// <summary>
        /// 单元数
        /// </summary>
        public virtual int? UnitsNumber
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "totalnumber")]
        /// <summary>
        /// 总户数
        /// </summary>
        public virtual int? TotalNumber
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "totalbuildarea")]
        /// <summary>
        /// 建筑面积
        /// </summary>
        public virtual decimal? TotalBuildArea
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "builddate")]
        /// <summary>
        /// 建筑时间
        /// </summary>
        public virtual DateTime? BuildDate
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "saledate")]
        /// <summary>
        /// 销售时间
        /// </summary>
        public virtual DateTime? SaleDate
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "averageprice")]
        /// <summary>
        /// 楼栋均价
        /// </summary>
        public virtual decimal? AveragePrice
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "averagefloor")]
        /// <summary>
        /// 均价层
        /// </summary>
        public virtual int? AverageFloor
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "joindate")]
        /// <summary>
        /// 入伙时间
        /// </summary>
        public virtual DateTime? JoinDate
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "licencedate")]
        /// <summary>
        /// 预售时间
        /// </summary>
        public virtual DateTime? LicenceDate
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "othername")]
        /// <summary>
        /// 楼栋别名
        /// </summary>
        public virtual string OtherName
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "weight")]
        /// <summary>
        /// 权重值
        /// </summary>
        public virtual decimal? Weight
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "isevalue")]
        /// <summary>
        /// 是否可估
        /// </summary>
        public virtual int? IsEValue
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "cityid")]
        /// <summary>
        /// 城市ID
        /// </summary>
        public virtual int CityID
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "createtime")]
        /// <summary>
        /// CreateTime
        /// </summary>
        public virtual DateTime? CreateTime
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "oldid")]
        /// <summary>
        /// OldId
        /// </summary>
        public virtual string OldId
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "valid")]
        /// <summary>
        /// Valid
        /// </summary>
        public virtual int? Valid
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "saleprice")]
        /// <summary>
        /// 销售均价
        /// </summary>
        public virtual decimal? SalePrice
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "savedatetime")]
        /// <summary>
        /// SaveDateTime
        /// </summary>
        public virtual DateTime? SaveDateTime
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "saveuser")]
        /// <summary>
        /// save user
        /// </summary>
        public virtual string SaveUser
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "locationcode")]
        /// <summary>
        /// 楼栋位置
        /// </summary>
        public virtual int? LocationCode
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "sightcode")]
        /// <summary>
        /// 楼栋景观
        /// </summary>
        public virtual int? SightCode
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "frontcode")]
        /// <summary>
        /// 楼栋朝向
        /// </summary>
        public virtual int? FrontCode
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "structureweight")]
        /// <summary>
        /// 楼栋结构修正价格
        /// </summary>
        public virtual decimal? StructureWeight
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "buildingtypeweight")]
        /// <summary>
        /// 建筑类型修正价格
        /// </summary>
        public virtual decimal? BuildingTypeWeight
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "yearweight")]
        /// <summary>
        /// 年期修正价格
        /// </summary>
        public virtual decimal? YearWeight
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "purposeweight")]
        /// <summary>
        /// 用途修正价格
        /// </summary>
        public virtual decimal? PurposeWeight
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "locationweight")]
        /// <summary>
        /// 楼栋位置修正价格
        /// </summary>
        public virtual decimal? LocationWeight
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "sightweight")]
        /// <summary>
        /// 景观修正价格
        /// </summary>
        public virtual decimal? SightWeight
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "frontweight")]
        /// <summary>
        /// 朝向修正价格
        /// </summary>
        public virtual decimal? FrontWeight
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "fxtcompanyid")]
        /// <summary>
        /// 评估机构ID
        /// </summary>
        public virtual int? FxtCompanyId
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "x")]
        /// <summary>
        /// X坐标
        /// </summary>
        public virtual decimal? X
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "y")]
        /// <summary>
        /// Y坐标
        /// </summary>
        public virtual decimal? Y
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "xyscale")]
        /// <summary>
        /// 比例尺
        /// </summary>
        public virtual int? XYScale
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "wall")]
        /// <summary>
        /// 外墙
        /// </summary>
        public virtual int? Wall
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "iselevator")]
        /// <summary>
        /// 是否带电梯
        /// </summary>
        public virtual int? IsElevator
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "subaverageprice")]
        /// <summary>
        /// 附属房屋均价
        /// </summary>
        public virtual decimal? SubAveragePrice
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "pricedetail")]
        /// <summary>
        /// 价格系数说明
        /// </summary>
        public virtual string PriceDetail
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "bhousetypecode")]
        /// <summary>
        /// 楼栋户型面积修正因素
        /// </summary>
        public virtual int? BHouseTypeCode
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "bhousetypeweight")]
        /// <summary>
        /// 楼栋户型面积修正价格
        /// </summary>
        public virtual decimal? BHouseTypeWeight
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "creator")]
        /// <summary>
        /// Creator
        /// </summary>
        public virtual string Creator
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "distance")]
        /// <summary>
        /// 楼间距
        /// </summary>
        public virtual int? Distance
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "distanceweight")]
        /// <summary>
        /// 楼间距系数
        /// </summary>
        public virtual decimal? DistanceWeight
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "basement")]
        /// <summary>
        /// 地下室层数
        /// </summary>
        public virtual int? basement
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "remark")]
        /// <summary>
        /// Remark
        /// </summary>
        public virtual string Remark
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "fxtbuildingid")]
        /// <summary>
        /// FxtBuildingId
        /// </summary>
        public virtual int? FxtBuildingId
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "status")]
        /// <summary>
        /// Status
        /// </summary>
        public virtual int? Status
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "maintenancecode")]
        /// <summary>
        /// 维护情况
        /// </summary>
        public virtual int? MaintenanceCode
        {
            get;
            set;
        }

    }
}