using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using FxtSpider.DAL.LinqToSql;
using FxtSpider.DAL.DB;
using FxtSpider.Common;

namespace FxtSpider.Bll
{
    public static class ProjectManager
    {

        public static readonly ILog log = LogManager.GetLogger(typeof(ProjectManager));

        public static SysData_Project GetProjectByProjectNameAndCityId(string projectName, int cityId, DataClass _db = null)
        {
            DataClass db = new DataClass(_db);
            SysData_Project project = null;
            string sql = string.Format("select top 1 * from SysData_Project with(nolock) where CityId={0} and ProjectName='{0}'", cityId, projectName);
            project = db.DB.ExecuteQuery<SysData_Project>(sql).FirstOrDefault();
            //project = db.DB.SysData_Project.FirstOrDefault(p => p.CityId == cityId && p.ProjectName == projectName);
            db.Connection_Close();
            db.Dispose();
            return project;
        }

        public static List<SysData_Project> GetProjectByProjectNameLikeAndCityId(string projectNameLike, int cityId, int count, DataClass _db = null)
        {
            DataClass db = new DataClass(_db);
            List<SysData_Project> projectList = db.DB.SysData_Project.Where(p => p.CityId == cityId && p.ProjectName.Contains(projectNameLike)).Take(count).ToList();
            db.Connection_Close();
            db.Dispose();
            return projectList;
        }

        public static SysData_Project InsertProject(string projectName, int cityId, int webId, DataClass _db = null)
        {
            DataClass db = new DataClass(_db);
            SysData_Project project = new SysData_Project();
            project.ProjectName = projectName;
            project.CityId = cityId;
            project.WebId = webId;
            project = Insert(project, db);
            db.Connection_Close();
            db.Dispose();
            return project;
        }
        /// <summary>
        /// 设置案例楼盘ID,行政区ID,片区ID
        /// </summary>
        /// <param name="_list"></param>
        /// <param name="_dc"></param>
        /// <returns></returns>
        public static List<VIEW_案例信息_城市表_网站表> SetProjectId(List<VIEW_案例信息_城市表_网站表> _list, DataClass _dc = null)
        {
            if (_list != null&&_list.Count>0)
            {
                List<long> longs = new List<long>();
                _list.ForEach(delegate(VIEW_案例信息_城市表_网站表 obj)
                {
                    if ((!string.IsNullOrEmpty(obj.楼盘名.TrimBlank()) && obj.ProjectId == null)
                           || (!string.IsNullOrEmpty(obj.行政区.TrimBlank()) && obj.AreaId == null)
                           || !string.IsNullOrEmpty(obj.片区.TrimBlank()) && obj.SubAreaId == null)
                    {
                        longs.Add(obj.ID);
                    }
                });
                if (longs != null && longs.Count > 0)
                {
                    DataClass dc = new DataClass(_dc);
                    List<案例信息> list = dc.DB.案例信息.Where(p => longs.Contains(p.ID)).ToList();
                    list.ForEach(delegate(案例信息 obj)
                    {
                        if (!string.IsNullOrEmpty(obj.楼盘名.TrimBlank()) && obj.ProjectId == null)
                        {
                            SysData_Project project = ProjectManager.GetProjectByProjectNameAndCityId(obj.楼盘名.TrimBlank(), obj.城市ID, _db: dc);
                            if (project == null)
                            {
                                project = ProjectManager.InsertProject(obj.楼盘名, obj.城市ID, Convert.ToInt32(obj.网站ID), _db: dc);
                            }
                            obj.ProjectId = project.ID;
                            obj.楼盘名 = null;
                        }
                        if (!string.IsNullOrEmpty(obj.行政区.TrimBlank()) && obj.AreaId == null)
                        {
                            SysData_Area areaObj = AreaManager.GetAreaByAreaNameLikeByCityId(obj.行政区, obj.城市ID, _db: dc);
                            if (areaObj == null)
                            {
                                areaObj = AreaManager.InsertArea(obj.行政区, obj.城市ID, Convert.ToInt32(obj.网站ID), _db: dc);
                            }
                            obj.AreaId = areaObj.ID;
                            obj.行政区 = null;
                        }
                        else if (string.IsNullOrEmpty(obj.行政区.TrimBlank()) && obj.AreaId == null)
                        {
                            obj.AreaId =0;
                            obj.行政区 = null;
                        }
                        if (!string.IsNullOrEmpty(obj.片区.TrimBlank()) && obj.SubAreaId == null)
                        {
                            SysData_SubArea subAreaObj = SubAreaManager.GetAreaByAreaNameByCityId(obj.片区, obj.城市ID, _db: dc);
                            if (subAreaObj == null)
                            {
                                subAreaObj = SubAreaManager.InsertArea(obj.片区, obj.城市ID, Convert.ToInt32(obj.网站ID), _db: dc);
                            }
                            obj.SubAreaId = subAreaObj.ID;
                            obj.片区 = null;
                        }
                        VIEW_案例信息_城市表_网站表 vObj = _list.Where(p => p.ID == obj.ID).FirstOrDefault();
                        if (vObj != null)
                        {
                            vObj.ProjectId = obj.ProjectId;
                            vObj.楼盘名 = obj.楼盘名;
                            vObj.AreaId = obj.AreaId;
                            vObj.行政区 = obj.行政区;
                            vObj.SubAreaId = obj.SubAreaId;
                            vObj.片区 = obj.片区;
                        }
                    });
                    dc.DB.SubmitChanges();
                    dc.Connection_Close();
                    dc.Dispose();
                }
            }
            return _list;
        }
        /// <summary>
        /// 设置案例楼盘ID,行政区ID,片区ID
        /// </summary>
        /// <param name="_list"></param>
        /// <param name="_dc"></param>
        /// <returns></returns>
        public static List<案例信息> SetProjectId(List<案例信息> _list, DataClass _dc = null)
        {
            if (_list != null && _list.Count > 0)
            {
                List<long> longs = new List<long>();
                _list.ForEach(delegate(案例信息 obj)
                {
                    if ((!string.IsNullOrEmpty(obj.楼盘名.TrimBlank()) && obj.ProjectId == null)
                        || (!string.IsNullOrEmpty(obj.行政区.TrimBlank()) && obj.AreaId == null)
                        || !string.IsNullOrEmpty(obj.片区.TrimBlank()) && obj.SubAreaId == null)
                    {
                        longs.Add(obj.ID);
                    }
                });
                if (longs != null && longs.Count > 0)
                {
                    DataClass dc = new DataClass(_dc);
                    List<案例信息> list = dc.DB.案例信息.Where(p => longs.Contains(p.ID)).ToList();
                    list.ForEach(delegate(案例信息 obj)
                    {
                        if (!string.IsNullOrEmpty(obj.楼盘名.TrimBlank()) && obj.ProjectId == null)
                        {
                            SysData_Project project = ProjectManager.GetProjectByProjectNameAndCityId(obj.楼盘名.TrimBlank(), obj.城市ID, _db: dc);
                            if (project == null)
                            {
                                project = ProjectManager.InsertProject(obj.楼盘名, obj.城市ID, Convert.ToInt32(obj.网站ID), _db: dc);
                            }
                            obj.ProjectId = project.ID;
                            obj.楼盘名 = null;
                        }
                        if (!string.IsNullOrEmpty(obj.行政区.TrimBlank()) && obj.AreaId == null)
                        {
                            SysData_Area areaObj = AreaManager.GetAreaByAreaNameLikeByCityId(obj.行政区, obj.城市ID, _db: dc);
                            if (areaObj == null)
                            {
                                areaObj = AreaManager.InsertArea(obj.行政区, obj.城市ID, Convert.ToInt32(obj.网站ID), _db: dc);
                            }
                            obj.AreaId = areaObj.ID;
                            obj.行政区 = null;
                        }
                        else if (string.IsNullOrEmpty(obj.行政区.TrimBlank()) && obj.AreaId == null)
                        {
                            obj.AreaId = 0;
                            obj.行政区 = null;
                        }
                        if (!string.IsNullOrEmpty(obj.片区.TrimBlank()) && obj.SubAreaId == null)
                        {
                            SysData_SubArea subAreaObj = SubAreaManager.GetAreaByAreaNameByCityId(obj.片区, obj.城市ID, _db: dc);
                            if (subAreaObj == null)
                            {
                                subAreaObj = SubAreaManager.InsertArea(obj.片区, obj.城市ID, Convert.ToInt32(obj.网站ID), _db: dc);
                            }
                            obj.SubAreaId = subAreaObj.ID;
                            obj.片区 = null;
                        }
                        案例信息 vObj = _list.Where(p => p.ID == obj.ID).FirstOrDefault();
                        if (vObj != null)
                        {
                            vObj.ProjectId = obj.ProjectId;
                            vObj.楼盘名 = obj.楼盘名;
                            vObj.AreaId = obj.AreaId;
                            vObj.行政区 = obj.行政区;
                            vObj.SubAreaId = obj.SubAreaId;
                            vObj.片区 = obj.片区;
                        }
                    });
                    dc.DB.SubmitChanges();
                    dc.Connection_Close();
                    dc.Dispose();
                }
            }
            return _list;
        }



        public static SysData_Project Insert(SysData_Project obj, DataClass _db = null)
        {
            DataClass db = new DataClass(_db);
            if (obj != null)
            {
                long nowID = 0;
                db.DB.SysData_Project_Insert(obj.ProjectName,obj.CityId,obj.WebId, out nowID);
                obj.ID = nowID;
            }
            return obj;
        }
    }
}
