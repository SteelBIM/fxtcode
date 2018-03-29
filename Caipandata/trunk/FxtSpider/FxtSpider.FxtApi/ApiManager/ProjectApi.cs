using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.FxtApi.Fxt.Api;
using FxtSpider.FxtApi.Model;
using FxtSpider.Common;
using log4net;
using FxtSpider.FxtApi.FxtApiClientManager;
using Newtonsoft.Json.Linq;

namespace FxtSpider.FxtApi.ApiManager
{
    public static class ProjectApi
    {
        public static readonly ILog log = LogManager.GetLogger(typeof(ProjectApi));

        #region (更新)
        public static bool InsertProjectApi(string projectName, int cityId, int areaId, int purposeCode, string address, out string message, FxtAPIClientExtend _fxtApi = null)
        {
            bool r=true;
            message = "";
            FxtAPIClientExtend fxtApi = new FxtAPIClientExtend(_fxtApi);
            try
            {

                string name = "InsertProject";
                var para = new { projectName = projectName, cityId = cityId, areaId = areaId, purposeCode = purposeCode, address=address };
                string jsonStr = Convert.ToString(EntranceApi.Entrance(name, para.ToJSONjss(), _fxtApi: fxtApi));   
                FxtApi_Result result = FxtApi_Result.ConvertToObj(jsonStr);
                if (result == null)
                {
                    fxtApi.Abort();
                    return false;
                }
                r = result.Result == 0 ? false : true;
                message = JsonHelp.DecodeField(result.Message);
                fxtApi.Abort();
            }
            catch (Exception ex)
            {
                fxtApi.Abort();
                log.Error("InsertProjectApi(string projectName,int cityId,int areaId,int purposeCode,string address,out string message)", ex);
                return false;
            }
            return r;
        }
        #endregion

        #region (查询)

        /// <summary>
        /// 根据城市名称+楼盘名称模糊检索出楼盘信息
        /// </summary>
        /// <param name="projectNameLike"></param>
        /// <param name="cityName"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<FxtApi_DATProject> GetProjectByCityNameAndLikeProjectName(string projectNameLike, string cityName, int count, FxtAPIClientExtend _fxtApi = null)
        {
            if (string.IsNullOrEmpty(projectNameLike) || string.IsNullOrEmpty(cityName))
            {
                return new List<FxtApi_DATProject>();
            }
            List<FxtApi_DATProject> list = new List<FxtApi_DATProject>();
            FxtAPIClientExtend fxtApi = new FxtAPIClientExtend(_fxtApi);
            try
            {
                string name = "GetProjectByCityNameAndLikePrjName";
                var para = new { cityName = cityName, projectName = projectNameLike, length = count };
                string jsonStr = Convert.ToString(EntranceApi.Entrance(name, para.ToJSONjss(), _fxtApi: fxtApi));  

                if (string.IsNullOrEmpty(jsonStr))
                {
                    fxtApi.Abort();
                    return new List<FxtApi_DATProject>();
                }
                list = FxtApi_DATProject.ConvertToObjList(jsonStr);
                list = list.DecodeField();
                fxtApi.Abort();
            }
            catch (Exception ex)
            {
                fxtApi.Abort();
                log.Error("GetProjectByCityNameAndLikeProjectName(string projectNameLike, string cityName, int count)", ex);
            }
            return list;
        }

        public static List<FxtApi_DATProjectView> GetProjectViewByCityNameAndLikeProjectName(string projectNameLike, string cityName, int count, FxtAPIClientExtend _fxtApi = null)
        {

            if (string.IsNullOrEmpty(projectNameLike) || string.IsNullOrEmpty(cityName))
            {
                return new List<FxtApi_DATProjectView>();
            }
            List<FxtApi_DATProjectView> list = new List<FxtApi_DATProjectView>();
            FxtAPIClientExtend fxtApi = new FxtAPIClientExtend(_fxtApi);
            try
            {
                string name = "GetProjectViewByCityNameAndLikePrjName";
                var para = new { cityName = cityName, projectName = projectNameLike, length = count };
                string jsonStr = Convert.ToString(EntranceApi.Entrance(name, para.ToJSONjss(), _fxtApi: fxtApi));  

                if (string.IsNullOrEmpty(jsonStr))
                {
                    fxtApi.Abort();
                    return new List<FxtApi_DATProjectView>();
                }
                list = FxtApi_DATProjectView.ConvertToObjList(jsonStr);
                list = list.DecodeField();
                fxtApi.Abort();
            }
            catch (Exception ex)
            {
                fxtApi.Abort();
                log.Error(string.Format("GetProjectViewByCityNameAndLikeProjectName(string projectNameLike:{0}, string cityName:{1}, int count:{2})",
                    projectNameLike == null ? "null" : "", cityName == null ? "null" : "", count), ex);
            }
            return list;
        }

        /// <summary>
        /// 根据名称成城市ID,获取楼盘信息(关联网络名查询)
        /// </summary>
        /// <param name="projectName">名字</param>
        /// <param name="fxtCityId">服务端的城市ID</param>
        /// <returns></returns>
        public static List<FxtApi_DATProject> GetProjectJoinProjectMatchByProjectNameCityId(string projectName, int fxtCityId, FxtAPIClientExtend _fxtApi = null)
        {
            if (string.IsNullOrEmpty(projectName))
            {
                return new List<FxtApi_DATProject>();
            }
            List<FxtApi_DATProject> list = new List<FxtApi_DATProject>();
            FxtAPIClientExtend fxtApi = new FxtAPIClientExtend(_fxtApi);
            try
            {
                string name = "GetProjectJoinProjectMatchByProjectNameCityId";
                var para = new { projectName = projectName, cityId = fxtCityId };
                string jsonStr = Convert.ToString(EntranceApi.Entrance(name, para.ToJSONjss(), _fxtApi: fxtApi));  

                if (string.IsNullOrEmpty(jsonStr))
                {
                    fxtApi.Abort();
                    return new List<FxtApi_DATProject>();
                }
                list = FxtApi_DATProject.ConvertToObjList(jsonStr);
                list = list.DecodeField();
                fxtApi.Abort();
            }
            catch (Exception ex)
            {
                fxtApi.Abort();
                log.Error(string.Format("GetProjectJoinProjectMatchByProjectNameCityId(string projectName:{0}, int fxtCityId:{1})",
                    projectName == null ? "null" : "", fxtCityId), ex);
            }
            return list;
        }
        /// <summary>
        /// 根据楼盘名+城市名获取楼盘信息
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="cityName"></param>
        /// <returns></returns>
        public static FxtApi_DATProject GetProjectByProjectNameAndCityName(string projectName, string cityName, FxtAPIClientExtend _fxtApi = null)
        {
            if (string.IsNullOrEmpty(projectName) || string.IsNullOrEmpty(cityName))
            {
                return null;
            }
            FxtApi_DATProject project = null;
            FxtAPIClientExtend fxtApi = new FxtAPIClientExtend(_fxtApi);
            try
            {
                string name = "GetProjectByProjectNameAndCityName";
                var para = new { projectName = projectName, cityName = cityName };
                string jsonStr = Convert.ToString(EntranceApi.Entrance(name, para.ToJSONjss(), _fxtApi: fxtApi));

                if (string.IsNullOrEmpty(jsonStr))
                {
                    fxtApi.Abort();
                    return null;
                }
                project = FxtApi_DATProject.ConvertToObj(jsonStr);
                project = project.DecodeField();
                fxtApi.Abort();
            }
            catch (Exception ex)
            {
                fxtApi.Abort();
                log.Error(string.Format("GetProjectByProjectNameAndCityName(string projectName:{0}, string cityName:{1})",
                    projectName == null ? "null" : "", cityName == null ? "null" : ""), ex);
            }
            return project;
        }
        /// <summary>
        /// 根据城市名称查询楼盘信息
        /// </summary>
        /// <param name="cityName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <param name="isGetCount">是否获取总数</param>
        /// <returns></returns>
        public static List<FxtApi_DATProject> GetProjectByCityName(string cityName, int pageIndex, int pageSize, out int count, bool isGetCount = true, FxtAPIClientExtend _fxtApi = null)
        {
            count = 0;
            int isgetcount = 1;
            if (!isGetCount)
            {
                isgetcount = 0;
            }
            FxtAPIClientExtend fxtApi = new FxtAPIClientExtend(_fxtApi);


            string name = "GetProjectByCityNameAndPage";
            var para = new { cityName = cityName, pageIndex = pageIndex, pageSize = pageSize, isGetCount = isgetcount };
            string jsonStr = Convert.ToString(EntranceApi.Entrance(name, para.ToJSONjss(), _fxtApi: fxtApi));

            if (string.IsNullOrEmpty(jsonStr))
            {
                fxtApi.Abort();
                return new List<FxtApi_DATProject>();
            }
            FxtApi_ResultPageList listObj = FxtApi_ResultPageList.ConvertToObj(jsonStr);
            count = listObj.Count;
            List<FxtApi_DATProject> list = FxtApi_DATProject.ConvertToObjList(listObj.ObjJson);
            list = list.DecodeField();
            fxtApi.Abort();
            return list;
        }

        public static List<FxtApi_DATProject> GetProjectByCityIdAndProjectIds(int fxtCityId, int[] projectIds, FxtAPIClientExtend _fxtApi = null)
        {
            string projectIdsStr = projectIds.ConvertToString();
            List<FxtApi_DATProject> list = new List<FxtApi_DATProject>();
            if (string.IsNullOrEmpty(projectIdsStr))
            {
                return list;
            }
            FxtAPIClientExtend fxtApi = new FxtAPIClientExtend(_fxtApi);
            try
            {
                JObject jObjPara = new JObject();
                jObjPara.Add(new JProperty("cityId", fxtCityId));
                jObjPara.Add(new JProperty("projectIds", projectIdsStr));
                string methodName = "GetProjectByCityIdAndProjectIds";
                string jsonStr = Convert.ToString(EntranceApi.Entrance(methodName, jObjPara.ToJSONjss(), _fxtApi: fxtApi));
                if (string.IsNullOrEmpty(jsonStr))
                {
                    fxtApi.Abort();
                    return new List<FxtApi_DATProject>();
                }
                list = FxtApi_DATProject.ConvertToObjList(jsonStr);
                list = list.DecodeField();
                fxtApi.Abort();
            }
            catch (Exception ex)
            {
                fxtApi.Abort();
                log.Error(string.Format("GetProjectByCityIdAndProjectIds(int fxtCityId, int[] projectIds, FxtAPIClientExtend _fxtApi = null),fxtCity={0}",
                     fxtCityId), ex);
            }
            return list;
        }
    

        #endregion

        public static void testcross()
        {
            FxtAPIClientExtend fxtApi = new FxtAPIClientExtend();
            try
            {
                JObject jObjPara = new JObject();
                jObjPara.Add(new JProperty("cityId", 1));
                string methodName = "testcross";
                string jsonStr = Convert.ToString(EntranceApi.Entrance(methodName, jObjPara.ToJSONjss(), _fxtApi: fxtApi));
            
                fxtApi.Abort();
            }
            catch (Exception ex)
            {
            }
        }
    }
}
