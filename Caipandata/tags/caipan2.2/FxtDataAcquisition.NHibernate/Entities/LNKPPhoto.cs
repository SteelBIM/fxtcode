using Newtonsoft.Json;
using System;

//Nhibernate Code Generation Template 1.0
//author:MythXin
//blog:www.cnblogs.com/MythXin
//Entity Code Generation Template
namespace FxtDataAcquisition.NHibernate.Entities
{
	 	//LNK_P_Photo
    public class LNKPPhoto
    {

        [JsonProperty(PropertyName = "id")]
        /// <summary>
        /// Id
        /// </summary>
        public virtual int Id
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "projectid")]
        /// <summary>
        /// ProjectId
        /// </summary>
        public virtual int ProjectId
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "phototypecode")]
        /// <summary>
        /// PhotoTypeCode
        /// </summary>
        public virtual int? PhotoTypeCode
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "path")]
        /// <summary>
        /// Path
        /// </summary>
        public virtual string Path
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "photodate")]
        /// <summary>
        /// PhotoDate
        /// </summary>
        public virtual DateTime? PhotoDate
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "photoname")]
        /// <summary>
        /// PhotoName
        /// </summary>
        public virtual string PhotoName
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
        [JsonProperty(PropertyName = "valid")]
        /// <summary>
        /// Valid
        /// </summary>
        public virtual int? Valid
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
        [JsonProperty(PropertyName = "buildingid")]
        /// <summary>
        /// BuildingId
        /// </summary>
        public virtual long? BuildingId
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "x")]
        /// <summary>
        /// X
        /// </summary>
        public virtual decimal? X
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "y")]
        /// <summary>
        /// Y
        /// </summary>
        public virtual decimal? Y
        {
            get;
            set;
        }

    }
}