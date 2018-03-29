using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.Domain.DTO.FxtUserCenterDTO
{
    public class UserCenter_UserInfo
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [JsonProperty(PropertyName = "username")]
        public virtual string UserName
        {
            get;
            set;
        }
        /// <summary>
        /// 姓名
        /// </summary>
        [JsonProperty(PropertyName = "truename")]
        public virtual string TrueName
        {
            get;
            set;
        }
        /// <summary>
        /// 电话
        /// </summary>
        [JsonProperty(PropertyName = "mobile")]
        public virtual string Mobile
        {
            get;
            set;
        }
        /// <summary>
        /// 机构名
        /// </summary>
        [JsonProperty(PropertyName = "companyname")]
        public virtual string CompanyName
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "uservalid")]
        public virtual int UserValid
        {
            get;
            set;
        }
        /// <summary>
        /// 总个数
        /// </summary>
        [JsonProperty(PropertyName = "recordcount")]
        public virtual int RecordCount
        {
            get;
            set;
        }
    }
}
