using FxtDataAcquisition.Domain.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxtDataAcquisition.Domain.DTO
{
    public class FieldGroupDto
    {
        [JsonProperty(PropertyName = "fieldgroupname")]
        public string FieldGroupName { get; set; }

        [JsonProperty(PropertyName = "sort")]
        public int Sort { get; set; }

        [JsonProperty(PropertyName = "fields")]
        public List<FieldDto> Fields { get; set; }

    }
}
