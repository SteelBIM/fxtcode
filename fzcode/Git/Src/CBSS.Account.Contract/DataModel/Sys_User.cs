using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CBSS.Framework.Contract;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CBSS.Account.Contract
{
    [Auditable]
    [Table("Sys_User")]
    public partial class Sys_User : ModelBase
    {
        public Sys_User()
        {
            //this.Roles = new List<Role>();
            //this.IsActive = true;
            //this.RoleIds = new List<int>();
        }

        /// <summary>
        /// 登录名
        /// </summary>
        [Required(ErrorMessage = "登录名不能为空")]
        public string LoginName { get; set; }

        /// <summary>
        /// 密码，使用MD5加密
        /// </summary>
        [Required(ErrorMessage = "密码不能为空")]
        public string Password { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [RegularExpression(@"^[1-9]{1}\d{10}$", ErrorMessage = "不是有效的手机号码")]
        public string Mobile { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [RegularExpression(@"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "电子邮件地址无效")]
        [StringLength(40,ErrorMessage = "电子邮件地址超过了最大长度")] 
        public string Email { get; set; }

        public bool IsActive { get; set; }

        ///// <summary>
        ///// 角色列表
        ///// </summary>
        //public virtual List<Role> Roles { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
       [Required(ErrorMessage = "请选择角色")]
        public int RoleId { get; set; }

       [SqlSugar.SugarColumn(IsIgnore=true)]
       public string NewPassword { get; set; }



        //[NotMapped]
        //public List<EnumBusinessPermission> BusinessPermissionList
        //{
        //    get
        //    {
        //        var permissions = new List<EnumBusinessPermission>();

        //        foreach (var role in Roles)
        //        {
        //            permissions.AddRange(role.BusinessPermissionList);
        //        }

        //        return permissions.Distinct().ToList();
        //    }
        //}
    }
}
