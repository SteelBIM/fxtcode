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
    [Table("Sys_Role")]
    public class Sys_Role : ModelBase   
    {
        public string Name { get; set; }
        public string Info { get; set; }

        //public virtual List<User> Users { get; set; }

        public string BusinessPermissionString { get; set; }

        //public List<EnumBusinessPermission> BusinessPermissionList
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(BusinessPermissionString))
        //            return new List<EnumBusinessPermission>();
        //        else
        //            return BusinessPermissionString.Split(",".ToCharArray()).Select(p => int.Parse(p)).Cast<EnumBusinessPermission>().ToList();
        //    }
        //    set
        //    {
        //        BusinessPermissionString = string.Join(",", value.Select(p => (int)p));
        //    }
        //}
    }
}
