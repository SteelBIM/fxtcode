using DataConv.Provider.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataConv.Provider
{
    public class ServiceFacade
    {
        /// <summary>
        /// 查询城市信息
        /// </summary>
        /// <returns></returns>
        public List<Model.City> QueryCityInfoList()
        {
            ServiceDao service = new ServiceDao();
            List<Model.City> tmp = service.QueryCityList();
            return tmp;
        }

        /// <summary>
        /// 分页查询城市数据
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<Model.City> PagingQueryCityList(int page)
        {
            ServiceDao service = new ServiceDao();
            List<Model.City> tmp = service.PagingQueryCityList(page);
            return tmp;
        }

        /// <summary>
        /// 查询行政区信息
        /// </summary>
        /// <param name="cityID"></param>
        /// <returns></returns>
        public List<Model.Area> QueryAreaInfoMap(int cityID)
        {
            ServiceDao service = new ServiceDao();
            return service.QueryAreaList(cityID);
        }

        /// <summary>
        /// 查询用途信息
        /// </summary>
        /// <returns></returns>
        public List<Model.SysCode> QueryPurposeInfoMap()
        {
            ServiceDao service = new ServiceDao();
            return service.QuerySysCodeList(" and ID = 1002 ");
        }

        /// <summary>
        /// 查询朝向信息
        /// </summary>
        /// <returns></returns>
        public List<Model.SysCode> QueryFrontInfoMap()
        {
            ServiceDao service = new ServiceDao();
            return service.QuerySysCodeList(" and ID = 2004 ");
        }

        /// <summary>
        /// 查询建筑类型
        /// </summary>
        /// <returns></returns>
        public List<Model.SysCode> QueryBuildingTypeInfoMap()
        {
            ServiceDao service = new ServiceDao();
            return service.QuerySysCodeList(" and ID = 2003 ");
        }

        /// <summary>
        /// 查询房屋类型
        /// </summary>
        /// <returns></returns>
        public List<Model.SysCode> QueryHouseTypeInfoMap()
        {
            ServiceDao service = new ServiceDao();
            return service.QuerySysCodeList(" and ID = 4001 ");
        }

        /// <summary>
        /// 查询结构信息
        /// </summary>
        /// <returns></returns>
        public List<Model.SysCode> QueryStructureInfoMap()
        {
            ServiceDao service = new ServiceDao();
            return service.QuerySysCodeList(" and ID = 2005 ");
        }

        /// <summary>
        /// 查询装修信息(暂时未用到)
        /// </summary>
        /// <returns></returns>
        public List<Model.SysCode> QueryFitmentInfoMap()
        {
            ServiceDao service = new ServiceDao();
            return service.QuerySysCodeList(" and ID = 1002 ");
        }

        /// <summary>
        /// 查询货币单位
        /// </summary>
        /// <returns></returns>
        public List<Model.SysCode> QueryMoneyUnitInfoMap()
        {
            ServiceDao service = new ServiceDao();
            return service.QuerySysCodeList(" and ID = 2002 ");
        }

        /// <summary>
        /// 批量插入案例数据
        /// </summary>
        /// <param name="dc"></param>
        /// <returns></returns>
        public int BatchInsertDataCase(Model.DataCase[] dc, string tableName)
        {
            ServiceDao service = new ServiceDao();
            return service.BatchInsertDataCase(dc, tableName);
        }

        /// <summary>
        /// 获取某个城市，某个区域的楼盘标准数据
        /// </summary>
        /// <param name="cityID"></param>
        /// <param name="AreaID"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public List<Model.DataProject> QueryDataProjectList(int cityID, int AreaID, string tableName)
        {
            ServiceDao service = new ServiceDao();
            return service.QueryDataProjectList(cityID, AreaID, tableName);
        }

                /// <summary>
        /// 分页查询城市所有楼盘名称信息
        /// </summary>
        /// <param name="CityID"></param>
        /// <param name="AreaID"></param>
        /// <param name="tName"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<Model.DataProject> PagingQueryProjectList(int CityID, int AreaID,
            string tName, int page)
        {
            ServiceDao service = new ServiceDao();
            return service.PagingQueryProjectList(CityID, AreaID, tName, page);
        }

    }
}
