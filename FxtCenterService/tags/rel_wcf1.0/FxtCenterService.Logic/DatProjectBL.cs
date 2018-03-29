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
            condition = " and ([PinYin] like @strKey "
                //+"or [Address] like @strKey " //不再检索地址，BUG #2198
                + "or [ProjectName] like @strKey  or [PinYinAll] like @strKey )";
            key = strKey + "%";
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
                recordcount = o.recordcount
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
    }
}
