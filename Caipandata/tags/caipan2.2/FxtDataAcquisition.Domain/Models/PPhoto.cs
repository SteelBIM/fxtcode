namespace FxtDataAcquisition.Domain.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PPhoto
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "projectid")]
        public int ProjectId { get; set; }

        [JsonProperty(PropertyName = "phototypecode")]
        public int? PhotoTypeCode { get; set; }

        [JsonProperty(PropertyName = "path")]
        public string Path { get; set; }

        [JsonProperty(PropertyName = "photodate")]
        public DateTime? PhotoDate { get; set; }

        [JsonProperty(PropertyName = "photoname")]
        public string PhotoName { get; set; }

        [JsonProperty(PropertyName = "cityid")]
        public int CityId { get; set; }

        [JsonProperty(PropertyName = "valid")]
        public int? Valid { get; set; }

        [JsonProperty(PropertyName = "fxtcompanyid")]
        public int FxtCompanyId { get; set; }

        [JsonProperty(PropertyName = "buildingid")]
        public long? BuildingId { get; set; }

        [JsonProperty(PropertyName = "x")]
        public decimal? X { get; set; }

        [JsonProperty(PropertyName = "y")]
        public decimal? Y { get; set; }
    }
}
