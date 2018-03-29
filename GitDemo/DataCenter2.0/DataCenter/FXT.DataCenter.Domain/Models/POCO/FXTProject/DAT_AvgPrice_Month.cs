using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class DAT_AvgPrice_Month
    {

        public int Id { get; set; }

        public int CityId { get; set; }

        public int AreaId { get; set; }

        public int SubAreaId { get; set; }

        public int ProjectId { get; set; }
       
        public DateTime AvgPriceDate { get; set; }

        [DisplayName("均价")]
        [Required(ErrorMessage = "均价不能为空")]
        public int AvgPrice {get;set; }

        public int BuildingAreaType { get; set; }

        #region 扩展字段

        public int FxtCompanyId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        [DisplayName("日期")]
        [Required(ErrorMessage = "日期不能为空")]
        public string CaseDate { get; set; }

        #endregion
    }
}
