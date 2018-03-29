using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace CAS.Entity
{
    public class FormData
    {
        //字段
        public string cfield { get; set; }
        //字段的值
        public string value { get; set; }
    }
    public class Field : FormData
    {
        //字段类型
        public SqlDbType type { get; set; }
        //操作符
        public string operatoin { get; set; }
        //别名
        public string alias { get; set; }
        //参数名
        public string parame { get; set; }
        //查询字符串
        public string query { get; set; }
    }
}
