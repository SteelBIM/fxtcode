using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.Common;

namespace FxtSpider.FxtApi.Model
{
    public class FxtApi_ResultPageList
    {
        public int Count
        {
            get;
            set;
        }
        public string ObjJson
        {
            get;
            set;
        }
        public static FxtApi_ResultPageList ConvertToObj(string json)
        {
            return JsonHelp.ParseJSONjss<FxtApi_ResultPageList>(json);
        }
        public static List<FxtApi_ResultPageList> ConvertToObjList(string json)
        {
            return JsonHelp.ParseJSONList<FxtApi_ResultPageList>(json);
        }
    }
}
