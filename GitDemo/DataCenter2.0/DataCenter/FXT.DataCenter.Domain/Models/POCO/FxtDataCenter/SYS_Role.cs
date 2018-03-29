using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace FXT.DataCenter.Domain.Models
{
    public class SYS_Role
    {

        public SYS_Role()
        {
            this.CityID = 6;
            this.FxtCompanyID = 25;
        }

        public int ID { get; set; }

        [Required(ErrorMessage = "角色名不能为空")]
        public string RoleName { get; set; }

        public string Remarks { get; set; }

        public int Valid { get; set; }

        public DateTime CreateTime { get; set; }

        public int CityID { get; set; }

        public string CityName { get; set; }

        public int FxtCompanyID { get; set; }
        
        //public string ValidDisplay
        //{
        //    get
        //    {
        //        return Valid == 1 ? "有效" : "无效";
        //    }
        //}

        //public string CreateTimeDisplay
        //{
        //    get
        //    {
        //        return CreateTime == null ? "" : CreateTime.ToShortDateString();
        //    }
        //}
    }
}
