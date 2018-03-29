using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.DAL.LinqToSql;
using FxtSpider.FxtApi.Model;
using FxtSpider.FxtApi.ApiManager;

namespace FxtSpider.Bll.FxtApiManager
{
    public static class ProjectMatchApiManager
    {
        public static bool InsertProjectMatchByCaseIdsAndProjectNameAndCityName(long[] caseIds, string projectName, string cityName,out string message)
        {
            message = "";
            if (caseIds==null||caseIds.Length<1)
            {
                return true;
            }
            List<VIEW_案例信息_城市表_网站表> list = CaseManager.GetCaseViewByIds(caseIds);
            bool result=InsertProjectMatchByCaseListAndProjectNameAndCityName(list, projectName, cityName, out message);
            return result;

        }
        public static bool InsertProjectMatchByCaseListAndProjectNameAndCityName(List<VIEW_案例信息_城市表_网站表> vlist, string projectName, string cityName, out string message)
        {
            message = "";
            if (vlist == null || vlist.Count < 1)
            {
                return true;
            }
            FxtApi_DATProject project = ProjectApi.GetProjectByProjectNameAndCityName(projectName, cityName);
            if (project == null)
            {
                message = "楼盘名不存在";
                return false;
            }
            List<FxtApi_SYSProjectMatch> List2 = new List<FxtApi_SYSProjectMatch>();
            foreach (VIEW_案例信息_城市表_网站表 _case in vlist)
            {
                string _projectname = string.IsNullOrEmpty(_case.ProjectName) ? _case.楼盘名 : _case.ProjectName;
                if (!string.IsNullOrEmpty(_projectname))
                {
                    string name = _projectname.Trim();
                    FxtApi_SYSProjectMatch obj = List2.Where(p => p.NetName.Equals(name)).FirstOrDefault();
                    if (obj == null)
                    {
                        obj = new FxtApi_SYSProjectMatch();
                        obj.NetName = name;
                        obj.ProjectName = project.ProjectName;
                        obj.ProjectNameId = project.ProjectId;
                        obj.CityId = project.CityID;
                        List2.Add(obj);
                    }
                }
            }   
            bool result = ProjectMatchApi.InsertProjectMatch(List2, out message);
            return result;

        }
    }
}
