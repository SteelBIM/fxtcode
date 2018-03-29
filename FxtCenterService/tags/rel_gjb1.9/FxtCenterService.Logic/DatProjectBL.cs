using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using FxtCenterService.DataAccess;
using System.Data;
using System.Collections;
using CAS.Common;

namespace FxtCenterService.Logic
{
    public class DatProjectBL
    {
        /// <summary>
        /// 房讯通的companyId
        /// </summary>
        public const int FXTCOMPANYID = 25;
        #region (照片类型Code(ID:2009))
        /// <summary>
        /// 照片类型-其他
        /// </summary>
        public const int PHOTOTYPECODE_9 = 2009009;
        #endregion

        /// <summary>
        /// 获取楼盘列表
        /// </summary>
        public static List<DATProject> GetDATProjectList(SearchBase search, string key, int areaid, int buildingtypecode, int purposecode)
        {
            return DatProjectDA.GetDATProjectList(search, key, areaid, buildingtypecode, purposecode);
        }
        /// <summary>
        /// 获取楼盘下拉列表
        /// </summary>
        public static List<Dictionary<string, object>> GetProjectDropDownList(SearchBase search, string strKey, int items)
        {
            List<Dictionary<string, object>> listResult = new List<Dictionary<string, object>>();

            DataTable dt = null;
            string condition = "", key, param;
            condition = " and ([PinYin] like @strKey or [OtherName] like @strKey "
                //+"or [Address] like @strKey " //不再检索地址，BUG #2198
                + "or [ProjectName] like @strKey  or [PinYinAll] like @strKey )";
            key ="%"+strKey + "%";
            search.Top = items;
            dt = DatProjectDA.GetProjectDropDownList(search, condition, key, null);
            listResult = JSONHelper.DataTableToList(dt);
            if (listResult.Count < items)
            {
                condition = @" and (([PinYin] like @param) or ([Address] like @param) or ([ProjectName] like @param) or ([OtherName] like @param) or ([PinYinAll] like @param) or (PinYin like @strKey))
 and [Address] not like @strKey and [PinYin] not like @strKey and [OtherName] not like @strKey and [PinYinAll] not like @strKey and [ProjectName] not like @strKey and PinYin not like @strKey";
                param = "%" + strKey + "%";
                search.Top = items - listResult.Count;
                dt = DatProjectDA.GetProjectDropDownList(search, condition, key, param);
                listResult.AddRange(JSONHelper.DataTableToList(dt));
            }

            return listResult;
        }
        public static DATProject GetDATProjectByPK(int id)
        {
            return DatProjectDA.GetDATProjectByPK(id);
        }

        /// <summary>
        /// 获得数据中心的楼盘信息，没有联合附表
        /// </summary>
        /// <param name="cityid"></param>
        /// <param name="projectid"></param>
        /// <returns></returns>
        public static DATProject GetProjectInfoById(int cityid, int projectid,int fxtcompanyid) 
        {
            return DatProjectDA.GetProjectInfoById(cityid, projectid,fxtcompanyid); 
        }

        /// <summary>
        /// 获得数据中心的楼盘图片，没有联合附表
        /// </summary>
        /// <param name="cityid"></param>
        /// <param name="projectid"></param>
        /// <returns></returns>
        public static List<LNKPPhoto> GetProjectPhotoById(int cityid, int projectid, int fxtcompanyid) 
        {
            return DatProjectDA.GetProjectPhotoById( cityid, projectid, fxtcompanyid);
        }

         /// <summary>
        /// 楼盘案例
        /// </summary>
        /// <param name="search"></param>
        /// <param name="projectid"></param>
        /// <returns></returns>
        public static DataTable GetProjectCase(SearchBase search, int projectid, int fxtcompanyid,int cityid) 
        {
            return DatProjectDA.GetProjectCase(search,projectid,fxtcompanyid,cityid);
        }

        /// <summary>
        /// 获取自动估价楼盘信息-返回是否可估
        /// </summary>
        /// <param name="search"></param>
        /// <param name="fxtcompanyid"></param>
        /// <param name="cityid"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetSearchProjectListByKey(SearchBase search, int fxtcompanyid, int cityid, string key) 
        {
            var autoproject = DatProjectDA.GetSearchProjectListByKey(search, fxtcompanyid, cityid, key).Select(o => new
            {
                projectid = o.projectid,
                projectname = o.projectname,
                isevalue = o.isevalue,
                recordcount = o.recordcount,
                address = o.address,
                x=o.x,
                y=o.y,
                areaid=o.areaid,
                weight=o.weight
            });

            return autoproject.ToJson();
        }

        /// <summary>
        /// 获取自动估价楼盘详细信息
        /// </summary>
        /// <param name="search"></param>
        /// <param name="fxtcompanyid"></param>
        /// <param name="cityid"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetProjectDetailsByProjectid(SearchBase search, int fxtcompanyid, int cityid, int projectid) 
        {
            DATProject modelProject = DatProjectDA.GetProjectDetailsByProjectid(search, fxtcompanyid, cityid, projectid);
            var project = new { areaid = modelProject.areaid, address = modelProject.address,
                                casecnt = modelProject.casecnt,
                                isevalue = modelProject.isevalue,
                                enddate = modelProject.enddate,
            };
            return project.ToJson();
        }

        /// <summary>
        /// 楼盘详细：楼盘ID，楼盘名，区域名，是否可估，停车位，管理费，地址，区域id,竣工时间，开发商，物业管理  
        /// hody,暂为易房保
        /// </summary>
        /// <param name="search"></param>
        /// <param name="fxtcompanyid"></param>
        /// <param name="cityid"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetProjectInfoDetailsByProjectid(SearchBase search, int fxtcompanyid, int cityid, int projectid)
        {
            DATProject modelProject = DatProjectDA.GetProjectInfoDetailsByProjectid(search, fxtcompanyid, cityid, projectid);
            var project = new
            {
                projectid=modelProject.projectid,
                projectname=modelProject.projectname,
                areaname=modelProject.areaname,
                isevalue=modelProject.isevalue,
                parkingnumber=modelProject.parkingnumber,
                managerprice=modelProject.managerprice,
                address=modelProject.address,
                areaid=modelProject.areaid,
                enddate=modelProject.enddate,
                developcompanyname=modelProject.developcompanyname,
                managercompanyname=modelProject.managercompanyname,
                casecnt = modelProject.casecnt
            };
            return project.ToJson();
        }
        /// <summary>
        /// 根据楼盘名查询楼盘信息
        /// 创建人:曾智磊,日期:2014-06-26
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="areaId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <param name="projectname"></param>
        /// <returns></returns>
        public static DATProject GetProjectInfoByName(int cityId, int areaId, int fxtCompanyId, string projectname)
        {
            return DatProjectDA.GetProjectInfoByName(cityId, areaId, fxtCompanyId, projectname);
        }
        /// <summary>
        /// 根据公司ID和楼盘ID在子表中查询数据
        /// 创建人:曾智磊,日期:2014-06-26
        /// </summary>
        /// <param name="fxtCompanyId"></param>
        /// <param name="cityId"></param>
        /// <param name="projectid"></param>
        /// <returns></returns>
        public static DATProject GetProjectSubByProjectIdAndCompanyId(int fxtCompanyId, int cityId, int projectId)
        {
            return DatProjectDA.GetProjectSubByProjectIdAndCompanyId(fxtCompanyId, cityId, projectId);
        }
        /// <summary>
        /// 根据公司ID和楼盘ID在主表中查询数据
        /// 创建人:曾智磊,日期:2014-06-26
        /// </summary>
        /// <param name="fxtCompanyId"></param>
        /// <param name="cityId"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public static DATProject GetProjectParentByProjectIdAndCompanyId(int fxtCompanyId, int cityId, int projectId)
        {
            return DatProjectDA.GetProjectParentByProjectIdAndCompanyId(fxtCompanyId, cityId, projectId);
        }
        /// <summary>
        /// 新增楼盘信息到主表
        /// </summary>
        /// <param name="model"></param>
        /// <param name="tableName">根据城市查询出来的表名</param>
        /// <returns></returns>
        public static int Add(DATProject model,string tableName)
        {
            if (model == null||string.IsNullOrEmpty(tableName))
            {
                return 0;
            }
            string _tableName = tableName;
            DATProject.SetTableName<DATProject>(_tableName);
            return DatProjectDA.Add(model);

        }
        /// <summary>
        /// 新增楼盘信息到子表
        /// </summary>
        /// <param name="model"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static int AddSub(DATProject model, string tableName)
        {
            if (model == null || string.IsNullOrEmpty(tableName))
            {
                return 0;
            }
            string _tableName = tableName;
            _tableName = tableName + "_sub";
            DATProject.SetTableName<DATProject>(_tableName);
            return DatProjectDA.Add(model);
        }
        /// <summary>
        /// 修改楼盘信息到主表
        /// </summary>
        /// <param name="model"></param>
        /// <param name="tableName">根据城市查询出来的表名</param>
        /// <returns></returns>
        public static int Update(DATProject model, string tableName)
        {            
            if (model == null || string.IsNullOrEmpty(tableName))
            {
                return 0;
            }
            string _tableName = tableName;
            DATProject.SetTableName<DATProject>(_tableName);
            return DatProjectDA.Update(model);

        }
        /// <summary>
        /// 修改楼盘信息到子表
        /// </summary>
        /// <param name="model"></param>
        /// <param name="tableName">根据城市查询出来的表名</param>
        /// <returns></returns>
        public static int UpdateSub(DATProject model, string tableName)
        {
            if (model == null || string.IsNullOrEmpty(tableName))
            {
                return 0;
            }
            string _tableName = tableName+"_sub";
            DATProject.SetTableName<DATProject>(_tableName);
            return DatProjectDA.Update(model);

        }

        /// <summary>
        /// 新增照片
        /// 创建人:曾智磊,日期:2014-07-07
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int AddPhoto(LNKPPhoto model)
        {
            return DatProjectDA.AddPhoto(model);
        }
        /// <summary>
        /// 根据楼盘ID获取楼盘信息(关联子表)
        /// 创建人:曾智磊,日期:2014-07-03
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public static DATProject GetProjectInfoByProjectId(int cityId, int fxtCompanyId, int projectId)
        {
            return DatProjectDA.GetProjectInfoByProjectId(cityId, fxtCompanyId, projectId);
        }
        /// <summary>
        /// 根据多个楼盘名称，获取楼盘信息(关联子表)
        /// 创建人:曾智磊,日期:2014-07-03
        /// </summary>
        /// <param name="search"></param>
        /// <param name="projectnames"></param>
        /// <returns></returns>
        public static List<DATProject> GetProjectInfoByNames(int cityId, int areaId, int fxtCompanyId, string[] projectnames)
        {
            return DatProjectDA.GetProjectInfoByNames(cityId, areaId, fxtCompanyId, projectnames);
        }
    }
}
