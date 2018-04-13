using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseActivate.Account.Constract.Models
{
    /// <summary>
    /// 配置 字典型数据
    /// </summary>
    public class cfg_keyvalue
    {
        public int ID { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        //public string ParentKey { get; set; }

        public string UseType { get; set; }
    }
}
