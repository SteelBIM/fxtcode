using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtDataAcquisition.Common;
using FxtDataAcquisition.NHibernate.Entities;
using FxtDataAcquisition.Data;
using NHibernate;
using log4net;
using FxtDataAcquisition.Application.Services;

namespace FxtDataAcquisition.BLL
{
    /// <summary>
    /// 数据库调出
    /// </summary>
    public class DatabaseCallManager
    {
        public static readonly ILog log = LogManager.GetLogger(typeof(DatabaseCallManager));
        /// <summary>
        /// 调出
        /// </summary>
        /// <param name="projects"></param>
        /// <returns></returns>
        public static bool Call(List<CAS.Entity.DBEntity.DATProject> projects)
        {
            DataBase db = new DataBase();
            using (ITransaction tran = db.DB.BeginTransaction())
            {
                try
                {
                    foreach (var item in projects)
                    {
                        string json = item.ToJSONjss();
                        DATProject project = JsonHelp.ParseJSONjss<DATProject>(json);
                        project.Status = SYSCodeManager.STATECODE_1;
                        project.FxtProjectId = item.projectid;
                        //db.DB.Create(project, tran);

                        //string buildingJson = item.buildinglist.ToJson();
                        //List<DATBuilding> buildings = JsonHelp.ParseJSONList<DATBuilding>(buildingJson);
                        ////db.DB.Create<DATBuilding>(buildings, tran);
                        //foreach (var buildingItem in item.buildinglist)
                        //{
                        //    //buildingItem.houselist.GroupBy(m=>m.)
                        //    string houseJson = buildingItem.houselist.ToJson();
                        //    List<DATHouse> houses = JsonHelp.ParseJSONList<DATHouse>(houseJson);
                        //    //db.DB.Create<DATHouse>(houses, tran);
                        //}
                    }
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message, ex);
                    return false;
                }
            }
            db.Close();
            return true;
        }
    }
}
