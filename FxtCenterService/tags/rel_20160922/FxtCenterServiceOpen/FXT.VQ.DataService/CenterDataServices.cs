using System;
using System.Web;
using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

using Newtonsoft.Json;
using FXT.VQ.DataService.Model;

/***********************************************************
 * 功能：数据中心数据服务类
 *  
 * 创建：魏贝
 * 时间：2015/05
***********************************************************/

namespace FXT.VQ.DataService
{
    public static class CenterDataServices
    {

        #region 楼盘信息
        /// <summary>
        /// 获取楼盘列表
        /// </summary>
        /// <param cityid="disArea">城市ID</param>
        /// <param key="disKey">楼盘过滤关键字</param>
        /// <returns></returns>
        public static string GetDistrictList(int cityid, string key)
        {
            SurveyApi sa = new SurveyApi("projectdropdownlistmcas");
            sa.info.funinfo = new
            {
                fxtcompanyid = ConfigSettings.mDataCenterCompanyid,
                cityid = cityid,
                items = 10,//减少数据量设置为2
                key = key
            };
            string json = sa.GetJsonString();

            return ServiceHelper.APIPostBack(json);
        }

        ///// <summary>
        ///// 获取楼盘信息
        ///// </summary>
        ///// <param name="cityid">城市ID</param>
        ///// <param name="projectid">楼盘ID</param>
        ///// <returns></returns>
        //public static string GetPojectInfo(int cityid, int projectid)
        //{
        //    SurveyApi sa = new SurveyApi("mcasprojectprice");
        //    sa.info.funinfo = new
        //    {
        //        cityid = cityid,
        //        buildingid = 0,
        //        projectid = projectid,
        //        type = 0,
        //        houseid = 0,
        //        companyid = ConfigSettings.mDataCenterCompanyid,
        //        frontCode = 0,
        //        username = "admin@fxt",
        //        buildingArea = 0,
        //        systypecode = ConfigSettings.mDataCenterSystypecode,
        //        floornumber = 0,
        //        totalfloor = 0,
        //        projectprice = 0
        //    };
        //    string json = sa.GetJsonString();

        //    return ServiceHelper.APIPostBack(json);
        //}


        /// <summary>
        /// 获取详细楼盘信息
        /// </summary>
        /// <param name="cityid">城市ID</param>
        /// <param name="projectid">楼盘ID</param>
        /// <returns></returns>
        public static string GetProjectDetailInfo(int cityid, int projectid)
        {
            SurveyApi sa = new SurveyApi("queryprojectdetail");
            sa.info.funinfo = new
            {
                cityid = cityid,
                projectid = projectid
            };
            string json = sa.GetJsonString();

            return ServiceHelper.APIPostBack(json);
        }

        /// <summary>
        /// 获取楼栋列表
        /// </summary>
        /// <param name="cityid">城市ID</param>
        /// <param name="projectid">楼盘ID</param>
        /// <param name="needorder">排序</param>
        /// <param name="key">关键字</param>
        /// <param name="orderby">排序（* desc/asc）</param>
        /// <returns></returns>
        public static string GetHouseNumberList(int cityid, int projectid, bool needorder, string key, string orderby)
        {
            SurveyApi sa = new SurveyApi("buildingbaseinfolistmcas");
            sa.info.funinfo = new
            {
                fxtcompanyid = ConfigSettings.mDataCenterCompanyid,
                cityid = cityid,
                projectid = projectid,
                needorder = needorder,
                key = key,
                orderby = orderby
            };
            string json = sa.GetJsonString();

            return ServiceHelper.APIPostBack(json);
        }

        /// <summary>
        /// 获取层列表
        /// </summary>
        /// <param name="cityid">城市ID</param>
        /// <param name="key">过滤关键字</param>
        /// <param name="buildingid">楼栋ID</param>
        /// <returns></returns>
        public static string GetFoolrNumberList(int cityid, string key, int buildingid)
        {
            SurveyApi sa = new SurveyApi("housefloorlistmcas");
            sa.info.funinfo = new
            {
                fxtcompanyid = ConfigSettings.mDataCenterCompanyid,
                cityid = cityid,
                key = key,
                buildingid = buildingid
            };
            string json = sa.GetJsonString();

            return ServiceHelper.APIPostBack(json);
        }

        /// <summary>
        /// 获取室列表
        /// </summary>
        /// <param name="cityid">城市ID</param>
        /// <param name="floorno">所在楼层编码</param>
        /// <param name="buildingid">楼栋ID</param>
        /// <param name="key">过滤关键字</param>
        /// <param name="orderby">排序（* desc/asc）</param>
        /// <returns></returns>
        public static string GetRoomNumberList(int cityid, int floorno, int buildingid, string key, string orderby)
        {
            SurveyApi sa = new SurveyApi("housedropdownlistmcas");
            sa.info.funinfo = new
            {
                fxtcompanyid = ConfigSettings.mDataCenterCompanyid,
                cityid = cityid,
                floorno = floorno,
                buildingid = buildingid,
                key = key,
                orderby = orderby
            };
            string json = sa.GetJsonString();

            return ServiceHelper.APIPostBack(json);
        }

        /// <summary>
        /// 数据中心 自动估价接口
        /// </summary>
        /// <param name="cityid">城市ID</param>
        /// <param name="projectid">楼盘ID</param>
        /// <param name="buildingid">楼栋ID</param>
        /// <param name="houseid">房号ID</param>
        /// <param name="totalfloor">总楼层</param>
        /// <param name="floornumber">楼层</param>
        ///  <param name="frontcode">朝向</param>
        ///  <param name="buildingArea">建筑面积</param>
        /// <param name="projectprice"></param>
        /// <returns></returns>
        public static string GetAutoPrice(int cityid, int projectid, int buildingid, int houseid,
             int totalfloor, int floornumber, int frontcode, decimal buildingArea, double projectprice)
        {
            string functionname;
            if (totalfloor == 0)//楼盘自动估价
                functionname = "mcasprojectprice";
            else
                functionname = "mcasbhautoprice";

            SurveyApi sa = new SurveyApi(functionname);
            sa.info.funinfo = new
            {
                cityid = cityid,
                buildingid = buildingid,
                projectid = projectid,
                type = 0,
                houseid = houseid,
                companyid = ConfigSettings.mDataCenterCompanyid,
                frontCode = frontcode,
                username = "admin@fxt", //mcasLoginInfo.username,
                buildingArea = buildingArea,
                systypecode = ConfigSettings.mDataCenterSystypecode,
                floornumber = floornumber,
                totalfloor = totalfloor,
                projectprice = projectprice
            };
            string json = sa.GetJsonString();

            return ServiceHelper.APIPostBack(json);
        }

        /// <summary>
        /// 获取楼盘案例数量
        /// </summary>
        /// <param name="cityid">城市ID</param>
        /// <param name="months">近几月</param>
        /// <param name="projectids">楼盘ID 例(1,2)</param>
        /// <returns></returns>
        public static string GetCaseCountList(int cityid, int months, string projectids)
        {
            SurveyApi sa = new SurveyApi("gcasetypecountsmcas");
            sa.info.funinfo = new
            {
                fxtcompanyid = ConfigSettings.mDataCenterCompanyid,
                cityid = cityid,
                months = months,
                projectids = projectids
            };
            string json = sa.GetJsonString();

            return ServiceHelper.APIPostBack(json);
        }

        /// <summary>
        /// 获取楼盘案例
        /// </summary>
        /// <param name="cityid">城市</param>
        /// <param name="projectid">楼盘</param>
        /// <param name="PageIndex">调取的页</param>
        /// <param name="PageRowCount">每页的行数</param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static string GetCaseList(int cityid, int projectid, int PageIndex, int PageRowCount, int month)
        {
            string begindate = DateTime.Now.AddMonths(-month).ToString("yyyy-MM-dd") + " 00:00:00";
            string enddate = DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59";

            SurveyApi sa = new SurveyApi("projectcasemcas");
            sa.info.funinfo = new
            {
                fxtcompanyid = ConfigSettings.mDataCenterCompanyid,
                cityid = cityid,
                projectid = projectid,
                begindate = begindate,
                enddate = enddate,
                pageindex = PageIndex,
                pagerecords = PageRowCount,
                sortname = "casedate",
                sortorder = "desc"
            };
            string json = sa.GetJsonString();

            return ServiceHelper.APIPostBack(json);
        }

        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="cityid">城市ID</param>
        /// <param name="projectids"></param>
        /// <returns></returns>
        public static string GetImageList(int cityid, int projectid)
        {
            SurveyApi sa = new SurveyApi("projectphotomcas");//queryprojectphoto
            sa.info.funinfo = new
            {
                fxtcompanyid = ConfigSettings.mDataCenterCompanyid,
                cityid = cityid,
                projectid = projectid
            };
            string json = sa.GetJsonString();

            return ServiceHelper.APIPostBack(json);
        }


        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="cityid">城市ID</param>
        /// <param name="projectids"></param>
        /// <returns></returns>
        public static string GetImageList(int cityid, int projectid, out string posts)
        {
            SurveyApi sa = new SurveyApi("projectphotomcas");//queryprojectphoto
            sa.info.funinfo = new
            {
                fxtcompanyid = ConfigSettings.mDataCenterCompanyid,
                cityid = cityid,
                projectid = projectid
            };
            string json = sa.GetJsonString();
            posts = json;

            return ServiceHelper.APIPostBack(json);
        }

        #endregion

        #region 楼盘楼栋房号 信息

        /// <summary>
        /// 获取楼盘信息 根据楼盘ID
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public static string GetProjectInfo(int cityid, int projectid, out string outJson)
        {
            SurveyApi sa = new SurveyApi("gpdinfo");
            sa.info.funinfo = new
            {
                cityid = cityid,
                projectid = projectid
            };
            string json = sa.GetJsonString();
            outJson = json;

            return ServiceHelper.APIPostBack(json);
        }

        /// <summary>
        /// 获取楼栋信息 根据楼栋ID
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="buildingId"></param>
        /// <returns></returns>
        public static string GetBuildingInfo(int cityid, int buildingid, out string outJson)
        {
            SurveyApi sa = new SurveyApi("gbdinfo");
            sa.info.funinfo = new
            {
                cityid = cityid,
                buildingid = buildingid
            };
            string json = sa.GetJsonString();

            outJson = json;

            return ServiceHelper.APIPostBack(json);
        }

        /// <summary>
        /// 获取房号信息 根据房号ID
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="houseId"></param>
        /// <returns></returns>
        public static string GetHouseInfo(int cityid, int houseid, out string outJson)
        {
            SurveyApi sa = new SurveyApi("ghdinfo");
            sa.info.funinfo = new
            {
                cityid = cityid,
                houseid = houseid
            };
            string json = sa.GetJsonString();

            outJson = json;

            return ServiceHelper.APIPostBack(json);
        }

        #endregion

        #region 省市区

        /// <summary>
        /// 获取所有省份列表
        /// </summary>
        /// <returns></returns>
        public static string GetProvinceList()
        {
            SurveyApi sa = new SurveyApi("provincelist");
            sa.info.funinfo = new
            {
            };
            string json = sa.GetJsonString();

            return ServiceHelper.APIPostBack(json);
        }

        /// <summary>
        /// 获取城市列表 通过省份ZIPCode
        /// </summary>
        /// <returns></returns>
        public static string GetCityList(string provinceid)
        {
            SurveyApi sa = new SurveyApi("cityzcodeList");
            sa.info.funinfo = new
            {
                zipcode = provinceid
            };
            string json = sa.GetJsonString();

            return ServiceHelper.APIPostBack(json);
        }

        /// <summary>
        /// 通过行政区域编码国标(城市) 取数据中心对应的City
        /// </summary>
        /// <param name="zipcode"></param>
        /// <returns></returns>
        public static string GetCityByZip(string zipcode)
        {
            SurveyApi sa = new SurveyApi("citylist");
            sa.info.funinfo = new
            {
                zipcode = zipcode
            };
            string json = sa.GetJsonString();

            return ServiceHelper.APIPostBack(json);
        }

        /// <summary>
        /// 通过行政区域编码国标(区域) 取数据中心对应的City
        /// </summary>
        /// <param name="zipcode"></param>
        /// <returns></returns>
        public static string GetAreaByZip(string zipcode)
        {
            SurveyApi sa = new SurveyApi("garealist");
            sa.info.funinfo = new
            {
                zipcode = zipcode
            };
            string json = sa.GetJsonString();

            return ServiceHelper.APIPostBack(json);
        }


        /// <summary>
        /// 通过CityID(城市) 取数据中心对应的City
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public static string GetCityById(int cityId)
        {
            SurveyApi sa = new SurveyApi("getcityinfo");
            sa.info.funinfo = new
            {
                cityid = cityId
            };
            string json = sa.GetJsonString();

            return ServiceHelper.APIPostBack(json);
        }

        /// <summary>
        /// 通过AreaID(区域) 取数据中心对应的Area
        /// </summary>
        /// <param name="areaId"></param>
        /// <returns></returns>
        public static string GetAreaById(int areaId)
        {
            SurveyApi sa = new SurveyApi("getareainfo");
            sa.info.funinfo = new
            {
                areaid = areaId
            };
            string json = sa.GetJsonString();

            return ServiceHelper.APIPostBack(json);
        }


        #endregion

        #region MD5加密  json处理

        /// <summary>
        /// 将对象转化成json字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static string ToJson(this Object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static string GetSecurityCode(string appid, string apppwd, string signname, string time, string functionname, string key)
        {
            string[] _securityArray = new string[5];

            _securityArray[0] = appid;
            _securityArray[1] = apppwd;
            _securityArray[2] = signname;
            _securityArray[3] = time;
            _securityArray[4] = functionname;
            Array.Sort(_securityArray);
            string strmd5 = string.Join("", _securityArray);
            return GetMd5(strmd5, key);

        }

        /// <summary>
        /// 进行MD5效验
        /// </summary>
        /// <param name="strmd5"></param>
        /// <param name="key">加密key</param>
        /// <returns></returns>
        public static string GetMd5(string strmd5, string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                strmd5 += key;
            }
            byte[] md5Bytes = ASCIIEncoding.Default.GetBytes(strmd5);
            byte[] encodedBytes;
            MD5 md5;
            md5 = new MD5CryptoServiceProvider();
            //FileStream fs= new FileStream(filepath,FileMode.Open,FileAccess.Read);
            encodedBytes = md5.ComputeHash(md5Bytes);
            string nn = BitConverter.ToString(encodedBytes);
            nn = Regex.Replace(nn, "-", "");//因为转化完的都是34-2d这样的，所以替换掉- 
            nn = nn.ToLower();//根据需要转化成小写
            //fs.Close();
            return nn;
        }
        #endregion

    }
}
