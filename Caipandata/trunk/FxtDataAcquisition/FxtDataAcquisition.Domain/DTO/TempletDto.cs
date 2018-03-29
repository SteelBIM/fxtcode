using FxtDataAcquisition.Domain.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxtDataAcquisition.Domain.DTO
{
    public class TempletDto
    {
        [JsonProperty(PropertyName = "templetid")]
        public int TempletId { get; set; }

        [JsonProperty(PropertyName = "templetname")]
        public string TempletName { get; set; }

        [JsonProperty(PropertyName = "fieldgroups")]
        public List<FieldGroupDto> FieldGroups { get; set; }
    }
}
