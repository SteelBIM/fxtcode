using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.Common;

namespace FxtSpider.FxtApi.Model
{
    public class FxtApi_Result
    {
        /// <summary>
        /// 结果,1:城市,0:失败
        /// </summary>
        public int Result
        {
            get;
            set;
        }
        /// <summary>
        /// 消息结果
        /// </summary>
        public string Message
        {
            get;
            set;
        }
        /// <summary>
        /// 返回的其他内容
        /// </summary>
        public string Detail
        {
            get;
            set;
        }

        public FxtApi_Result()
        { }
        public FxtApi_Result(int result, string message, string detail)
        {
            this.Result = result;
            this.Message = message;
            this.Detail = detail;
        }
        public FxtApi_Result(int result, string message)
        {
            this.Result = result;
            this.Message = message;
        }

        public static FxtApi_Result ConvertToObj(string json)
        {
            return JsonHelp.ParseJSONjss<FxtApi_Result>(json);
        }
        public static List<FxtApi_Result> ConvertToObjList(string json)
        {
            return JsonHelp.ParseJSONList<FxtApi_Result>(json);
        }
    }
}
