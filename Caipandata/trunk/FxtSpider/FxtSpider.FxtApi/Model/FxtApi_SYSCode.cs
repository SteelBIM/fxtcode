using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.Common;

namespace FxtSpider.FxtApi.Model
{
    public class FxtApi_SYSCode
    {

        /// <summary>
        /// ID
        /// </summary>
        public virtual int ID
        {
            get;
            set;
        }
        /// <summary>
        /// Code
        /// </summary>
        public virtual int Code
        {
            get;
            set;
        }
        /// <summary>
        /// CodeName
        /// </summary>
        public virtual string CodeName
        {
            get;
            set;
        }
        /// <summary>
        /// CodeType
        /// </summary>
        public virtual string CodeType
        {
            get;
            set;
        }
        /// <summary>
        /// Remark
        /// </summary>
        public virtual string Remark
        {
            get;
            set;
        }
        /// <summary>
        /// SubCode
        /// </summary>
        public virtual int? SubCode
        {
            get;
            set;
        }

        public static FxtApi_SYSCode ConvertToObj(string json)
        {
            return JsonHelp.ParseJSONjss<FxtApi_SYSCode>(json);
        }
        public static List<FxtApi_SYSCode> ConvertToObjList(string json)
        {
            return JsonHelp.ParseJSONList<FxtApi_SYSCode>(json);
        }
    }
}
