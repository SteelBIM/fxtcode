using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxtDataAcquisition.Domain.DTO
{
    /// <summary>
    /// 查勘统计
    /// </summary>
    public class SurveyCountDto
    {
        /// <summary>
        /// 查勘人账号
        /// </summary>
        [JsonProperty(PropertyName = "surveyusername")]
        public string SurveyUserName { get; set; }
        /// <summary>
        /// 查勘人姓名
        /// </summary>
        [JsonProperty(PropertyName = "surveyusertruename")]
        public string SurveyUserTrueName { get; set; }
        /// <summary>
        /// 待查勘数量
        /// </summary>
        [JsonProperty(PropertyName = "tosurveycount")]
        public int ToSurveyCount { get; set; }
        /// <summary>
        /// 查勘中数量
        /// </summary>
        [JsonProperty(PropertyName = "inthesurveycount")]
        public int InTheSurveyCount { get; set; }
        /// <summary>
        /// 已查勘数量
        /// </summary>
        [JsonProperty(PropertyName = "havesurveycount")]
        public int HaveSurveyCount { get; set; }
        /// <summary>
        /// 待审批数量
        /// </summary>
        [JsonProperty(PropertyName = "pendingapprovalcount")]
        public int PendingApprovalCount { get; set; }
        /// <summary>
        /// 审批已通过数量
        /// </summary>
        [JsonProperty(PropertyName = "passedapprovalcount")]
        public int PassedApprovalCount { get; set; }
        /// <summary>
        /// 已入库数量
        /// </summary>
        [JsonProperty(PropertyName = "alreadystoragecount")]
        public int AlreadyStorageCount { get; set; }
        /// <summary>
        /// 已入库楼栋数量
        /// </summary>
        [JsonProperty(PropertyName = "alreadystoragebuildingcount")]
        public int AlreadyStorageBuildingCount { get; set; }
    }
}
