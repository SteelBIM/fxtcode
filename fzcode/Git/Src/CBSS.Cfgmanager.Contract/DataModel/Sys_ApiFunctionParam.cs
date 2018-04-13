using CBSS.Framework.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Cfgmanager.Contract.DataModel
{
    [Auditable]
    [Table("Sys_ApiFunctionParam")]
    public class Sys_ApiFunctionParam
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int ApiFunctionParamID { get; set; }
        /// <summary>
        /// 父参数名
        /// </summary>
        public string ApiFunctionParamParentID { get; set; }
        /// <summary>
        /// 参数名
        /// </summary>
        public string ParameterFields { get; set; }
        /// <summary>
        /// 参数描述
        /// </summary>
        public string ParameterExplain { get; set; }
        /// <summary>
        /// 参数值
        /// </summary>
        public string ParameterValue { get; set; }
        /// <summary>
        /// 参数字段类型（1:int,2:string,3:bool,4:list,5:class,6:double,7:strJson）
        /// </summary>
        public string ParameterType { get; set; }
        /// <summary>
        /// 是否必填
        /// </summary>
        public int IsAllowNull { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 接口ID
        /// </summary>
        public int ApiFunctionID { get; set; }
        /// <summary>
        /// 参数请求类型（0:请求参数，1:返回参数）
        /// </summary>
        public int Type { get; set; }
    }
}
