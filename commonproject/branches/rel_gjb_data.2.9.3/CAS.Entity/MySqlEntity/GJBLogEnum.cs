using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.Entity.MySqlEntity
{
    public class GJBLogEnum
    {
       
        public class WebType
        {
            public const string 估价宝 = "gjb";
            public const string CAS = "gjbcas";
            public const string 微信 = "gjbwx";
            public const string 估价宝API = "gjbapi";
        }
        public enum OpFunctionType
        {
            页面请求 = 1,
            正式报告生成_Word = 2,
            预评报告生成_Word = 3,
            测算生成 = 4,
            询价单生成 = 5,
            查勘生成 = 6,
            其他附件生成 = 7,
            税费生成 = 8,
            正式报告生成_Excel = 9,
            预评报告生成_Excel = 10,
            业务附件生成 = 11,
            预评附件生成 = 12,
            报告附件生成 = 13,
            收费附件生成 = 14,
            归档附件生成 = 15
        }
    }
}
