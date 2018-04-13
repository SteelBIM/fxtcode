using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.Contract.ViewModel
{
    public class UserInfoModel
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long? UserID { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string TrueName { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string TelePhone { get; set; }
        /// <summary>
        /// 用户角色
        /// </summary>
        public int? UserType { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime? Regdate { get; set; }
        /// <summary>
        /// 省
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 市
        /// </summary>

        public string City { get; set; }
        /// <summary>
        /// 区
        /// </summary>
        public string Area { get; set; }
        /// <summary>
        /// 学校名称
        /// </summary>
        public string SchoolName { get; set; }

        /// <summary>
        /// 班级
        /// </summary>
        public string ClassName { get; set; }
        /// <summary>
        /// 注册设备
        /// </summary>
        public string DeviceType { get; set; }
        /// <summary>
        /// 应用名称
        /// </summary>
        public string AppName { get; set; }
    }
}
