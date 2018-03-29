using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.FxtApi.Model;
using FxtSpider.FxtApi.ApiManager;

namespace FxtSpider.Bll.FxtApiManager
{
    public class AreaApiManager
    {
        public static List<FxtApi_SYSArea> GetAreaByCityName(string cityName)
        {
            if (string.IsNullOrEmpty(cityName))
            {
                return new List<FxtApi_SYSArea>();
            }
            List<FxtApi_SYSArea> list = AreaApi.GetAreaByCityName(cityName);
            return list;
        }
        public static List<FxtApi_SYSArea> GetAreaByCityId(int cityId)
        {
            List<FxtApi_SYSArea> list = AreaApi.GetAreaByCityId(cityId);
            return list;
        }
    }
}
