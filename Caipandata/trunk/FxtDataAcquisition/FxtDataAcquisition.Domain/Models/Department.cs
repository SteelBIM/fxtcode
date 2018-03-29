namespace FxtDataAcquisition.Domain.Models
{
    using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

    public partial class Department
    {
        public int DepartmentId { get; set; }

        public int Fk_CompanyId { get; set; }

        public string DepartmentName { get; set; }

        public int FK_CityId { get; set; }

        public int FK_DepTypeCode { get; set; }

        public int? FK_ParentId { get; set; }

        public string Address { get; set; }

        public string Fax { get; set; }

        public string Telephone { get; set; }

        public string EMail { get; set; }

        public string LinkMan { get; set; }

        public int? FxtCompanyId { get; set; }

        public int? DValid { get; set; }

        public int? FK_DepAttr { get; set; }

        public virtual ICollection<DepartmentUser> DepartmentUsers { get; set; }
    }
}
