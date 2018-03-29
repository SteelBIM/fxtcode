namespace FxtDataAcquisition.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SYS_Menu
    {
        public int Id { get; set; }

        public int ParentID { get; set; }

        public string MenuName { get; set; }

        public int Valid { get; set; }

        public string Remark { get; set; }

        public string URL { get; set; }

        public int TypeCode { get; set; }

        public int? ModuleCode { get; set; }

        public string IconClass { get; set; }

        public virtual ICollection<SYS_Role_Menu> RoleMenus { get; set; }

    }
}
