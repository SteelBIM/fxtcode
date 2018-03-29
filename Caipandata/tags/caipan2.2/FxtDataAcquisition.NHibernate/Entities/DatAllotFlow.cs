using Newtonsoft.Json;
using System;

//Nhibernate Code Generation Template 1.0
//author:MythXin
//blog:www.cnblogs.com/MythXin
//Entity Code Generation Template
namespace FxtDataAcquisition.NHibernate.Entities
{
    /// <summary>
    /// Dat_AllotFlow(任务表)
    /// </summary>
    public class DatAllotFlow
    {

        [JsonProperty(PropertyName = "id")]
        /// <summary>
        /// 查勘分配、调查任务表
        /// </summary>
        public virtual long id
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "cityid")]
        /// <summary>
        /// CityId
        /// </summary>
        public virtual int CityId
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "fxtcompanyid")]
        /// <summary>
        /// FxtCompanyId
        /// </summary>
        public virtual int FxtCompanyId
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "dattype")]
        /// <summary>
        /// 数据类型：楼栋、楼层、房号
        /// </summary>
        public virtual int DatType
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "datid")]
        /// <summary>
        /// 数据ID
        /// </summary>
        public virtual long DatId
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "otherid")]
        /// <summary>
        /// OtherId
        /// </summary>
        public virtual string OtherId
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "statecode")]
        /// <summary>
        /// 数据状态
        /// </summary>
        public virtual int StateCode
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "statedate")]
        /// <summary>
        /// 状态更新时间
        /// </summary>
        public virtual DateTime? StateDate
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "createtime")]
        /// <summary>
        /// 时间
        /// </summary>
        public virtual DateTime CreateTime
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "username")]
        /// <summary>
        /// 分配人ID
        /// </summary>
        public virtual string UserName
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "usertruename")]
        /// <summary>
        /// 分配人姓名
        /// </summary>
        public virtual string UserTrueName
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "surveyusername")]
        /// <summary>
        /// 查勘员ID
        /// </summary>
        public virtual string SurveyUserName
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "surveyusertruename")]
        /// <summary>
        /// 查勘员姓名
        /// </summary>
        public virtual string SurveyUserTrueName
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "remark")]
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "x")]
        /// <summary>
        /// 查勘员坐标X
        /// </summary>
        public virtual decimal? X
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "y")]
        /// <summary>
        ///查勘员坐标Y
        /// </summary>
        public virtual decimal? Y
        {
            get;
            set;
        }

    }
}