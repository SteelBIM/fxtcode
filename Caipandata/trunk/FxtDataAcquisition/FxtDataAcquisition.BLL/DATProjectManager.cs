using FxtDataAcquisition.Data;
using FxtDataAcquisition.FxtAPI.FxtDataWcf.Manager;
using FxtDataAcquisition.NHibernate.Entities;
using log4net;
using Newtonsoft.Json.Linq;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using FxtDataAcquisition.DTODomain.NHibernate;
using FxtDataAcquisition.Common;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using FxtDataAcquisition.Application.Services;

namespace FxtDataAcquisition.BLL
{
    public static class DATProjectManager
    {
        public static readonly ILog log = LogManager.GetLogger(typeof(DATProjectManager));

        #region 查询

        /// <summary>
        /// 获取新查勘任务(未查勘+已接收)
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="cityId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="isGetCount"></param>
        /// <returns></returns>
        public static IList<DATProject> GetAllotSurveyProject(string userName, int cityId, int pageIndex, int pageSize, bool isGetCount = true, DataBase _db = null)
        {
            int count = 0;
            DataBase db = new DataBase(_db);
            try
            {
                IList<DATProject> list = GetAllotProjectByStatus(userName, cityId, new int[] { SYSCodeManager.STATECODE_2, SYSCodeManager.STATECODE_3 }, pageIndex, pageSize, out count, isGetCount, db);
                db.Close();
                return list;
            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }
        }
        /// <summary>
        /// 根据任务状态获取任务楼盘
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="cityId"></param>
        /// <param name="status">多个状态id组成的数组</param>
        /// <param name="pageInfo">分页类,null为查询全部</param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static IList<DATProject> GetAllotProjectByStatus(string userName, int cityId, int[] status, int pageIndex, int pageSize, out int count, bool isGetCount = true, DataBase _db = null)
        {
            count = 0;
            if (status == null || status.Length < 1)
            {
                return new List<DATProject>();
            }
            DataBase db = new DataBase(_db);
            try
            {
                string sql = string.Format("{0} Valid=1 and projectid in (select DatId from Dat_AllotFlow  where CityId=:cityId and StateCode in ({1}) and SurveyUserName=:surveyUserName and DatType=:datType) ",
                    //string sql = string.Format("{0} Valid=1 and projectid in (select DatId from Dat_AllotFlow  where CityId=:cityId and StateCode in ({1}) and SurveyUserName=:surveyUserName and DatType=:datType)",
                NHibernateUtility.GetMSSQL_SQL(typeof(DATProject), NHibernateUtility.TableName_DATProject),
                status.ConvertToString());

                List<NHParameter> parameters = new List<NHParameter>();
                parameters.Add(new NHParameter("cityId", cityId, NHibernateUtil.Int32));
                parameters.Add(new NHParameter("surveyUserName", userName, NHibernateUtil.String));
                parameters.Add(new NHParameter("datType", SYSCodeManager.DATATYPECODE_1, NHibernateUtil.Int32));
                IList<DATProject> list = new List<DATProject>();
                UtilityPager pageInfo = new UtilityPager(pageSize, pageIndex, isGetCount);
                list = db.DB.PagerList<DATProject>(pageInfo, sql, parameters, "UpdateDateTime", "Desc").ToList();
                db.Close();
                return list;
            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }
        }
        /// <summary>
        /// 获取所有任务（不包含未分配的任务）
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public static IList<DATProject> GetAllotProject(string projectName, int cityId, DataBase _db = null)
        {
            DataBase db = new DataBase(_db);
            try
            {
                string sql = string.Format("{0} Valid=1 and ProjectName like '%{1}%' and projectid in (select DatId from Dat_AllotFlow  where CityId={2} and StateCode <>{3} and DatType={4})  order by UpdateDateTime desc",
               NHibernateUtility.GetMSSQL_SQL(typeof(DATProject), NHibernateUtility.TableName_DATProject),
               projectName, cityId, SYSCodeManager.STATECODE_4, SYSCodeManager.DATATYPECODE_1);
                IList<DATProject> list = db.DB.GetCustomSQLQueryList<DATProject>(sql, null).ToList();
                db.Close();
                return list;
            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }
        }

        /// <summary>
        /// 获取查勘中任务(查勘中)
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public static IList<DATProject> GetAllotSurveyingProject(string userName, int cityId, DataBase _db = null)
        {
            DataBase db = new DataBase(_db);
            try
            {
                string sql = string.Format("{0} Valid=1 and projectid in (select DatId from Dat_AllotFlow  where CityId={1} and StateCode ={2} and SurveyUserName='{3}' and DatType={4})  order by UpdateDateTime desc",
               NHibernateUtility.GetMSSQL_SQL(typeof(DATProject), NHibernateUtility.TableName_DATProject),
               cityId, SYSCodeManager.STATECODE_4, userName, SYSCodeManager.DATATYPECODE_1);
                IList<DATProject> list = db.DB.GetCustomSQLQueryList<DATProject>(sql, null).ToList();
                db.Close();
                return list;
            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }
        }

        /// <summary>
        /// 根据楼盘fxtprojectID、名称获取任务(未入库)
        /// </summary>
        /// <param name="fxtProjectID"></param>
        /// <param name="cityId"></param>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public static DATProject GetAllotNotStorageProject(int fxtProjectID, int cityId, string projectName, DataBase _db = null)
        {
            DataBase db = new DataBase(_db);
            try
            {
                string where = string.Empty;
                string whereValue = string.Empty;
                if (fxtProjectID > 0)
                {
                    where = " and FxtProjectId = ";
                    whereValue = fxtProjectID.ToString();
                }
                else if (!string.IsNullOrEmpty(projectName))
                {
                    where = " and projectName =  ";
                    whereValue = "'" + projectName + "'";
                }
                string sql = string.Format("{0} Valid=1  and  projectid in (select DatId from Dat_AllotFlow  where CityId={1} and StateCode <>{2} and DatType={3}) {4} {5}  order by UpdateDateTime desc",
               NHibernateUtility.GetMSSQL_SQL(typeof(DATProject), NHibernateUtility.TableName_DATProject),
               cityId, SYSCodeManager.STATECODE_10, SYSCodeManager.DATATYPECODE_1, where, whereValue);
                DATProject project = db.DB.GetCustomSQLQueryEntity<DATProject>(sql, null);
                db.Close();
                return project;
            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }
        }


        /// <summary>
        /// 根据多个楼盘ID获取楼盘信息
        /// </summary>
        /// <param name="projectIds"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static IList<DATProject> GetProjectByProjectIds(int[] projectIds, DataBase _db = null)
        {

            if (projectIds == null || projectIds.Length < 1)
            {
                return new List<DATProject>();
            }
            DataBase db = new DataBase(_db);
            try
            {
                string sql = "{0} ProjectId in (" + projectIds.ConvertToString() + ")  order by UpdateDateTime desc";
                sql = string.Format(sql, NHibernateUtility.GetMSSQL_SQL_NOLOCK(NHibernateUtility.TableName_DATProject));
                IList<DATProject> list = db.DB.GetCustomSQLQueryList<DATProject>(sql, null);
                db.Close();
                return list;
            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }
        }
        /// <summary>
        /// 根据楼盘ID获取楼盘信息
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="cityId"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static DATProject GetProjectByProjectId(int projectId, int cityId, DataBase _db = null)
        {
            DataBase db = new DataBase(_db);
            try
            {
                string sql = "{0} ProjectId =:projectId and CityID =:cityId and Valid=1";
                sql = string.Format(sql, NHibernateUtility.GetMSSQL_SQL_NOLOCK(NHibernateUtility.TableName_DATProject));
                List<NHParameter> parameters = new List<NHParameter>();
                parameters.Add(new NHParameter("projectId", projectId, NHibernateUtil.Int32));
                parameters.Add(new NHParameter("cityId", cityId, NHibernateUtil.Int32));
                DATProject datproject = db.DB.GetCustomSQLQueryEntity<DATProject>(sql, parameters);//.GetCustom<DATProject>((Expression<Func<DATProject, bool>>)(tbl => tbl.ProjectId == projectId && tbl.CityID == cityId&&tbl.Valid==1));
                db.Close();
                return datproject;
            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }
        }

        #endregion

        #region Excel
        /// <summary>
        /// 获取楼盘信息
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="cityId"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static DATProject GetProjectByProjectId(int projectId, DataBase _db = null)
        {
            DataBase db = new DataBase(_db);
            try
            {
                string sql = "{0} ProjectId =:projectId and Valid=1";
                sql = string.Format(sql, NHibernateUtility.GetMSSQL_SQL_NOLOCK(NHibernateUtility.TableName_DATProject));
                List<NHParameter> parameters = new List<NHParameter>();
                parameters.Add(new NHParameter("projectId", projectId, NHibernateUtil.Int32));
                DATProject datproject = db.DB.GetCustomSQLQueryEntity<DATProject>(sql, parameters);
                db.Close();
                return datproject;
            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }
        }
        /// <summary>
        /// 根据楼盘ID获取楼盘信息
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        //public static List<DATBuilding> GetBuildingByProjectId(int projectId, DataBase _db = null)
        //{ 

        //}
        #endregion

        #region 更新
        public static int SetAllotProjectInfo(long allotId, string userName, int fxtCompanyId, int cityId, decimal? allotflowX, decimal? allotflowY,
             string developersCompany, string managerCompany, JObject _project,
            //IList<LNKPAppendage> _lnkpaList, 
            JArray _buildingJoinHouseList
            , out string message, out long addAllotId, out int addProjectId, DataBase _db = null)
        {
            DateTime nowTime = DateTime.Now;
            addAllotId = allotId;
            addProjectId = 0;
            message = "";
            if (!CheckProjectObj(_project, out message))
            {
                return 0;
            }
            developersCompany = developersCompany.DecodeField().TrimBlank();
            managerCompany = managerCompany.DecodeField().TrimBlank();
            DataBase db = new DataBase(_db);
            DATProject project = null;
            DatAllotFlow allotFlow = null;
            bool addProject = false;
            if (allotId != 0)
            {
                allotFlow = DatAllotFlowManager.GetDatAllotFlowByIdAndUserIdAndCityId(allotId, userName, cityId, db);
                if (allotFlow == null)
                {
                    db.Close();
                    message = "此任务不存在,或已撤销";
                    return 0;
                }
                project = GetProjectByProjectId(Convert.ToInt32(allotFlow.DatId), cityId, db);
                if (project == null)
                {
                    db.Close();
                    message = "此任务不存在,或已撤销";
                    return 0;
                }
            }
            else
            {
                allotFlow = new DatAllotFlow();
                allotFlow.CityId = cityId;
                allotFlow.FxtCompanyId = fxtCompanyId;
                allotFlow.DatType = SYSCodeManager.DATATYPECODE_1;
                allotFlow.CreateTime = nowTime;
                allotFlow.UserName = userName;
                allotFlow.SurveyUserName = userName;
                allotFlow.Remark = "正式库调取或自行分配";
                addProject = true;
                project = new DATProject();
            }
            using (ITransaction tx = db.DB.BeginTransaction())
            {
                try
                {
                    //更新Project值
                    #region (Project更新值)
                    foreach (var _jobj in _project)
                    {
                        string key = _jobj.Key;
                        var property = project.GetType().GetProperties()
                                 .Where(pInfo => pInfo.Name.ToLower().Equals(key.ToLower())).FirstOrDefault();
                        if (property == null)
                        {
                            continue;
                        }
                        object _value = CommonUtility.valueType(property.PropertyType, _jobj.Value.Value<JValue>().Value, isDecode: true);
                        property.SetValue(project, _value, null);
                    }
                    project.Creator = userName;
                    ////上传任务：如果有图片，任务状态先不修改，等图片上传完后才修改。
                    //if (!project.PhotoCount.HasValue || project.PhotoCount.Value <= 0)
                    //{
                    project.Status = SYSCodeManager.STATECODE_5;

                    //}
                    project.CityID = cityId;
                    //project.PurposeCode = SYSCodeApi.PROJECT_PURPOSECODE1;
                    if (addProject)
                    {
                        project.FxtCompanyId = fxtCompanyId;
                        project.CreateTime = nowTime;//创建时间
                        project.Creator = userName;//创建人
                        project.Valid = 1;
                        db.DB.Create(project, tx);
                        allotFlow.DatId = project.ProjectId;
                    }
                    else
                    {
                        project.SaveDateTime = nowTime;//最后修改时间
                        project.SaveUser = userName;//修改人
                        db.DB.Update(project, tx);
                    }
                    addProjectId = project.ProjectId;
                    #endregion
                    //更新Project配套
                    #region (Project更新配套)
                    //停车状况
                    #region (停车状况)
                    int classcode = 0;
                    int.TryParse(_project.Value<string>("parkingstatus"), out classcode);
                    IList<LNKPAppendage> _lnkpaList = new List<LNKPAppendage>();
                    _lnkpaList.Add(new LNKPAppendage()
                    {
                        CityId = cityId,
                        ProjectId = addProjectId,
                        ClassCode = classcode,
                        AppendageCode = SYSCodeManager.APPENDAGECODE_14
                    });

                    if (_lnkpaList != null && _lnkpaList.Count > 0)
                    {
                        List<int> appendageCodes = new List<int>();
                        foreach (LNKPAppendage lnkp in _lnkpaList)
                        {
                            appendageCodes.Add(lnkp.AppendageCode);
                        }
                        IList<LNKPAppendage> lnkpaList = LNKPAppendageManager.GetLNKPAppendageByProjectIdAndAppendageCodes(cityId, project.ProjectId, appendageCodes.ToArray(), _db: db);
                        IList<LNKPAppendage> addlnkpaList = new List<LNKPAppendage>();
                        for (int i = 0; i < _lnkpaList.Count; i++)
                        {
                            LNKPAppendage _lnkpa = _lnkpaList[i];
                            LNKPAppendage lnkpa = lnkpaList.Where(obj => obj.AppendageCode == _lnkpa.AppendageCode).FirstOrDefault();
                            if (lnkpa == null)
                            {
                                _lnkpa.CityId = cityId;
                                _lnkpa.ProjectId = project.ProjectId;
                                _lnkpa.IsInner = true;
                                addlnkpaList.Add(_lnkpa);
                            }
                            else
                            {
                                lnkpa.P_AName = _lnkpa.P_AName;
                                lnkpa.ClassCode = _lnkpa.ClassCode;
                                lnkpa.ProjectId = project.ProjectId;
                                lnkpa.CityId = cityId;
                            }
                        }
                        if (addlnkpaList != null && addlnkpaList.Count > 0)
                        {
                            db.DB.Create<LNKPAppendage>(addlnkpaList, tx);
                        }
                        if (lnkpaList != null && lnkpaList.Count > 0)
                        {
                            db.DB.Update<LNKPAppendage>(lnkpaList, tx);
                        }

                    }
                    #endregion
                    if (allotflowX.HasValue && allotflowY.HasValue)
                    {
                        string loadcation = allotflowY.Value.ToString() + "," + allotflowX.Value.ToString();
                        //Action<string, int, int> func = UpDateAppendage;
                        //func.BeginInvoke(loadcation, addProjectId, cityId, new AsyncCallback(CallBackMethod), func);
                        //int pid = addProjectId;
                        //Task.Factory.StartNew(() => UpDateAppendage(loadcation, pid, cityId));
                    }

                    #endregion
                    //更新Project关联公司
                    #region (Project更新关联公司)
                    int[] companyTypes = new int[] { SYSCodeApi.COMPANYTYPECODE_1, SYSCodeApi.COMPANYTYPECODE_4 };
                    IList<LNKPCompany> lnkpcList = LNKPCompanyManager.GetLNKPCompanyByCompanyTypes(cityId, project.ProjectId, companyTypes, _db: db);
                    IList<LNKPCompany> addlnkpcList = new List<LNKPCompany>();
                    //更新开发商
                    if (!string.IsNullOrEmpty(developersCompany))
                    {
                        LNKPCompany lnkpc = lnkpcList.Where(obj => obj.LNKPCompanyPX.CompanyType == SYSCodeApi.COMPANYTYPECODE_1).FirstOrDefault();
                        if (lnkpc == null)
                        {
                            lnkpc = new LNKPCompany
                            {
                                CompanyName = developersCompany,
                                LNKPCompanyPX = new ProjectPKCompanyTypePKCity()
                                {
                                    CompanyType = SYSCodeApi.COMPANYTYPECODE_1,
                                    CityId = cityId,
                                    ProjectId = project.ProjectId
                                }
                            };
                            addlnkpcList.Add(lnkpc);
                        }
                        else
                        {
                            lnkpc.CompanyName = developersCompany;
                        }
                    }
                    //更新物业管理公司
                    if (!string.IsNullOrEmpty(managerCompany))
                    {
                        LNKPCompany lnkpc2 = lnkpcList.Where(obj => obj.LNKPCompanyPX.CompanyType == SYSCodeApi.COMPANYTYPECODE_4).FirstOrDefault();
                        if (lnkpc2 == null)
                        {
                            lnkpc2 = new LNKPCompany
                            {
                                CompanyName = managerCompany,
                                LNKPCompanyPX = new ProjectPKCompanyTypePKCity()
                                {
                                    CompanyType = SYSCodeApi.COMPANYTYPECODE_4,
                                    CityId = cityId,
                                    ProjectId = project.ProjectId
                                }
                            };
                            addlnkpcList.Add(lnkpc2);
                        }
                        else
                        {
                            lnkpc2.CompanyName = managerCompany;
                        }
                    }
                    if (addlnkpcList != null && addlnkpcList.Count > 0)
                    {
                        db.DB.Create<LNKPCompany>(addlnkpcList, tx);
                    }
                    if (lnkpcList != null && lnkpcList.Count > 0)
                    {
                        db.DB.Update<LNKPCompany>(lnkpcList, tx);
                    }
                    #endregion

                    //更新Building值
                    #region(Building更新值)
                    List<int> existsBuildingIds = new List<int>();
                    int houseCountByProject = 0;//统计楼盘中总户数
                    int buildingCountByProject = 0;//统计楼盘中总栋数
                    if (_buildingJoinHouseList != null && _buildingJoinHouseList.Count > 0)
                    {
                        foreach (var arry in _buildingJoinHouseList)
                        {
                            buildingCountByProject = buildingCountByProject + 1;//统计楼栋数
                            JObject jobj = arry as JObject;
                            //验证楼栋字段数据
                            if (!CheckBuildingObj(jobj, out message))
                            {
                                tx.Rollback();
                                db.Close();
                                return 0;
                            }
                            int buildingId = Convert.ToInt32(jobj.Value<JValue>("buildingid").Value);
                            #region (设置building)
                            DATBuilding building = null;
                            //如果为修改
                            if (buildingId != 0)
                            {
                                building = DATBuildingManager.GetBuildingByBuildingId(buildingId, cityId, _db: db);
                                if (building == null)
                                {
                                    building = new DATBuilding();
                                    buildingId = 0;
                                    building.CreateTime = DateTime.Now;
                                    building.Creator = userName;
                                }
                                else
                                {
                                    building.SaveDateTime = DateTime.Now;
                                    building.SaveUser = userName;
                                }
                            }
                            else //如果为新增
                            {
                                building = new DATBuilding();
                                building.CreateTime = DateTime.Now;
                                building.Creator = userName;
                            }
                            foreach (var _prop in jobj)
                            {
                                string key = _prop.Key;
                                var property = building.GetType().GetProperties()
                                         .Where(pInfo => pInfo.Name.ToLower().Equals(key.ToLower())).FirstOrDefault();
                                if (property == null)
                                {
                                    continue;
                                }
                                if (key == "builddate")
                                {
                                    var v = _prop.Value.Value<JValue>().Value;
                                    if (v.ToString() == "null")
                                    {
                                        continue;
                                    }
                                }
                                object _value = CommonUtility.valueType(property.PropertyType, _prop.Value.Value<JValue>().Value, true);
                                property.SetValue(building, _value, null);
                            }
                            //给building各属性赋值
                            building.Valid = 1;
                            building.ProjectId = project.ProjectId;
                            building.Status = SYSCodeManager.STATECODE_5;
                            building.FxtCompanyId = allotFlow.FxtCompanyId;
                            building.CityID = cityId;
                            //提交新增或修改building
                            if (buildingId == 0)
                            {
                                db.DB.Create(building, tx);
                            }
                            else
                            {
                                db.DB.Update(building, tx);
                            }
                            buildingId = building.BuildingId;
                            #endregion
                            #region (设置house)
                            JArray houseArray = jobj["houselist"] as JArray;
                            List<int> houserIds = new List<int>();
                            //获取ID
                            foreach (var array in houseArray)
                            {
                                JObject jobj2 = (JObject)array;
                                int houserId = Convert.ToInt32(jobj2.Value<JValue>("houseid").Value);
                                if (houserId != 0)
                                {
                                    houserIds.Add(houserId);
                                }
                            }
                            //获取楼栋下的原房号数据
                            IList<DATHouse> houseList = DATHouseManager.GetHouseByHouseIds(houserIds.ToArray(), cityId, _db: db);
                            IList<DATHouse> addhouseList = new List<DATHouse>();
                            IList<DATHouse> delhouseList = new List<DATHouse>();
                            //设置新增和修改实体
                            #region
                            int houseCount = 0;//统计当前楼栋的房号数量
                            foreach (var array in houseArray)
                            {
                                houseCount = houseCount + 1;
                                JObject jobj2 = (JObject)array;

                                //验证房号字段数据
                                if (!CheckHouseObj(jobj2, out message))
                                {
                                    tx.Rollback();
                                    db.Close();
                                    return 0;
                                }
                                int houserId = Convert.ToInt32(jobj2.Value<JValue>("houseid").Value);
                                string houseno = Convert.ToString(jobj2.Value<JValue>("houseno").Value).DecodeField();
                                DATHouse house = houseList.Where(obj => obj.HouseId == houserId).FirstOrDefault();
                                bool isadd = false;
                                if (house == null)
                                {
                                    isadd = true;
                                    house = new DATHouse();
                                }
                                foreach (var _prop in jobj2)
                                {
                                    string key = _prop.Key;
                                    var property = house.GetType().GetProperties().Where(pInfo => pInfo.Name.ToLower().Equals(key.ToLower())).FirstOrDefault();
                                    if (property == null)
                                    {
                                        continue;
                                    }
                                    object _value = CommonUtility.valueType(property.PropertyType, _prop.Value.Value<JValue>().Value, true);
                                    property.SetValue(house, _value, null);
                                }
                                //给house各属性赋值
                                house.BuildingId = buildingId;
                                house.Valid = 1;
                                house.Status = SYSCodeManager.STATECODE_5;
                                house.FxtCompanyId = allotFlow.FxtCompanyId;
                                house.CityID = cityId;
                                house.CreateTime = DateTime.Now;
                                house.Creator = userName;
                                if (string.IsNullOrEmpty(house.UnitNo) && string.IsNullOrEmpty(houseno))
                                {
                                    house.UnitNo = house.HouseName.Replace(house.FloorNo.ToString(), "$");
                                }
                                else
                                {
                                    house.UnitNo = DATHouseManager.SetHouseUnitNoAndHouseNo(house.UnitNo, houseno);
                                }
                                //设置新增实体
                                if (isadd)
                                {
                                    addhouseList.Add(house);
                                }
                            }
                            #endregion
                            //设置删除实体
                            if (houseList != null && houseList.Count > 0)
                            {
                                db.DB.Update<DATHouse>(houseList, tx);
                            }
                            if (addhouseList != null && addhouseList.Count > 0)
                            {
                                db.DB.Create<DATHouse>(addhouseList, tx);
                            }
                            List<DATHouse> delHouse = new List<DATHouse>();
                            delHouse.AddRange(houseList);
                            delHouse.AddRange(addhouseList);
                            delhouseList = delHouse;
                            DATHouseManager.DeleteByNotHouseIds(delhouseList, buildingId, cityId, _db: db, transaction: tx);
                            //更新楼栋总户数
                            //if (Convert.ToInt32(building.TotalNumber) < houseCount)
                            //{
                            //    building.TotalNumber = houseCount;
                            //    db.DB.Update(building, tx);
                            //}
                            //building.TotalNumber = houseCount;
                            //db.DB.Update(building, tx);

                            houseCountByProject = houseCountByProject + houseCount;//统计楼盘总户数
                            #endregion
                            existsBuildingIds.Add(buildingId);
                        }
                        //删除不要的楼栋信息
                        DATBuildingManager.DeleteByNotBuildingIds(existsBuildingIds.ToArray(), project.ProjectId, cityId, _db: db, transaction: tx);
                    }
                    #endregion
                    //统计楼盘中的总户数和总栋数
                    //if (Convert.ToInt32(project.BuildingNum) < buildingCountByProject || Convert.ToInt32(project.TotalNum) < houseCountByProject)
                    //{
                    //    if (Convert.ToInt32(project.BuildingNum) < buildingCountByProject)
                    //    {
                    //        project.BuildingNum = buildingCountByProject;
                    //    }
                    //    if (Convert.ToInt32(project.TotalNum) < houseCountByProject)
                    //    {
                    //        project.TotalNum = houseCountByProject;
                    //    }
                    //    db.DB.Update(project, tx);
                    //}
                    //project.BuildingNum = buildingCountByProject;
                    //project.TotalNum = houseCountByProject;
                    //db.DB.Update(project, tx);

                    //任务表更新值
                    ////上传任务：如果有图片，任务状态先不修改，等图片上传完后才修改。
                    //if (!project.PhotoCount.HasValue || project.PhotoCount.Value <= 0)
                    //{
                    allotFlow.StateCode = SYSCodeManager.STATECODE_5;
                    //    //状态记录表插入值
                    //    DatAllotSurveyManager.InsertAllotSurvey(allotFlow.id, allotFlow.CityId, allotFlow.FxtCompanyId, userName, SYSCodeManager.STATECODE_5, DateTime.Now, db, tx);
                    //}
                    allotFlow.StateDate = nowTime;
                    allotFlow.X = allotflowX;
                    allotFlow.Y = allotflowY;
                    if (!addProject)
                    {
                        db.DB.Update(allotFlow, tx);
                    }
                    else
                    {
                        db.DB.Create(allotFlow, tx);
                        addAllotId = allotFlow.id;
                    }

                    DatAllotSurveyManager.InsertAllotSurvey(allotFlow.id, allotFlow.CityId, allotFlow.FxtCompanyId, userName, SYSCodeManager.STATECODE_5, DateTime.Now, db, tx);

                    tx.Commit();
                }
                catch (Exception ex)
                {
                    tx.Rollback();
                    log.Error("提交任务系统异常", ex);
                    message = "提交任务系统异常";
                    db.Close();
                    return -1;
                }
            }
            db.Close();
            return 1;

        }

        //回调方法
        static void CallBackMethod(IAsyncResult ar)
        {
            AsyncResult result = (AsyncResult)ar;
            Action<string, int, int> a = (Action<string, int, int>)result.AsyncDelegate;
            a.EndInvoke(ar);
        }

        /// <summary>
        /// 更新配套设施
        /// </summary>
        /// <param name="xy"></param>
        /// <param name="addProjectId"></param>
        /// <param name="cityId"></param>
        public static void UpDateAppendage(string xy, int addProjectId, int cityId)
        {
            DataBase db = new DataBase();
            try
            {
                string loadcation = xy;
                IList<LNKPAppendage> _lnkpaList = BaiduApiManager.GetAllAppendage(loadcation, addProjectId);//.ToJSONjss().ParseJSONList<LNKPAppendage>();
                if (_lnkpaList != null && _lnkpaList.Count > 0)
                {
                    List<int> appendageCodes = new List<int>();
                    foreach (LNKPAppendage lnkp in _lnkpaList)
                    {
                        appendageCodes.Add(lnkp.AppendageCode);
                    }
                    IList<LNKPAppendage> lnkpaList = LNKPAppendageManager.GetLNKPAppendageByProjectIdAndAppendageCodes(cityId, addProjectId, appendageCodes.ToArray(), _db: db);
                    IList<LNKPAppendage> addlnkpaList = new List<LNKPAppendage>();
                    for (int i = 0; i < _lnkpaList.Count; i++)
                    {
                        LNKPAppendage _lnkpa = _lnkpaList[i];
                        LNKPAppendage lnkpa = lnkpaList.Where(obj => obj.AppendageCode == _lnkpa.AppendageCode).FirstOrDefault();
                        if (lnkpa == null)
                        {
                            if (_lnkpa.CityId == 0)
                            {
                                _lnkpa.CityId = cityId;
                            }
                            _lnkpa.ProjectId = addProjectId;
                            _lnkpa.IsInner = true;
                            addlnkpaList.Add(_lnkpa);
                        }
                        else
                        {

                            lnkpa.P_AName = _lnkpa.P_AName;
                            lnkpa.ClassCode = _lnkpa.ClassCode;
                            lnkpa.ProjectId = addProjectId;
                            if (lnkpa.CityId == 0)
                            {
                                lnkpa.CityId = cityId;
                            }
                        }
                    }
                    if (addlnkpaList != null && addlnkpaList.Count > 0)
                    {
                        db.DB.Create<LNKPAppendage>(addlnkpaList);
                    }
                    if (lnkpaList != null && lnkpaList.Count > 0)
                    {
                        db.DB.Update<LNKPAppendage>(lnkpaList);
                    }

                }
                db.Close();
            }
            catch (Exception ex)
            {
                db.Close();
                log.Error(ex);
            }

        }

        #endregion

        public static int UpdateProjectInfo(int projectId, int cityId, string userName, JObject projectObj, IList<LNKPAppendage> _lnkpaList, string developersCompany, string managerCompany, out string message, DataBase _db = null)
        {

            DataBase db = new DataBase(_db);
            message = "";
            DateTime nowTime = DateTime.Now;
            DATProject project = GetProjectByProjectId(projectId, cityId, db);
            if (project == null)
            {
                db.Close();
                message = "楼盘不存在或已被删除";
                return 0;
            }
            try
            {
                //更新Project值
                #region (Project更新值)
                foreach (var _jobj in projectObj)
                {
                    string key = _jobj.Key;
                    var property = project.GetType().GetProperties()
                             .Where(pInfo => pInfo.Name.ToLower().Equals(key.ToLower())).FirstOrDefault();
                    if (property == null)
                    {
                        continue;
                    }
                    object _value = CommonUtility.valueType(property.PropertyType, _jobj.Value.Value<JValue>().Value, isDecode: true);
                    property.SetValue(project, _value, null);
                }
                project.SaveDateTime = nowTime;//最后修改时间
                project.SaveUser = userName;//修改人
                db.DB.Update(project);
                #endregion
                //更新Project配套
                #region (Project更新配套)
                if (_lnkpaList != null && _lnkpaList.Count > 0)
                {
                    List<int> appendageCodes = new List<int>();
                    foreach (LNKPAppendage lnkp in _lnkpaList)
                    {
                        appendageCodes.Add(lnkp.AppendageCode);
                    }
                    IList<LNKPAppendage> lnkpaList = LNKPAppendageManager.GetLNKPAppendageByProjectIdAndAppendageCodes(cityId, project.ProjectId, appendageCodes.ToArray(), _db: db);
                    IList<LNKPAppendage> addlnkpaList = new List<LNKPAppendage>();
                    for (int i = 0; i < _lnkpaList.Count; i++)
                    {
                        LNKPAppendage _lnkpa = _lnkpaList[i];
                        LNKPAppendage lnkpa = lnkpaList.Where(obj => obj.AppendageCode == _lnkpa.AppendageCode).FirstOrDefault();
                        if (lnkpa == null)
                        {
                            _lnkpa.CityId = cityId;
                            _lnkpa.ProjectId = project.ProjectId;
                            _lnkpa.IsInner = true;
                            addlnkpaList.Add(_lnkpa);
                        }
                        else
                        {
                            lnkpa.P_AName = _lnkpa.P_AName;
                            lnkpa.ClassCode = _lnkpa.ClassCode;
                            lnkpa.ProjectId = project.ProjectId;
                            lnkpa.CityId = cityId;
                        }
                    }
                    if (addlnkpaList != null && addlnkpaList.Count > 0)
                    {
                        db.DB.Create<LNKPAppendage>(addlnkpaList);
                    }
                    if (lnkpaList != null && lnkpaList.Count > 0)
                    {
                        db.DB.Update<LNKPAppendage>(lnkpaList);
                    }

                }
                #endregion
                //更新Project关联公司
                #region (Project更新关联公司)
                int[] companyTypes = new int[] { SYSCodeApi.COMPANYTYPECODE_1, SYSCodeApi.COMPANYTYPECODE_4 };
                IList<LNKPCompany> lnkpcList = LNKPCompanyManager.GetLNKPCompanyByCompanyTypes(cityId, project.ProjectId, companyTypes, _db: db);
                IList<LNKPCompany> addlnkpcList = new List<LNKPCompany>();
                //更新开发商
                if (!string.IsNullOrEmpty(developersCompany))
                {
                    LNKPCompany lnkpc = lnkpcList.Where(obj => obj.LNKPCompanyPX.CompanyType == SYSCodeApi.COMPANYTYPECODE_1).FirstOrDefault();
                    if (lnkpc == null)
                    {
                        lnkpc = new LNKPCompany
                        {
                            CompanyName = developersCompany,
                            LNKPCompanyPX = new ProjectPKCompanyTypePKCity()
                            {
                                CompanyType = SYSCodeApi.COMPANYTYPECODE_1,
                                CityId = cityId,
                                ProjectId = project.ProjectId
                            }
                        };
                        addlnkpcList.Add(lnkpc);
                    }
                    else
                    {
                        lnkpc.CompanyName = developersCompany;
                    }
                }
                //更新物业管理公司
                if (!string.IsNullOrEmpty(managerCompany))
                {
                    LNKPCompany lnkpc2 = lnkpcList.Where(obj => obj.LNKPCompanyPX.CompanyType == SYSCodeApi.COMPANYTYPECODE_4).FirstOrDefault();
                    if (lnkpc2 == null)
                    {
                        lnkpc2 = new LNKPCompany
                        {
                            CompanyName = managerCompany,
                            LNKPCompanyPX = new ProjectPKCompanyTypePKCity()
                            {
                                CompanyType = SYSCodeApi.COMPANYTYPECODE_4,
                                CityId = cityId,
                                ProjectId = project.ProjectId
                            }
                        };
                        addlnkpcList.Add(lnkpc2);
                    }
                    else
                    {
                        lnkpc2.CompanyName = managerCompany;
                    }
                }
                if (addlnkpcList != null && addlnkpcList.Count > 0)
                {
                    db.DB.Create<LNKPCompany>(addlnkpcList);
                }
                if (lnkpcList != null && lnkpcList.Count > 0)
                {
                    db.DB.Update<LNKPCompany>(lnkpcList);
                }
                #endregion
            }
            catch (Exception ex)
            {
                db.Close();
                message = "系统异常";
                log.Error("修改楼盘时失败,projectid:" + projectId, ex);
                return -1;
            }
            db.Close();
            return 1;
        }

        #region common
        public static bool CheckProjectObj(JObject jobj, out string message)
        {
            message = "";
            DATProject project = new DATProject();
            project.ProjectId.GetType();
            //
            object allotflowx = jobj["allotflowx"] == null || string.IsNullOrEmpty(Convert.ToString(jobj.Value<JValue>("allotflowx").Value)) ? null : jobj.Value<JValue>("allotflowx").Value;
            object allotflowy = jobj["allotflowy"] == null || string.IsNullOrEmpty(Convert.ToString(jobj.Value<JValue>("allotflowy").Value)) ? null : jobj.Value<JValue>("allotflowy").Value;
            //if (allotflowx == null || allotflowy == null)
            //{
            //    message = "请输入查勘员经度和纬度";
            //    return false;
            //}
            if (!StringHelp.CheckDecimal(Convert.ToString(allotflowx)) || !StringHelp.CheckDecimal(Convert.ToString(allotflowy)))
            {
                message = "请输入正确的查勘员经度和纬度";
                return false;
            }
            //
            string projectname = jobj["projectname"] == null || jobj.Value<string>("projectname") == "" ? null : jobj.Value<string>("projectname").DecodeField();
            if (string.IsNullOrEmpty(projectname))
            {
                message = "请输入楼盘名";
                return false;
            }
            //
            object cityid = jobj["cityid"] == null || string.IsNullOrEmpty(Convert.ToString(jobj.Value<JValue>("cityid").Value)) ? null : jobj.Value<JValue>("cityid").Value;
            object areaid = jobj["areaid"] == null || string.IsNullOrEmpty(Convert.ToString(jobj.Value<JValue>("areaid").Value)) ? null : jobj.Value<JValue>("areaid").Value;
            if (cityid == null || !StringHelp.CheckInteger(cityid.ToString()) || areaid == null || !StringHelp.CheckInteger(areaid.ToString()))
            {
                message = "请正确输入城市和行政区";
                return false;
            }
            //
            string address = jobj["address"] == null ? null : jobj.Value<string>("address").DecodeField();
            //if (string.IsNullOrEmpty(address))
            //{
            //    message = "请输入楼盘地址";
            //    return false;
            //}
            //
            object enddate = jobj["enddate"] == null || jobj.Value<string>("enddate") == "" ? null : jobj.Value<JValue>("enddate").Value;
            if (enddate != null && !StringHelp.CheckIsDate(enddate.ToString().DecodeField())) //(enddate == null || !StringHelp.CheckIsDate(enddate.ToString().DecodeField()))
            {
                message = "请输入正确的竣工时间";
                return false;
            }
            //
            object buildingarea = jobj["buildingarea"] == null || jobj.Value<string>("buildingarea") == "" ? null : jobj.Value<JValue>("buildingarea").Value;
            if (buildingarea != null && !StringHelp.CheckDecimal(buildingarea.ToString().DecodeField()))//(buildingarea == null || !StringHelp.CheckDecimal(buildingarea.ToString().DecodeField()))
            {
                message = "请输入正确的建筑面积";
                return false;
            }
            //
            object landarea = jobj["landarea"] == null || jobj.Value<string>("landarea") == "" ? null : jobj.Value<JValue>("landarea").Value;
            if (landarea != null && !StringHelp.CheckDecimal(landarea.ToString().DecodeField()))//(landarea == null || !StringHelp.CheckDecimal(landarea.ToString().DecodeField()))
            {
                message = "请输入正确的占地面积";
                return false;
            }
            //
            object cubagerate = jobj["cubagerate"] == null || jobj.Value<string>("cubagerate") == "" ? null : jobj.Value<JValue>("cubagerate").Value;
            if (cubagerate != null && !StringHelp.CheckDecimal(cubagerate.ToString().DecodeField()))//(cubagerate == null || !StringHelp.CheckDecimal(cubagerate.ToString().DecodeField()))
            {
                message = "请输入正确的容积率值";
                return false;
            }
            //
            object greenrate = jobj["greenrate"] == null || jobj.Value<string>("greenrate") == "" ? null : jobj.Value<JValue>("greenrate").Value;
            if (greenrate != null && !StringHelp.CheckDecimal(greenrate.ToString().DecodeField()))//(greenrate == null || !StringHelp.CheckDecimal(greenrate.ToString().DecodeField()))
            {
                message = "请输入正确的绿化率值";
                return false;
            }
            //
            string managerprice = jobj["managerprice"] == null ? null : jobj.Value<string>("managerprice").DecodeField();
            //if (string.IsNullOrEmpty(managerprice))
            //{
            //    message = "请输入物业管理费";
            //    return false;
            //}
            //
            object parkingnumber = jobj["parkingnumber"] == null || jobj.Value<string>("parkingnumber") == "" ? null : jobj.Value<JValue>("parkingnumber").Value;
            if (parkingnumber != null && !StringHelp.CheckInteger(parkingnumber.ToString().DecodeField()))//(parkingnumber == null || !StringHelp.CheckInteger(parkingnumber.ToString().DecodeField()))
            {
                message = "请输入车位数量";
                return false;
            }
            //
            object totalnum = jobj["totalnum"] == null || jobj.Value<string>("totalnum") == "" ? null : jobj.Value<JValue>("totalnum").Value;
            if (totalnum != null && !StringHelp.CheckInteger(totalnum.ToString().DecodeField()))//(totalnum == null || !StringHelp.CheckInteger(totalnum.ToString().DecodeField()))
            {
                message = "请输入总户数";
                return false;
            }
            //
            object buildingnum = jobj["buildingnum"] == null || jobj.Value<string>("buildingnum") == "" ? null : jobj.Value<JValue>("buildingnum").Value;
            if (buildingnum != null && !StringHelp.CheckInteger(buildingnum.ToString().DecodeField()))//(buildingnum == null || !StringHelp.CheckInteger(buildingnum.ToString().DecodeField()))
            {
                message = "请输入总栋数";
                return false;
            }
            //
            object saledate = jobj["saledate"] == null || jobj.Value<string>("saledate") == "" ? null : jobj.Value<JValue>("saledate").Value;
            if (saledate != null && !StringHelp.CheckIsDate(saledate.ToString().DecodeField()))//(saledate == null || !StringHelp.CheckIsDate(saledate.ToString().DecodeField()))
            {
                message = "请输入开盘时间";
                return false;
            }
            //
            object buildingdate = jobj["buildingdate"] == null || jobj.Value<string>("buildingdate") == "" ? null : jobj.Value<JValue>("buildingdate").Value;
            if (buildingdate != null && !StringHelp.CheckIsDate(buildingdate.ToString()))//(buildingdate == null || !StringHelp.CheckIsDate(buildingdate.ToString()))
            {
                message = "请输入开工时间";
                return false;
            }
            //
            object statedate = jobj["statedate"] == null || jobj.Value<string>("statedate") == "" ? null : jobj.Value<JValue>("statedate").Value;
            if (statedate != null && !StringHelp.CheckIsDate(statedate.ToString().DecodeField()))//(statedate == null || !StringHelp.CheckIsDate(statedate.ToString().DecodeField()))
            {
                message = "请输入采集时间";
                return false;
            }
            return true;
        }
        public static bool CheckBuildingObj(JObject jobj, out string message)
        {
            message = "";
            //
            string buildingname = jobj["buildingname"] == null ? null : jobj.Value<string>("buildingname").DecodeField();
            if (string.IsNullOrEmpty(buildingname))
            {
                message = "请输入楼栋名称";
                return false;
            }
            if (buildingname.Length > 150)
            {
                message = "请输入楼栋名称长度不能大于150";
                return false;
            }
            //
            string doorplate = jobj["doorplate"] == null ? null : jobj.Value<string>("doorplate").DecodeField();
            if (doorplate != null && doorplate.Length > 200)
            {
                message = "请输入门牌号长度不能大于200";
                return false;
            }
            //
            string othername = jobj["othername"] == null ? null : jobj.Value<string>("othername").DecodeField();
            if (othername != null && othername.Length > 50)
            {
                message = "请输入楼栋别名长度不能大于50";
                return false;
            }
            //
            object structurecode = jobj["structurecode"] == null ? null : jobj.Value<JValue>("structurecode").Value;
            if (structurecode != null && !StringHelp.CheckInteger(structurecode.ToString().DecodeField()))
            {
                message = "请输入正确的建筑结构";
                return false;
            }
            //
            object locationcode = jobj["locationcode"] == null || jobj.Value<string>("locationcode") == "" ? null : jobj.Value<JValue>("locationcode").Value;
            if (locationcode != null && !StringHelp.CheckInteger(locationcode.ToString().DecodeField()))
            {
                message = "请输入正确的位置";
                return false;
            }
            //
            object averageprice = jobj["averageprice"] == null || jobj.Value<string>("averageprice") == "" ? null : jobj.Value<JValue>("averageprice").Value;
            if (averageprice != null && !StringHelp.CheckDecimal(averageprice.ToString().DecodeField()))
            {
                message = "请输入正确的楼栋均价";
                return false;
            }
            //
            object builddate = jobj["builddate"] == null || jobj.Value<string>("builddate") == "" || jobj.Value<string>("builddate") == "null" ? null : jobj.Value<JValue>("builddate").Value;
            if (builddate != null && !StringHelp.CheckIsDate(builddate.ToString().DecodeField()))
            {
                message = "请输入正确的楼栋竣工时间(建筑时间)";
                return false;
            }
            //
            object iselevator = jobj["iselevator"] == null || jobj.Value<string>("iselevator") == "" ? null : jobj.Value<JValue>("iselevator").Value;
            if (iselevator != null && !StringHelp.CheckInteger(iselevator.ToString().DecodeField()))
            {
                message = "请输入正确的楼栋是否带电梯";
                return false;
            }
            //
            string elevatorrate = jobj["elevatorrate"] == null || jobj.Value<string>("elevatorrate") == "" ? null : jobj.Value<string>("elevatorrate").DecodeField();
            if (elevatorrate != null && elevatorrate.Length > 50)
            {
                message = "请输入楼栋梯户比字符不能大于50";
                return false;
            }
            //
            string pricedetail = jobj["pricedetail"] == null ? null : jobj.Value<string>("pricedetail").DecodeField();
            if (pricedetail != null && pricedetail.Length > 500)
            {
                message = "请输入价格系数说明字符不能大于500";
                return false;
            }
            //
            object sightcode = jobj["sightcode"] == null ? null : jobj.Value<JValue>("sightcode").Value;
            if (sightcode != null && !StringHelp.CheckInteger(sightcode.ToString().DecodeField()))
            {
                message = "请输入正确的景观值";
                return false;
            }
            //
            object totalfloor = jobj["totalfloor"] == null ? null : jobj.Value<JValue>("totalfloor").Value;
            if (totalfloor == null || !StringHelp.CheckInteger(totalfloor.ToString().DecodeField()))
            {
                message = "请输入楼栋总层数";
                return false;
            }
            return true;
        }

        public static bool CheckHouseObj(JObject jobj, out string message)
        {

            message = "";
            //
            string unitno = jobj["unitno"] == null || jobj.Value<string>("unitno") == "" ? null : jobj.Value<string>("unitno").DecodeField();
            if (unitno != null && unitno.Length > 20)
            {
                message = "请输入房号单元名称字符长度不能大于20";
                return false;
            }
            //
            object floorno = jobj["floorno"] == null ? null : jobj.Value<JValue>("floorno").Value;
            int fno = 0;
            if (floorno == null || !int.TryParse(floorno.ToString().DecodeField(), out fno))
            {
                message = "请输入起始楼层";
                return false;
            }
            //
            //string housename = jobj["housename"] == null ? null : jobj.Value<string>("housename").DecodeField();
            //if (housename == null)
            //{
            //    message = "请输入房号";
            //    return false;
            //}
            //if (housename.Length > 20)
            //{
            //    message = "请输入房号字符长度不能大于20";
            //    return false;
            //}
            //
            object frontcode = jobj["frontcode"] == null ? null : jobj.Value<JValue>("frontcode").Value;
            if (frontcode == null || !StringHelp.CheckInteger(frontcode.ToString().DecodeField()))
            {
                message = "请输入朝向";
                return false;
            }
            //
            object buildarea = jobj["buildarea"] == null || jobj.Value<JValue>("buildarea").Value.ToString() == "" ? null : jobj.Value<JValue>("buildarea").Value;
            if (buildarea != null && !StringHelp.CheckDecimal(buildarea.ToString().DecodeField()))
            {
                message = "请输入正确的面积格式";
                return false;
            }
            //
            object housetypecode = jobj["housetypecode"] == null || jobj.Value<JValue>("housetypecode").Value.ToString() == "" ? null : jobj.Value<JValue>("housetypecode").Value;
            if (housetypecode != null && !StringHelp.CheckInteger(housetypecode.ToString().DecodeField()))
            {
                message = "请输入正确的户型";
                return false;
            }
            //
            object sightcode = jobj["sightcode"] == null ? null : jobj.Value<JValue>("sightcode").Value;
            if (sightcode != null && !StringHelp.CheckInteger(sightcode.ToString().DecodeField()))
            {
                message = "请输入景观";
                return false;
            }
            return true;
        }
        public static bool CheckAppendage(LNKPAppendage lnkpa, out string message)
        {
            message = "";
            if (lnkpa == null)
                return true;
            if (string.IsNullOrEmpty(lnkpa.P_AName))
            {
                message = "请填写配套信息";
                return false;
            }
            return true;
        }

        #endregion


    }
}
