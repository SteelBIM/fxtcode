using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FxtDataAcquisition.Domain.Models;

namespace FxtDataAcquisition.Domain.DTO
{
    public class HouseDetailsDto
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "fxtcompanyid")]
        public int? FxtCompanyId { get; set; }

        /// <summary>
        /// 房号ID
        /// </summary>
        [JsonProperty(PropertyName = "houseid")]
        public int HouseId { get; set; }
        [JsonProperty(PropertyName = "buildingid")]
        public int BuildingId { get; set; }
        /// <summary>
        /// 房号名称
        /// </summary>
        [JsonProperty(PropertyName = "housename")]
        public string HouseName { get; set; }
        /// <summary>
        /// 户型
        /// </summary>
        [JsonProperty(PropertyName = "housetypecode")]
        public int? HouseTypeCode { get; set; }
        /// <summary>
        /// 楼层
        /// </summary>
        [JsonProperty(PropertyName = "floorno")]
        public int FloorNo { get; set; }
        /// <summary>
        /// 单元
        /// </summary>
        [JsonProperty(PropertyName = "unitno")]
        public string UnitNo { get; set; }
        /// <summary>
        /// 室号
        /// </summary>
        [JsonProperty(PropertyName = "roomno")]
        public string RoomNo { get; set; }
        /// <summary>
        /// 面积
        /// </summary>
        [JsonProperty(PropertyName = "buildarea")]
        public decimal? BuildArea { get; set; }
        /// <summary>
        /// 朝向
        /// </summary>
        [JsonProperty(PropertyName = "frontcode")]
        public int? FrontCode { get; set; }
        /// <summary>
        /// 景观
        /// </summary>
        [JsonProperty(PropertyName = "sightcode")]
        public int? SightCode { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [JsonProperty(PropertyName = "remark")]
        public string Remark { get; set; }
        /// <summary>
        /// 户型结构
        /// </summary>
        [JsonProperty(PropertyName = "structurecode")]
        public int? StructureCode { get; set; }
        /// <summary>
        /// 用途
        /// </summary>
        [JsonProperty(PropertyName = "purposecode")]
        public int? PurposeCode { get; set; }
        [JsonProperty(PropertyName = "cityid")]
        public int CityID { get; set; }
        [JsonProperty(PropertyName = "valid")]
        public int? Valid { get; set; }
        /// <summary>
        /// 通风采光
        /// </summary>
        [JsonProperty(PropertyName = "vdcode")]
        public int? VDCode { get; set; }
        /// <summary>
        /// 噪音情况
        /// </summary>
        [JsonProperty(PropertyName = "noisecode")]
        public int? NoiseCode { get; set; }
        /// <summary>
        /// 名义层（实际层）
        /// </summary>
        [JsonProperty(PropertyName = "nominalfloor")]
        public string NominalFloor { get; set; }

        /// <summary>
        /// 数据中心房号ID
        /// </summary>
        [JsonProperty(PropertyName = "fxthouseid")]
        public int? FxtHouseId { get; set; }
        /// <summary>
        /// 朝向
        /// </summary>
        [JsonProperty(PropertyName = "frontcodename")]
        public string FrontCodeName { get; set; }
        /// <summary>
        /// 户型
        /// </summary>
        [JsonProperty(PropertyName = "housetypecodename")]
        public string HouseTypeCodeName { get; set; }
        /// <summary>
        /// 景观
        /// </summary>
        [JsonProperty(PropertyName = "gightcodename")]
        public string SightCodeName { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        [JsonProperty(PropertyName = "unitprice")]
        public decimal? UnitPrice { get; set; }
        /// <summary>
        /// 价格系数
        /// </summary>
        [JsonProperty(PropertyName = "weight")]
        public decimal? Weight { get; set; }
        /// <summary>
        /// 噪音情况
        /// </summary>
        [JsonProperty(PropertyName = "noisecodename")]
        public string NoiseCodeName { get; set; }
        /// <summary>
        /// 用途
        /// </summary>
        [JsonProperty(PropertyName = "purposecodename")]
        public string PurposeCodeName { get; set; }
        /// <summary>
        /// 是否可估
        /// </summary>
        [JsonProperty(PropertyName = "isevalue")]
        public int? IsEValue { get; set; }
        /// <summary>
        /// 户型结构
        /// </summary>
        [JsonProperty(PropertyName = "structurecodename")]
        public string StructureCodeName { get; set; }
        /// <summary>
        /// 总价
        /// </summary>
        [JsonProperty(PropertyName = "totalprice")]
        public decimal? TotalPrice { get; set; }
        /// <summary>
        /// 通风采光
        /// </summary>
        [JsonProperty(PropertyName = "vdcodename")]
        public string VDCodeName { get; set; }
        /// <summary>
        /// 附属房屋类型
        /// </summary>
        [JsonProperty(PropertyName = "subhousetype")]
        public int SubHouseType { get; set; }
        /// <summary>
        /// 附属房屋类型
        /// </summary>
        [JsonProperty(PropertyName = "subhousetypename")]
        public string SubHouseTypeName { get; set; }
        /// <summary>
        /// 附属房屋面积
        /// </summary>
        [JsonProperty(PropertyName = "subhousearea")]
        public decimal? SubHouseArea { get; set; }
        /// <summary>
        /// 面积确认
        /// </summary>
        [JsonProperty(PropertyName = "isshowbuildingarea")]
        public short? IsShowBuildingArea { get; set; }
        /// <summary>
        /// 面积确认
        /// </summary>
        [JsonProperty(PropertyName = "isshowbuildingareaname")]
        public string IsShowBuildingAreaName { get; set; }
        /// <summary>
        /// 套内面积
        /// </summary>
        [JsonProperty(PropertyName = "innerbuildingarea")]
        public decimal? InnerBuildingArea { get; set; }
        /// <summary>
        /// 装修
        /// </summary>
        [JsonProperty(PropertyName = "fitmentcode")]
        public int? FitmentCode { get; set; }
        /// <summary>
        /// 装修
        /// </summary>
        [JsonProperty(PropertyName = "fitmentcodename")]
        public string FitmentCodeName { get; set; }
        /// <summary>
        /// 是否有厨房
        /// </summary>
        [JsonProperty(PropertyName = "cookroom")]
        public int? Cookroom { get; set; }
        /// <summary>
        /// 是否有厨房
        /// </summary>
        [JsonProperty(PropertyName = "cookroomnaem")]
        public string CookroomName { get; set; }
        /// <summary>
        /// 阳台数
        /// </summary>
        [JsonProperty(PropertyName = "balcony")]
        public int? Balcony { get; set; }
        /// <summary>
        /// 洗手间数
        /// </summary>
        [JsonProperty(PropertyName = "toilet")]
        public int? Toilet { get; set; }

        [JsonProperty(PropertyName = "saveuser")]
        public string SaveUser { get; set; }

        [JsonProperty(PropertyName = "creator")]
        public string Creator { get; set; }

        //[JsonProperty(PropertyName = "unitname")]
        ///// <summary>
        ///// 单元号
        ///// </summary>
        //public string UnitName { get; set; }
        //[JsonProperty(PropertyName = "houseno")]
        ///// <summary>
        ///// 室号
        ///// </summary>
        //public string HouseNo { get; set; }
    }
}
