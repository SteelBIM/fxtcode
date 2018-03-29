using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxtDataAcquisition.Domain.DTO
{
    public class UserInfoDto
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
        /// 多个角色名称(由逗号分隔)
        /// </summary>
        [JsonProperty(PropertyName = "rolenames")]
        public virtual string RoleNames
        {
            get;
            set;
        }
        /// <summary>
        /// DepartmentId
        /// </summary>
        [JsonProperty(PropertyName = "departmentid")]
        public virtual int? DepartmentId
        {
            get;
            set;
        }
        /// <summary>
        /// DepartmentName
        /// </summary>
        [JsonProperty(PropertyName = "departmentname")]
        public virtual string DepartmentName
        {
            get;
            set;
        }
    }
}
