using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using FxtSpider.Common;

namespace FxtSpider.FxtApi.Model
{
    public  class FxtApi_SYSArea
    {
        public int AreaId
        {
            get;
            set;
        }
        public string AreaName
        {
            get;
            set;
        }
        public int CityId
        {
            get;
            set;
        }
        public FxtApi_SYSArea()
        { }
        public FxtApi_SYSArea(int areaId, string areaName, int cityId)
        {
            this.AreaId = areaId;
            this.AreaName = areaName;
            this.CityId = cityId;
        }
        public static FxtApi_SYSArea ConvertToObj(string json)
        {
            return JsonHelp.ParseJSONjss<FxtApi_SYSArea>(json);
        }
        public static List<FxtApi_SYSArea> ConvertToObjList(string json)
        {
            return JsonHelp.ParseJSONList<FxtApi_SYSArea>(json);
        }
    }
}
