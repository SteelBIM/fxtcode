using FxtDataAcquisition.Domain.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.Domain.DTO
{
    public class ProjectDto
    {
        #region 楼盘基本信息
        [JsonProperty(PropertyName = "projectid")]
        public int ProjectId { get; set; }
        /// <summary>
        /// 数据中心楼盘ID
        /// </summary>
        [JsonProperty(PropertyName = "fxtprojectid")]
        public int? FxtProjectId { get; set; }
        /// <summary>
        /// 楼盘名称
        /// </summary>
        [JsonProperty(PropertyName = "projectname")]
        public string ProjectName { get; set; }
        /// <summary>
        /// 宗地号
        /// </summary>
        [JsonProperty(PropertyName = "fieldno")]
        public string FieldNo { get; set; }
        /// <summary>
        /// 主用途
        /// </summary>
        [JsonProperty(PropertyName = "purposecode")]
        public int PurposeCode { get; set; }
        /// <summary>
        /// 楼盘地址
        /// </summary>
        [JsonProperty(PropertyName = "address")]
        public string Address { get; set; }
        /// <summary>
        /// 占地面积
        /// </summary>
        [JsonProperty(PropertyName = "landarea")]
        public decimal? LandArea { get; set; }
        /// <summary>
        /// 土地起始日期
        /// </summary>
        [JsonProperty(PropertyName = "startdate")]
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// 土地使用年限
        /// </summary>
        [JsonProperty(PropertyName = "usableyear")]
        public int? UsableYear { get; set; }
        /// <summary>
        /// 建筑面积
        /// </summary>
        [JsonProperty(PropertyName = "buildingarea")]
        public decimal? BuildingArea { get; set; }
        /// <summary>
        /// 可销售面积
        /// </summary>
        [JsonProperty(PropertyName = "salablearea")]
        public decimal? SalableArea { get; set; }
        /// <summary>
        /// 容积率
        /// </summary>
        [JsonProperty(PropertyName = "cubagerate")]
        public decimal? CubageRate { get; set; }
        /// <summary>
        /// 绿化率
        /// </summary>
        [JsonProperty(PropertyName = "greenrate")]
        public decimal? GreenRate { get; set; }
        /// <summary>
        /// 开工日期
        /// </summary>
        [JsonProperty(PropertyName = "buildingdate")]
        public DateTime? BuildingDate { get; set; }
        /// <summary>
        /// 封顶日期
        /// </summary>
        [JsonProperty(PropertyName = "coverdate")]
        public DateTime? CoverDate { get; set; }
        /// <summary>
        /// 开盘日期
        /// </summary>
        [JsonProperty(PropertyName = "saledate")]
        public DateTime? SaleDate { get; set; }
        /// <summary>
        /// 入伙日期
        /// </summary>
        [JsonProperty(PropertyName = "joindate")]
        public DateTime? JoinDate { get; set; }
        /// <summary>
        /// 竣工时间
        /// </summary>
        [JsonProperty(PropertyName = "enddate")]
        public DateTime? Enddate { get; set; }
        /// <summary>
        /// 内部认购日期
        /// </summary>
        [JsonProperty(PropertyName = "innersaledate")]
        public DateTime? InnerSaleDate { get; set; }
        /// <summary>
        /// 产权形式
        /// </summary>
        [JsonProperty(PropertyName = "rightcode")]
        public int? RightCode { get; set; }
        /// <summary>
        /// 车位数
        /// </summary>
        [JsonProperty(PropertyName = "parkingnumber")]
        public int? Parkingnumber { get; set; }
        /// <summary>
        /// 项目均价
        /// </summary>
        [JsonProperty(PropertyName = "averageprice")]
        public decimal? AveragePrice { get; set; }
        /// <summary>
        /// 管理处电话
        /// </summary>
        [JsonProperty(PropertyName = "managertel")]
        public string ManagerTel { get; set; }
        /// <summary>
        /// 物业费
        /// </summary>
        [JsonProperty(PropertyName = "managerprice")]
        public string ManagerPrice { get; set; }
        /// <summary>
        /// 总套数
        /// </summary>
        [JsonProperty(PropertyName = "totalnum")]
        public int? Totalnum { get; set; }
        /// <summary>
        /// 总栋数
        /// </summary>
        [JsonProperty(PropertyName = "buildingnum")]
        public int? BuildingNum { get; set; }
        /// <summary>
        /// 项目概况
        /// </summary>
        [JsonProperty(PropertyName = "detail")]
        public string Detail { get; set; }
        /// <summary>
        /// 主建筑物类型
        /// </summary>
        [JsonProperty(PropertyName = "buildingtypecode")]
        public int? BuildingTypeCode { get; set; }
        /// <summary>
        /// 办公面积
        /// </summary>
        [JsonProperty(PropertyName = "officearea")]
        public decimal? OfficeArea { get; set; }
        /// <summary>
        /// 其他用途面积
        /// </summary>
        [JsonProperty(PropertyName = "otherarea")]
        public decimal? OtherArea { get; set; }
        /// <summary>
        /// 土地规划用途
        /// </summary>
        [JsonProperty(PropertyName = "planpurpose")]
        public string PlanPurpose { get; set; }
        /// <summary>
        /// 价格调查时间
        /// </summary>
        [JsonProperty(PropertyName = "pricedate")]
        public DateTime? PriceDate { get; set; }
        /// <summary>
        /// 是否完成基础数据
        /// </summary>
        [JsonProperty(PropertyName = "iscomplete")]
        public int? IsComplete { get; set; }
        /// <summary>
        /// 别名
        /// </summary>
        [JsonProperty(PropertyName = "othername")]
        public string OtherName { get; set; }
        /// <summary>
        /// 商业面积
        /// </summary>
        [JsonProperty(PropertyName = "businessarea")]
        public decimal? BusinessArea { get; set; }
        /// <summary>
        /// 工业面积
        /// </summary>
        [JsonProperty(PropertyName = "industryarea")]
        public decimal? IndustryArea { get; set; }
        /// <summary>
        /// 是否可估
        /// </summary>
        [JsonProperty(PropertyName = "isevalue")]
        public int? IsEValue { get; set; }
        /// <summary>
        /// 拼音简写
        /// </summary>
        [JsonProperty(PropertyName = "pinyin")]
        public string PinYin { get; set; }
        /// <summary>
        /// 城市ID
        /// </summary>
        [JsonProperty(PropertyName = "cityid")]
        public int CityID { get; set; }
        /// <summary>
        /// 行政区
        /// </summary>
        [JsonProperty(PropertyName = "areaid")]
        public int AreaID { get; set; }
        /// <summary>
        /// 行政区
        /// </summary>
        [JsonProperty(PropertyName = "areaname")]
        public string AreaName { get; set; }
        /// <summary>
        /// 区域ID
        /// </summary>
        [JsonProperty(PropertyName = "subareaid")]
        public int? SubAreaId { get; set; }
        /// <summary>
        /// 开盘均价
        /// </summary>
        [JsonProperty(PropertyName = "saleprice")]
        public decimal? SalePrice { get; set; }
        /// <summary>
        /// 楼盘名称全拼
        /// </summary>
        [JsonProperty(PropertyName = "pinyinall")]
        public string PinYinAll { get; set; }
        /// <summary>
        /// 楼栋坐标x
        /// </summary>
        [JsonProperty(PropertyName = "x")]
        public decimal? X { get; set; }
        /// <summary>
        /// 楼栋坐标y
        /// </summary>
        [JsonProperty(PropertyName = "y")]
        public decimal? Y { get; set; }
        /// <summary>
        /// 是否空楼盘
        /// </summary>
        [JsonProperty(PropertyName = "isempty")]
        public int? IsEmpty { get; set; }
        /// <summary>
        /// 东
        /// </summary>
        [JsonProperty(PropertyName = "east")]
        public string East { get; set; }
        /// <summary>
        /// 西
        /// </summary>
        [JsonProperty(PropertyName = "west")]
        public string West { get; set; }
        /// <summary>
        /// 南
        /// </summary>
        [JsonProperty(PropertyName = "south")]
        public string South { get; set; }
        /// <summary>
        /// 北
        /// </summary>
        [JsonProperty(PropertyName = "north")]
        public string North { get; set; }
        /// <summary>
        /// 停车状况
        /// </summary>
        [JsonProperty(PropertyName = "parkingstatus")]
        public int? ParkingStatus { get; set; }
        /// <summary>
        /// 建筑质量
        /// </summary>
        [JsonProperty(PropertyName = "buildingquality")]
        public int? BuildingQuality { get; set; }
        /// <summary>
        /// 小区规模
        /// </summary>
        [JsonProperty(PropertyName = "housingscale")]
        public int? HousingScale { get; set; }
        /// <summary>
        /// 楼栋备注
        /// </summary>
        [JsonProperty(PropertyName = "buildingeetail")]
        public string BuildingDetail { get; set; }
        /// <summary>
        /// 房号备注
        /// </summary>
        [JsonProperty(PropertyName = "housedetail")]
        public string HouseDetail { get; set; }
        /// <summary>
        /// 地下室用途
        /// </summary>
        [JsonProperty(PropertyName = "basementpurpose")]
        public string BasementPurpose { get; set; }
        /// <summary>
        /// 物业管理质量
        /// </summary>
        [JsonProperty(PropertyName = "managerquality")]
        public int? Managerquality { get; set; }
        /// <summary>
        /// 设备设施
        /// </summary>
        [JsonProperty(PropertyName = "pacilities")]
        public string Facilities { get; set; }
        /// <summary>
        /// 配套等级
        /// </summary>
        [JsonProperty(PropertyName = "appendageclass")]
        public int? AppendageClass { get; set; }
        /// <summary>
        /// 区域分析
        /// </summary>
        [JsonProperty(PropertyName = "regionalanalysis")]
        public string RegionalAnalysis { get; set; }
        /// <summary>
        /// 有利因素
        /// </summary>
        [JsonProperty(PropertyName = "wrinkle")]
        public string Wrinkle { get; set; }
        /// <summary>
        /// 不利因素
        /// </summary>
        [JsonProperty(PropertyName = "aversion")]
        public string Aversion { get; set; }
        /// <summary>
        /// 车位描述
        /// </summary>
        [JsonProperty(PropertyName = "parkingdesc")]
        public string ParkingDesc { get; set; }

        [JsonProperty(PropertyName = "subareaname")]
        public string SubAreaName { get; set; }

        #endregion

        /// <summary>
        /// 当前楼栋数
        /// </summary>
        [JsonProperty(PropertyName = "tatolbuilddingnume")]
        public int TatolBuildingNum { get; set; }
        /// <summary>
        /// 照片数
        /// </summary>
        [JsonProperty(PropertyName = "photocount")]
        public int PhotoCount { get; set; }
        /// <summary>
        /// 状态更新时间
        /// </summary>
        [JsonProperty(PropertyName = "statedate")]
        public DateTime? StateDate { get; set; }
        /// <summary>
        /// 开发商
        /// </summary>
        [JsonProperty(PropertyName = "developers")]
        public string Developers { get; set; }
        /// <summary>
        /// 物业管理公司
        /// </summary>
        [JsonProperty(PropertyName = "manager_company")]
        public string ManagerCompany { get; set; }

        /// <summary>
        /// 任务备注
        /// </summary>
        [JsonProperty(PropertyName = "allotflowremark")]
        public string AllotFlowremark { get; set; }
        /// <summary>
        /// 任务id
        /// </summary>
        [JsonProperty(PropertyName = "allotid")]
        public long Allotid { get; set; }

        [JsonProperty(PropertyName = "fxtcompanyId")]
        public int? FxtCompanyId { get; set; }

        [JsonProperty(PropertyName = "saveuser")]
        public string SaveUser { get; set; }

        [JsonProperty(PropertyName = "creator")]
        public string Creator { get; set; }

        [JsonProperty(PropertyName = "buildinglist")]
        public List<BuildingDto> BuildingDtolist { get; set; }

        //[JsonProperty(PropertyName = "buildings")]
        //public List<BuildingDto> Buildings { get; set; }

        /// <summary>
        /// 关联公司
        /// </summary>
        [JsonProperty(PropertyName = "pcompanys")]
        public List<PCompanyDto> PCompanyDtoList { get; set; }
        /// <summary>
        /// 关联配套
        /// </summary>
        [JsonProperty(PropertyName = "pappendage")]
        public List<PAppendageDto> AppendageDtoList { get; set; }

    }
}
