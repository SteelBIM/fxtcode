using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseActivate.Activate.Constract.Model
{
    /// <summary>
    /// 年级表
    /// </summary>
    public class base_grade
    {
        public int Gid { get; set; }

        /// <summary>
        /// 年级
        /// </summary>
        public string Gtitle { get; set; }

        /// <summary>
        /// 排序序号
        /// </summary>
        public int Sort { get; set; }
    }
}
