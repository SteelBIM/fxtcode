using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.FxtApi.ApiManager;
using FxtSpider.FxtApi.Model;
using FxtSpider.Common;

namespace FxtSpider.Bll.FxtApiManager
{
    public static class ProjectApiManager
    {
        /// <summary>
        /// 根据名称+城市名称 模糊获取
        /// </summary>
        /// <param name="likeName"></param>
        /// <param name="cityName"></param>
        /// <param name="count"></param>
        /// <returns>FxtApi_DATProject实体json字符串数组</returns>
        public static string[] GetProjectInfoStringsByLikeNameAndCityName(string likeName, string cityName, int count)
        {
            string[] strings = null;
            List<FxtApi_DATProjectView> list = ProjectApi.GetProjectViewByCityNameAndLikeProjectName(likeName, cityName, count);
            if (list != null&&list.Count>0)
            {
                List<string> strList = new List<string>();
                foreach (FxtApi_DATProjectView project in list)
                {
                    strList.Add(project.EncodeField().ToJSONjss());
                }
                strings = strList.ToArray();
            }
            return strings;
        }
        /// <summary>
        /// 根据爬取楼盘名称获取对应的库名称
        /// </summary>
        /// <param name="projectNames"></param>
        /// <param name="cityName"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetNowProjectNameJoinFxtProjectName(string[] projectNames, string cityName)
        {
            Dictionary<string, string> dic = null;
            if (projectNames != null && projectNames.Length > 0)
            {
                List<string> strList = new List<string>();
                foreach (string str in projectNames)
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        string str2 = strList.Where(p => p == str).FirstOrDefault();
                        if (string.IsNullOrEmpty(str2))
                        {
                            strList.Add(str);
                        }
                    }
                }
                projectNames = strList.ToArray();
            }
            if (projectNames == null || projectNames.Length < 1)
            {
                return dic;
            }
            dic = new Dictionary<string, string>();
            foreach (string str in projectNames)
            {
                if (!dic.ContainsKey(str))
                {
                    dic.Add(str, "");
                }
            }
            if (string.IsNullOrEmpty(cityName))
            {
                return dic;
            }
            FxtApi_SYSCity city = CityApi.GetCityByCityName(cityName);
            if (city == null)
            {
                return dic;
            }
            foreach (string str in projectNames)
            {
                string value = "";
                List<FxtApi_DATProject> projectList = ProjectApi.GetProjectJoinProjectMatchByProjectNameCityId(str, city.CityId);
                if (projectList != null && projectList.Count > 0)
                {
                    value = projectList[0].ProjectName;
                }
                dic[str] = value;
            }
            return dic;
        }
    }
}
