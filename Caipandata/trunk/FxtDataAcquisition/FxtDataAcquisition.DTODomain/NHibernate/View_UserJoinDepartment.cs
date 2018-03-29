using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.DTODomain.NHibernate
{
    public class View_UserJoinDepartment
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
        /// 小组ID
        /// </summary>
        [JsonProperty(PropertyName = "departmentid")]
        public virtual int DepartmentID
        {
            get;
            set;
        }
        /// <summary>
        /// 城市
        /// </summary>
        [JsonProperty(PropertyName = "cityid")]
        public virtual int CityID
        {
            get;
            set;
        }
        /// <summary>
        /// 公司
        /// </summary>
        [JsonProperty(PropertyName = "fxtcompanyid")]
        public virtual int FxtCompanyID
        {
            get;
            set;
        }
        /// <summary>
        /// 小组名
        /// </summary>
        [JsonProperty(PropertyName = "departmentname")]
        public virtual string DepartmentName
        {
            get;
            set;
        }
        /// <summary>
        /// 小组是否可用(标记删除)
        /// </summary>
        [JsonProperty(PropertyName = "dvalid")]
        public virtual int DValid
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "__hibernate_sort_row")]
        /// <summary>
        /// 排序
        /// </summary>
        public virtual long __hibernate_sort_row
        {
            get;
            set;
        }
    }
}
