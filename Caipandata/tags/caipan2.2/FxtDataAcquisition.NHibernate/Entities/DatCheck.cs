using Newtonsoft.Json;
using System;

//Nhibernate Code Generation Template 1.0
//author:MythXin
//blog:www.cnblogs.com/MythXin
//Entity Code Generation Template
namespace FxtDataAcquisition.NHibernate.Entities
{
	/// <summary>
    /// Dat_Check(任务审核表,一个任务对应一条数据)
	/// </summary>
    public class DatCheck
    {

        [JsonProperty(PropertyName = "id")]
        /// <summary>
        /// 数据审核表
        /// </summary>
        public virtual long Id
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
        [JsonProperty(PropertyName = "allotid")]
        /// <summary>
        /// 任务ID
        /// </summary>
        public virtual long AllotId
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
        [JsonProperty(PropertyName = "checkusername1")]
        /// <summary>
        /// 自审人
        /// </summary>
        public virtual string CheckUserName1
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "checkstate1")]
        /// <summary>
        /// 自审结果,1:通过，0:不通过
        /// </summary>
        public virtual int? CheckState1
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "checkremark1")]
        /// <summary>
        /// 自审备注
        /// </summary>
        public virtual string CheckRemark1
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "checkdate1")]
        /// <summary>
        /// 自审时间
        /// </summary>
        public virtual DateTime? CheckDate1
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "checkusername2")]
        /// <summary>
        /// 审核人
        /// </summary>
        public virtual string CheckUserName2
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "checkstate2")]
        /// <summary>
        /// 审核结果,1:通过，0:不通过
        /// </summary>
        public virtual int? CheckState2
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "checkremark2")]
        /// <summary>
        /// 审核备注
        /// </summary>
        public virtual string CheckRemark2
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "checkdate2")]
        /// <summary>
        /// 审核时间
        /// </summary>
        public virtual DateTime? CheckDate2
        {
            get;
            set;
        }

    }
}