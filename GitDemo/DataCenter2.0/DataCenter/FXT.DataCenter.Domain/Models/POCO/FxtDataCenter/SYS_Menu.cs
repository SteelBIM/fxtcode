using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class SYS_Menu
    {
        public int ID { get; set; }
        public int ParentID { get; set; }
        public string MenuName { get; set; }
        public int Valid { get; set; }
        public string Remark { get; set; }
        public string URL { get; set; }
        public int TypeCode { get; set; }
        public int ModuleCode { get; set; }

        #region 用来标记是否被选中
        public bool Selected { get; set; }
        #endregion
    }
}
