using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxtDataAcquisition.Domain.DTO
{
    public class FieldDto
    {
        [JsonProperty(PropertyName = "fieldname")]
        public string FieldName { get; set; }

        [JsonProperty(PropertyName = "sort")]
        public int Sort { get; set; }
        /// <summary>
        /// 字段类型（1.文本框，2.多行文本，3.下拉框，5.多选框，6.日期选择框）
        /// </summary>
        [JsonProperty(PropertyName = "fieldtype")]
        public int FieldType { get; set; }
        /// <summary>
        /// 字段类型（E.文本框，T.多行文本，R.单选框，C.多选框，DT.日期选择框）
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
        /// <summary>
        /// 默认值
        /// </summary>
        [JsonProperty(PropertyName = "defaultvalue")]
        public string DefaultValue { get; set; }
        /// <summary>
        /// 字段最大长度
        /// </summary>
        [JsonProperty(PropertyName = "maxlength")]
        public int? MaxLength { get; set; }
        /// <summary>
        /// 字段最小长度
        /// </summary>
        [JsonProperty(PropertyName = "minlength")]
        public int? MinLength { get; set; }
        /// <summary>
        /// 是否必填(0.否，1.是)
        /// </summary>
        [JsonProperty(PropertyName = "isrequire")]
        public int? IsRequired { get; set; }
        /// <summary>
        /// 是否可空(0.否，1.是)
        /// </summary>
        [JsonProperty(PropertyName = "isnull")]
        public int? IsNull { get; set; }
        /// <summary>
        /// 字段值类型（0.string,1.int,2.decimal,3.datetime）
        /// </summary>
        [JsonProperty(PropertyName = "editexttype")]
        public int? EdiTextType { get; set; }
        /// <summary>
        /// 选项值
        /// </summary>
        [JsonProperty(PropertyName = "choise")]
        public List<string> Choise { get; set; }

        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }

        //[JsonProperty(PropertyName = "valuename")]
        //public string ValueName { get; set; }
    }
}
