using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxtDataAcquisition.Domain.DTO
{
    public class HouseApiDto
    {
        [JsonProperty(PropertyName = "id")]
        public Guid? AppId { get; set; }

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
        /// 模板
        /// </summary>
        [JsonProperty(PropertyName = "templet")]
        public TempletDto Templet { get; set; }
    }
}
