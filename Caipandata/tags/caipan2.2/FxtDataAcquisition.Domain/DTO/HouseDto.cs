using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.Domain.DTO
{
    public class HouseDto
    {
        [JsonProperty(PropertyName = "id")]
        public Guid? AppId { get; set; }

        [JsonProperty(PropertyName = "houseid")]
        public int HouseId { get; set; }

        [JsonProperty(PropertyName = "buildingid")]
        public int BuildingId { get; set; }

        [JsonProperty(PropertyName = "housename")]
        public string HouseName { get; set; }

        [JsonProperty(PropertyName = "housetypecode")]
        public int? HouseTypeCode { get; set; }

        [JsonProperty(PropertyName = "unitno")]
        public string UnitNo { get; set; }

        [JsonProperty(PropertyName = "houseno")]
        public string HouseNo { get; set; }

        [JsonProperty(PropertyName = "floorno")]
        public int FloorNo { get; set; }
        [JsonProperty(PropertyName = "endfloorno")]
        public int? EndFloorNo { get; set; }

        [JsonProperty(PropertyName = "buildarea")]
        public decimal? BuildArea { get; set; }

        [JsonProperty(PropertyName = "frontcode")]
        public int? FrontCode { get; set; }

        [JsonProperty(PropertyName = "sightcode")]
        public int? SightCode { get; set; }

        [JsonProperty(PropertyName = "remark")]
        public string Remark { get; set; }

        [JsonProperty(PropertyName = "structurecode")]
        public int? StructureCode { get; set; }

        [JsonProperty(PropertyName = "purposecode")]
        public int? PurposeCode { get; set; }

        [JsonProperty(PropertyName = "cityid")]
        public int CityID { get; set; }
        [JsonProperty(PropertyName = "fxtcompanyid")]
        public int? FxtCompanyId { get; set; }

        [JsonProperty(PropertyName = "fxthouseid")]
        public int? FxtHouseId { get; set; }

        [JsonProperty(PropertyName = "vdcode")]
        public int? VDCode { get; set; }

        [JsonProperty(PropertyName = "noisecode")]
        public int? NoiseCode { get; set; }
        [JsonProperty(PropertyName = "valid")]
        public int Valid { get; set; }
    }
}
