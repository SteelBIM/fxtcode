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
        /// 户型结构
        /// </summary>
        [JsonProperty(PropertyName = "structurecodename")]
        public string StructureCodeName { get; set; }
        /// <summary>
        /// 通风采光
        /// </summary>
        [JsonProperty(PropertyName = "vdcodename")]
        public string VDCodeName { get; set; }
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
