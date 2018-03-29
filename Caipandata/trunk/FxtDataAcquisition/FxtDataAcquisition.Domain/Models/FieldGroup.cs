using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxtDataAcquisition.Domain.Models
{
    /// <summary>
    /// 分组
    /// </summary>
    public class FieldGroup
    {
        public int FieldGroupId { get; set; }
        public int TempletId { get; set; }
        public string FieldGroupName { get; set; }
        public int Sort { get; set; }
        public string AddUser { get; set; }
        public DateTime AddTime { get; set; }
        public string SaveUser { get; set; }
        public DateTime? SaveTime { get; set; }
        public string DelUser { get; set; }
        public DateTime? DelTime { get; set; }
        public int Vaild { get; set; }
        public virtual Templet Templet { get; set; }
        public virtual ICollection<Field> Fields { get; set; }

    }
}
