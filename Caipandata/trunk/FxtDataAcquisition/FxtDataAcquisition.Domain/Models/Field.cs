using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxtDataAcquisition.Domain.Models
{
    /// <summary>
    /// 字段
    /// </summary>
    public class Field
    {
        public int FieldId { get; set; }
        public int FieldGroupId { get; set; }
        public string FieldName { get; set; }
        /// <summary>
        /// 字段类型（1.文本框，2.多行文本，3.下拉框，5.多选框，6.日期选择框）
        /// </summary>
        public int FieldType { get; set; }
        /// <summary>
        /// 字段类型（E.文本框，T.多行文本，R.单选框，C.多选框，DT.日期选择框）
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultValue { get; set; }
        /// <summary>
        /// 字段最大长度
        /// </summary>
        public int? MaxLength { get; set; }
        /// <summary>
        /// 字段最小长度
        /// </summary>
        public int? MinLength { get; set; }
        /// <summary>
        /// 是否必填(0.否，1.是)
        /// </summary>
        public int? IsRequired { get; set; }
        /// <summary>
        /// 是否可空(0.否，1.是)
        /// </summary>
        public int? IsNull { get; set; }
        /// <summary>
        /// 字段值类型（0.string,1.int,2.decimal,3.datetime）
        /// </summary>
        public int? EdiTextType { get; set; }
        /// <summary>
        /// 是否必选(0.否，1.是)
        /// </summary>
        public int? IsSelect { get; set; }
        /// <summary>
        /// 选项值
        /// </summary>
        //public string Choise { get; set; }
        //public string Value { get; set; }
        public string AddUser { get; set; }
        public DateTime AddTime { get; set; }
        public string SaveUser { get; set; }
        public DateTime? SaveTime { get; set; }
        public string DelUser { get; set; }
        public DateTime? DelTime { get; set; }
        public int Vaild { get; set; }
        public virtual FieldGroup FieldGroup { get; set; }
    }
}
