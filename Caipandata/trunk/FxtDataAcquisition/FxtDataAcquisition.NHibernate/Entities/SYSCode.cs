using Newtonsoft.Json;
using System;

/**
 * 作者: 曾智磊
 * 时间: 2014.03.03
 * 摘要: 新建实体类
 * **/
namespace FxtDataAcquisition.NHibernate.Entities
{
    //SYS_Code
    public class SYSCode
    {
        [JsonProperty(PropertyName = "id")]
        /// <summary>
        /// ID
        /// </summary>
        public virtual int ID
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "code")]
        /// <summary>
        /// Code
        /// </summary>
        public virtual int Code
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "codename")]
        /// <summary>
        /// CodeName
        /// </summary>
        public virtual string CodeName
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "codetype")]
        /// <summary>
        /// CodeType
        /// </summary>
        public virtual string CodeType
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
        [JsonProperty(PropertyName = "subcode")]
        /// <summary>
        /// SubCode
        /// </summary>
        public virtual int? SubCode
        {
            get;
            set;
        }

    }
}