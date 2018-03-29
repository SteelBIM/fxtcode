using Newtonsoft.Json;
using System;

//Nhibernate Code Generation Template 1.0
//author:MythXin
//blog:www.cnblogs.com/MythXin
//Entity Code Generation Template
namespace FxtDataAcquisition.NHibernate.Entities
{
    /// <summary>
    /// Dat_AllotSurvey(任务查勘日志记录表,一个任务ID对应多条)
    /// </summary>
    public class DatAllotSurvey
    {

        [JsonProperty(PropertyName = "id")]
        /// <summary>
        /// 查勘记录表
        /// </summary>
        public virtual long Id
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "allotid")]
        /// <summary>
        /// 查勘任务ID
        /// </summary>
        public virtual long AllotId
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
        [JsonProperty(PropertyName = "username")]
        /// <summary>
        /// 操作人
        /// </summary>
        public virtual string UserName
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "truename")]
        /// <summary>
        /// 操作人姓名
        /// </summary>
        public virtual string TrueName
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "createdate")]
        /// <summary>
        /// 记录时间
        /// </summary>
        public virtual DateTime? CreateDate
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "statecode")]
        /// <summary>
        /// 状态
        /// </summary>
        public virtual int? StateCode
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
        [JsonProperty(PropertyName = "remark")]
        /// <summary>
        /// Remark
        /// </summary>
        public virtual string Remark
        {
            get;
            set;
        }

    }
}