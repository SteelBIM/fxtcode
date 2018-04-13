using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.Contract.ViewModel
{
  public  class ApiDocModel
    {
        public string SystemCode { get; set; }
        public string SystemName { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public bool hover { get; set; }
        public string format { get; set; }
        public string method { get; set; }
        public string apiway { get; set; }
        /// <summary>
        /// 请求参数示例
        /// </summary>
        public object demo { get; set; }
        /// <summary>
        /// 请求参数说明
        /// </summary>
        public List<InParamsModel> inparams { get; set; }
        public object output { get; set; }
        public List<InParamsModel> outexplain { get; set; }
    }
   public class ParamModel
    {
        public string ParameterFields { get; set; }
        public string ParameterValue { get; set; }
    }
    public class InParamsModel
    {
        public string name { get; set; }
        public string value { get; set; }
        public string desc { get; set; }
        public bool must { get; set; }
    }
}
