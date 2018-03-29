namespace FxtDataAcquisition.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SYS_Role_Menu_Function
    {
        public int Id { get; set; }

        public int RoleMenuID { get; set; }

        public int FunctionCode { get; set; }

        public int Valid { get; set; }

        public int CityID { get; set; }

        public int FxtCompanyID { get; set; }

        public virtual SYS_Role_Menu RoleMenu { get; set; }
    }
}
