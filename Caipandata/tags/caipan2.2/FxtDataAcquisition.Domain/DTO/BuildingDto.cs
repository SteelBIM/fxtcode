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

        [JsonProperty(PropertyName = "buildingname")]
        public string BuildingName { get; set; }

        [JsonProperty(PropertyName = "projectid")]
        public int ProjectId { get; set; }

        [JsonProperty(PropertyName = "purposecode")]
        public int? PurposeCode { get; set; }

        [JsonProperty(PropertyName = "structurecode")]
        public int? StructureCode { get; set; }

        [JsonProperty(PropertyName = "totalfloor")]
        public int? TotalFloor { get; set; }

        [JsonProperty(PropertyName = "elevatorrate")]
        public string ElevatorRate { get; set; }

        [JsonProperty(PropertyName = "builddate")]
        public DateTime? BuildDate { get; set; }

        [JsonProperty(PropertyName = "othername")]
        public string OtherName { get; set; }

        [JsonProperty(PropertyName = "cityid")]
        public int CityID { get; set; }

        [JsonProperty(PropertyName = "locationcode")]
        public int? LocationCode { get; set; }

        [JsonProperty(PropertyName = "fxtcompanyid")]
        public int? FxtCompanyId { get; set; }

        [JsonProperty(PropertyName = "iselevator")]
        public int? IsElevator { get; set; }

        [JsonProperty(PropertyName = "remark")]
        public string Remark { get; set; }

        [JsonProperty(PropertyName = "fxtbuildingid")]
        public int? FxtBuildingId { get; set; }

        [JsonProperty(PropertyName = "maintenancecode")]
        public int? MaintenanceCode { get; set; }

        [JsonProperty(PropertyName = "x")]
        public decimal? X { get; set; }

        [JsonProperty(PropertyName = "y")]
        public decimal? Y { get; set; }
        [JsonProperty(PropertyName = "valid")]
        public int Valid { get; set; }
        /// <summary>
        /// 楼栋图片数量
        /// </summary>
        [JsonProperty(PropertyName = "buildimagecount")]
        public int BuildImageCount { get; set; }
        /// <summary>
        /// 单元数量
        /// </summary>
        [JsonProperty(PropertyName = "unitsnumber")]
        public int? UnitsNumber { get; set; }

        [JsonProperty(PropertyName = "houselist")]
        public virtual List<HouseDto> HouseDtolist { get; set; }

    }
}
