using Newtonsoft.Json;
using System;

//Nhibernate Code Generation Template 1.0
//author:MythXin
//blog:www.cnblogs.com/MythXin
//Entity Code Generation Template
namespace FxtDataAcquisition.NHibernate.Entities
{
    //LNK_P_Appendage
    public class LNKPAppendage
    {

        [JsonProperty(PropertyName = "id")]
        /// <summary>
        /// id
        /// </summary>
        public virtual int id
        {
            get;
            set;
        }

        /// <summary>
        /// 唯一标示
        /// </summary>
        [JsonProperty(PropertyName = "uid")]
        public virtual string Uid
        {
            get;
            set;
        }
        

        [JsonProperty(PropertyName = "appendagecode")]
        /// <summary>
        /// AppendageCode
        /// </summary>
        public virtual int AppendageCode
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
        [JsonProperty(PropertyName = "area")]
        /// <summary>
        /// Area
        /// </summary>
        public virtual decimal? Area
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "p_aname")]
        /// <summary>
        /// P_AName
        /// </summary>
        public virtual string P_AName
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "isinner")]
        /// <summary>
        /// IsInner
        /// </summary>
        public virtual bool IsInner
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "cityid")]
        /// <summary>
        /// CityId
        /// </summary>
        public virtual int? CityId
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "classcode")]
        /// <summary>
        /// 配套等级
        /// </summary>
        public virtual int? ClassCode
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "distance")]
        /// <summary>
        /// 距离
        /// </summary>
        public virtual int? Distance
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "address")]
        /// <summary>
        /// 地址
        /// </summary>
        public virtual string Address
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "x")]
        public virtual decimal x
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "y")]
        public virtual decimal y
        {
            get;
            set;
        }
    }
}