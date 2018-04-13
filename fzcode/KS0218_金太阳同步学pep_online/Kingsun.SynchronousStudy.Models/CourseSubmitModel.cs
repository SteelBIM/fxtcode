using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kingsun.SynchronousStudy.Models
{
    public class CourseSubmitModel : TB_Course
    {
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath
        {
            get;
            set;
        }
        /// <summary>
        /// 文件MD5值
        /// </summary>
        public string FileMD5
        {
            get;
            set;
        }
    }
}
