using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxtDataAcquisition.Domain.DTO
{
    public class PAppendageDto
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "appendagecode")]
        public int AppendageCode { get; set; }

        [JsonProperty(PropertyName = "projectid")]
        public int ProjectId { get; set; }

        [JsonProperty(PropertyName = "area")]
        public decimal? Area { get; set; }

        [JsonProperty(PropertyName = "p_aname")]
        public string P_AName { get; set; }

        [JsonProperty(PropertyName = "isinner")]
        public bool IsInner { get; set; }

        [JsonProperty(PropertyName = "cityid")]
        public int? CityId { get; set; }

        [JsonProperty(PropertyName = "classcode")]
        public int? ClassCode { get; set; }

        [JsonProperty(PropertyName = "distance")]
        public int? Distance { get; set; }

        [JsonProperty(PropertyName = "address")]
        public string Address { get; set; }

        [JsonProperty(PropertyName = "x")]
        public decimal? X { get; set; }

        [JsonProperty(PropertyName = "y")]
        public decimal? Y { get; set; }

        [JsonProperty(PropertyName = "uid")]
        public string Uid { get; set; }
    }
}
