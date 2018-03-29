using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.Domain.DTO.FxtUserCenterDTO
{
    public class UserCenter_Apps
    {
        /// <summary>
        /// api的CODE
        /// </summary>
        [JsonProperty(PropertyName = "appid")]
        public virtual int AppId
        {
            get;
            set;
        }
        /// <summary>
        /// api密码
        /// </summary>
        [JsonProperty(PropertyName = "apppwd")]
        public virtual string AppPwd
        {
            get;
            set;
        }
        /// <summary>
        /// api链接
        /// </summary>
        [JsonProperty(PropertyName = "appurl")]
        public virtual string AppUrl
        {
            get;
            set;
        }
        /// <summary>
        /// api的Key
        /// </summary>
        [JsonProperty(PropertyName = "appkey")]
        public virtual string AppKey
        {
            get;
            set;
        }
    }
}
