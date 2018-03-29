using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxtDataAcquisition.Domain.DTO
{
    public class MenuDto
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "parentid")]
        public int ParentID { get; set; }

        [JsonProperty(PropertyName = "menuname")]
        public string MenuName { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string URL { get; set; }

        [JsonProperty(PropertyName = "iconclass")]
        public string IconClass { get; set; }

        [JsonProperty(PropertyName = "totalcount")]
        public int TotalCount { get; set; }

        [JsonProperty(PropertyName = "classid")]
        public string ClassId { get; set; }
    }
}
