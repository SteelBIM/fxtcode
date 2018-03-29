using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.DTODomain.FxtUserCenterDTO
{
    public class UserCenter_LoginUserInfo
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
        /// 企业标识码
        /// </summary>
        [JsonProperty(PropertyName = "signname")]
        public virtual string SignName
        {
            get;
            set;
        }
        /// <summary>
        /// 密码
        /// </summary>
        [JsonProperty(PropertyName = "password")]
        public virtual string Password
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
        /// 机构名
        /// </summary>
        [JsonProperty(PropertyName = "companyname")]
        public virtual string CompanyName
        {
            get;
            set;
        }
        /// <summary>
        /// 所属机构ID
        /// </summary>
        [JsonProperty(PropertyName = "fxtcompanyid")]
        public virtual int FxtCompanyId
        {
            get;
            set;
        }
    }
}
