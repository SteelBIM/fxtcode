using BaiduAPI;
using BaiduAPI.WebAPI.GeocodingAPI;
using FxtDataAcquisition.Domain.DTO.FxtDataWcfDTO;
using FxtDataAcquisition.FxtAPI.FxtDataWcf.Manager;
using FxtDataAcquisition.NHibernate.Entities;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.BLL
{
    public class BaiduApiManager
    {
        public static readonly ILog log = LogManager.GetLogger(typeof(BaiduApiManager));
        private static string ak;
        private static int radius = 1000;
        private static string query;
        private static PlaceRequest rq;
        private static List<FxtApi_SYSCity> list;
        public static void loadData()
        {
            ak = ConfigurationManager.AppSettings["baidu_ak"];
            if (!int.TryParse(ConfigurationManager.AppSettings["baidu_radius"], out radius))
                radius = 1000;
            query = ConfigurationManager.AppSettings["baidu_query"];

            rq = new PlaceRequest();
            rq.ak = ak;
            rq.page_num = 0;
            rq.page_size = 20;
            rq.radius = radius;
            rq.scope = BaiduAPI.ScopeType.Details;
            rq.output = BaiduAPI.OutputType.json;
            rq.region = "全国";
            rq.query = query;
        }
        /// <summary>
        /// 获取所有兴趣点
        /// </summary>
        /// <param name="location"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public static List<LNKPAppendage> GetAllAppendage(string location, int projectId)
        {
            loadData();

            list = SYSCityApi.GetAllCity();

            //rq.location = "22.55932,114.04508";
            List<LNKPAppendage> lnkpaList = new List<LNKPAppendage>();
            try
            {
                rq.location = location;
                string[] queryArray = rq.query.Split('$');
                for (int t = 0; t < queryArray.Length; t++)
                {
                    rq.page_num = 0;
                    rq.query = queryArray[t];

                    //分页获取
                    PlaceResponse rs = BaiduAPI.PlaceAPIManger.SearchPOI(rq);
                    BaiduApiManager.FullAppendage(rs, projectId, rq.query, lnkpaList);
                    if (rs.total > 20)
                    {
                        int pageTotal = rs.total / 20;
                        for (int i = 1; i < pageTotal + 1; i++)
                        {
                            rq.page_num = i;
                            PlaceResponse rs2 = BaiduAPI.PlaceAPIManger.SearchPOI(rq);
                            BaiduApiManager.FullAppendage(rs2, projectId, rq.query, lnkpaList);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                lnkpaList = new List<LNKPAppendage>();
            }

            return lnkpaList;
        }

        private static void FullAppendage(PlaceResponse rs, int projectId, string type, List<LNKPAppendage> lList)
        {
            if (rs.status == "0" && rs.total > 0)
            {
                foreach (var item in rs.results)
                {
                    LNKPAppendage inkpa = new LNKPAppendage();
                    inkpa.AppendageCode = GetCode(type);
                    inkpa.P_AName = item.name;
                    inkpa.Distance = item.detail_info.distance;
                    inkpa.ProjectId = projectId;
                    inkpa.Address = item.address;
                    inkpa.x = item.location.lng;
                    inkpa.y = item.location.lat;
                    inkpa.Uid = item.uid;
                    inkpa.CityId = GetCityIdBylocation(item.location);

                    var list = lList.Where(m => m.Uid == inkpa.Uid);
                    if (list.Count() < 1)
                    {
                        lList.Add(inkpa);
                    }
                }
            }
        }
        /// <summary>
        /// 获取城市id 根据坐标
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        private static int GetCityIdBylocation(Location location)
        {
            int cityid = 0;
            string loc = location.lat + "," + location.lng;
            GeocodingResponse response = GeocodingManager.Search(new GeocodingRequest()
            {
                ak = ak,
                output = OutputType.json,
                pois = 0,
                location = loc
            });
            FxtApi_SYSCity city = list.Where(m => m.CityName.Contains(response.result.addressComponent.city)).FirstOrDefault();
            if (city != null)
            {
                cityid = city.CityId;
            }
            return cityid;
        }

        private static int GetCode(string type)
        {
            int code = 0;
            switch (type)
            {
                case "会所":
                    code = 2008001;
                    break;
                case "超市":
                    code = 2008002;
                    break;
                case "幼儿园":
                    code = 2008003;
                    break;
                case "地铁轻轨":
                    code = 2008004;
                    break;
                case "银行":
                    code = 2008005;
                    break;
                case "学校":
                    code = 2008006;
                    break;
                case "医院":
                    code = 2008007;
                    break;
                case "公园":
                    code = 2008008;
                    break;
                case "商场":
                    code = 2008009;
                    break;
                case "菜市场":
                    code = 2008010;
                    break;
                case "公交":
                    code = 2008012;
                    break;
                case "图书馆":
                    code = 2008024;
                    break;
                case "体育馆":
                    code = 2008025;
                    break;
                case "音乐厅":
                    code = 2008026;
                    break;
                case "少年宫":
                    code = 2008027;
                    break;
                case "美术馆":
                    code = 2008028;
                    break;
                case "酒店":
                    code = 2008029;
                    break;
                default:
                    break;
            }
            return code;
        }
    }
}
