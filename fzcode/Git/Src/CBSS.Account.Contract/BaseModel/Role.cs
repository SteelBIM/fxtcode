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
    [Table("Role")]
    public class Role : ModelBase   
    {
        [Required(ErrorMessage = "角色名不能为空")]
        public string Name { get; set; }
        public string Info { get; set; }


        [NotMapped]
        [SugarColumn(IsIdentity = true, ColumnName = null, IsIgnore = true)]
        public virtual List<User> Users { get; set; }


        public string BusinessPermissionString { get; set; }


        [NotMapped]
        [SugarColumn(IsIdentity = true, ColumnName = null, IsIgnore = true)]
        public List<EnumBusinessPermission> BusinessPermissionList
        {
            get
            {
                if (string.IsNullOrEmpty(BusinessPermissionString))
                    return new List<EnumBusinessPermission>();
                else
                    return BusinessPermissionString.Split(",".ToCharArray()).Select(p => int.Parse(p)).Cast<EnumBusinessPermission>().ToList();
            }
            set
            {
                BusinessPermissionString = string.Join(",", value.Select(p => (int)p));
            }
        }
    }
}
