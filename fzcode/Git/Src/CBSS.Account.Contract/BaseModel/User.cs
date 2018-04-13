using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CBSS.Framework.Contract;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SqlSugar;

namespace CBSS.Account.Contract
{
    [Auditable]
    [Table("User")]
    public partial class User : ModelBase
    {
        public User()
        {
            this.Roles = new List<Role>();
            this.IsActive = true;
            this.RoleIds = new List<int>();
        }

        /// <summary>
        /// 登录名
        /// </summary>
        [Required(ErrorMessage = "登录名不能为空")]
        public string LoginName { get; set; }

        /// <summary>
        /// 密码，使用MD5加密
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [RegularExpression(@"^[1-9]{1}\d{10}$", ErrorMessage = "不是有效的手机号码")]
        public string Mobile { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "电子邮件地址无效")]
        public string Email { get; set; }

        public bool IsActive { get; set; }

        /// <summary>
        /// 角色列表
        /// </summary>
       [SugarColumn(IsIdentity = true, IsIgnore = true)]
        public virtual List<Role> Roles { get; set; }

        [NotMapped]
        [SugarColumn(IsIdentity = true, IsIgnore = true)]
        public List<int> RoleIds { get; set; }

        [NotMapped]
        [SugarColumn( IsIdentity = true, IsIgnore = true)]
        public string NewPassword { get; set; }



        [NotMapped]
        [SugarColumn(IsIdentity = true, ColumnName = null, IsIgnore = true)]
        public List<EnumBusinessPermission> BusinessPermissionList
        {
            get
            {
                var permissions = new List<EnumBusinessPermission>();

                foreach (var role in Roles)
                {
                    permissions.AddRange(role.BusinessPermissionList);
                }

                return permissions.Distinct().ToList();
            }
        }
    }
}
