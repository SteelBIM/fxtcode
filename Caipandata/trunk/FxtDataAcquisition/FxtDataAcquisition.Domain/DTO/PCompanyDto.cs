namespace FxtDataAcquisition.Domain.DTO
{
    using System;
    using Newtonsoft.Json;

    /// <summary>
    /// 公司
    /// </summary>
    public class PCompanyDto
    {
        [JsonProperty(PropertyName = "projectid")]
        public int ProjectId { get; set; }

        [JsonProperty(PropertyName = "companyname")]
        public string CompanyName { get; set; }

        [JsonProperty(PropertyName = "companytype")]
        public int CompanyType { get; set; }

        [JsonProperty(PropertyName = "cityid")]
        public int CityId { get; set; }
    }
}
