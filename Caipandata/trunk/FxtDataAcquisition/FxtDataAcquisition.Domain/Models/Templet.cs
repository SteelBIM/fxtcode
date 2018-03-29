using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxtDataAcquisition.Domain.Models
{
    /// <summary>
    /// 模板
    /// </summary>
    public class Templet
    {
        public int TempletId { get; set; }
        public string TempletName { get; set; }
        /// <summary>
        /// 模板类型（1034001.楼盘，1034002.楼栋，1034004.单元室号）
        /// </summary>
        public int DatType { get; set; }
        public int FxtCompanyId { get; set; }
        public string AddUser { get; set; }
        public DateTime AddTime { get; set; }
        public string SaveUser { get; set; }
        public DateTime? SaveTime { get; set; }
        public string DelUser { get; set; }
        public DateTime? DelTime { get; set; }
        public int Vaild { get; set; }
        public bool IsCurrent { get; set; }
        public virtual ICollection<FieldGroup> FieldGroups { get; set; }
        //public virtual ICollection<Project> Projects { get; set; }

    }
}
