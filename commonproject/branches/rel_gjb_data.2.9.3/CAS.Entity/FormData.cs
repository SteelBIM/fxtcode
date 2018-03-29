using System.Data;

namespace CAS.Entity
{
    public class FormData
    {
        /// <summary>
        /// 要进行比较的字段（当operatoin为比较操作符例如大于号时用到）
        /// </summary>
        public string cfield { get; set; }
        /// <summary>
        /// "0"：当比较值为0时则忽略此条件，"can 0":当比较值为0时则忽略此条件(用在operatoin=custom时)，"can null":当比较值为空或者空字符串时则忽略此条件(用在operatoin=custom时)
        /// </summary>
        public string value { get; set; }
    }
    public class Field : FormData
    {
        /// <summary>
        /// 字段类型
        /// </summary>
        public SqlDbType type { get; set; }
        /// <summary>
        /// 操作符，如大于号、小于号等，另外要自定义查询字符串时则为custom，custom_1(业务逻辑的特殊处理)，custom_2(自定义查询字符串中带有模糊查询时)，custom_3(业务逻辑的特殊处理)
        /// </summary>
        public string operatoin { get; set; }
        /// <summary>
        /// 别名，给当前查询条件定义一个标识，必填
        /// </summary>
        public string alias { get; set; }
        /// <summary>
        /// 参数名，需要给比较值定义一个参数名称，如果传空则代表比较值是以字符串拼接方式到sql中(在operatoin=custom时会用到)
        /// </summary>
        public string parame { get; set; }
        /// <summary>
        /// 要自定义的查询字符串,假如没有传参即parame=''时, 自定义查询不需要加"and等连接符,代码处理会自动加上"
        /// </summary>
        public string query { get; set; }
        /// <summary>
        /// 1:只插入参数，0：插入sql和参数
        /// </summary>
        public int insertype { get; set; }
    }
}
