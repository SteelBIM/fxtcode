
using CDI.Models;
using CDI.Utils;
using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CDI.Client
{
    public class ServiceRef
    {
        private static readonly ILog logger = CurrentData.Instance.Logger;
        /// <summary>
        /// 获取所有城市数据
        /// </summary>
        public static List<City> QueryCityInfoList(IProxy psc, int page)
        {
            var cities = psc.PagingQueryCityList(page).ValidateStatus<CityResponseModel>().Citys;

            #region 循环获取每个城市对应的区域数据，并将成City对象存入List集合中
            List<City> list_city = new List<City>();
            foreach (City item in cities)
            {
                // 查询城市对应案例表信息
                list_city.Add(new City()
                {
                    CityID = item.CityID,
                    CityName = item.CityName,
                    ProvinceId = item.ProvinceId,
                    CaseTable = item.CaseTable,
                    ProjectTable = item.ProjectTable
                });
            }
            #endregion
            return list_city;
        }

        /// <summary>
        /// 查询用途信息
        /// </summary>
        /// <param name="psc"></param>
        /// <returns></returns>
        public static Dictionary<string, int> QueryPurposeInfoMap(IProxy psc)
        {
            Dictionary<string, int> PurposeMap = new Dictionary<string, int>();
            var s = psc.QueryPurposeInfoMap().ValidateStatus<SysCodeResponseModel>().SysCodes;
            foreach (SysCode item in s)
            {
                PurposeMap.Add(item.CodeName, item.Code);
                logger.Debug(item.CodeName + "\t" + item.Code);
            }

            return PurposeMap;
        }

        /// <summary>
        /// 查询朝向信息
        /// </summary>
        /// <param name="psc"></param>
        /// <returns></returns>
        public static Dictionary<string, int> QueryFrontInfoMap(IProxy psc)
        {
            Dictionary<string, int> FrontMap = new Dictionary<string, int>();
            var s1 = psc.QueryFrontInfoMap().ValidateStatus<SysCodeResponseModel>().SysCodes;
            foreach (SysCode item in s1)
            {
                FrontMap.Add(item.CodeName, item.Code);
                logger.Debug(item.CodeName + "\t" + item.Code);
            }

            return FrontMap;
        }

        /// <summary>
        /// 查询建筑类型信息
        /// </summary>
        /// <param name="psc"></param>
        /// <returns></returns>
        public static Dictionary<string, int> QueryBuildingTypeInfoMap(IProxy psc)
        {
            Dictionary<string, int> BuildingTypeMap = new Dictionary<string, int>();
            var s2 = psc.QueryBuildingTypeInfoMap().ValidateStatus<SysCodeResponseModel>().SysCodes;
            foreach (SysCode item in s2)
            {
                BuildingTypeMap.Add(item.CodeName, item.Code);
                logger.Debug(item.CodeName + "\t" + item.Code);
            }
            return BuildingTypeMap;
        }

        /// <summary>
        /// 查询户型信息
        /// </summary>
        /// <param name="psc"></param>
        /// <returns></returns>
        public static Dictionary<string, int> QueryHouseTypeInfoMap(IProxy psc)
        {
            Dictionary<string, int> HouseTypeMap = new Dictionary<string, int>();
            var s3 = psc.QueryHouseTypeInfoMap().ValidateStatus<SysCodeResponseModel>().SysCodes;
            foreach (SysCode item in s3)
            {
                HouseTypeMap.Add(item.CodeName, item.Code);
                logger.Debug(item.CodeName + "\t" + item.Code);
            }
            return HouseTypeMap;
        }

        /// <summary>
        /// 查询户型结构信息
        /// </summary>
        /// <param name="psc"></param>
        /// <returns></returns>
        public static Dictionary<string, int> QueryStructureInfoMap(IProxy psc)
        {
            Dictionary<string, int> StructureMap = new Dictionary<string, int>();
            var s4 = psc.QueryStructureInfoMap().ValidateStatus<SysCodeResponseModel>().SysCodes;
            foreach (SysCode item in s4)
            {
                StructureMap.Add(item.CodeName, item.Code);
                logger.Debug(item.CodeName + "\t" + item.Code);
            }
            return StructureMap;
        }

        /// <summary>
        /// 查询货币单位信息
        /// </summary>
        /// <param name="psc"></param>
        /// <returns></returns>
        public static Dictionary<string, int> QueryMoneyUnitInfoMap(IProxy psc)
        {
            Dictionary<string, int> StructureMap = new Dictionary<string, int>();
            var s4 = psc.QueryMoneyUnitInfoMap().ValidateStatus<SysCodeResponseModel>().SysCodes;
            foreach (SysCode item in s4)
            {
                StructureMap.Add(item.CodeName, item.Code);
                logger.Debug(item.CodeName + "\t" + item.Code);
            }
            return StructureMap;
        }

        public static List<DataProject> QueryDataProjectList(IProxy psc, int cityId, int AreaId, string tableName)
        {
            var projects = psc.QueryDataProjectList(cityId, AreaId, tableName).ValidateStatus<DataProjectResponseModel>().DataProjects;

            #region 循环获取每个城市对应的区域数据，并将成City对象存入List集合中
            List<DataProject> list_projects = new List<DataProject>();
            foreach (DataProject item in projects)
            {
                DataProject item_tmp = new DataProject()
                {
                    CityID = item.CityID,
                    OtherName = item.OtherName,
                    PinYin = item.PinYin,
                    PinYinAll = item.PinYinAll,
                    ProjectId = item.ProjectId,
                    ProjectName = item.ProjectName
                };

                //projectName
                //if ("".Equals(item.PinYinAll) || "".Equals(item.PinYin))
                //{
                string tmp_all = "";
                //if (item.ProjectName.Length >= 3)
                //{
                //    tmp_all = Chinese2Spell.Convert(item_tmp.ProjectName.Substring(0, 3));
                //}
                //else
                //{
                tmp_all = Chinese2Spell.Convert(item_tmp.ProjectName);
                //}

                item_tmp.PinYinAll = tmp_all.ToUpper();   //ShenZhen
                item_tmp.PinYin = Chinese2Spell.getFirstLetter(tmp_all);  //SZ
                //}

                //otherName
                if (!string.IsNullOrEmpty(item_tmp.OtherName))
                //if (!"".Equals(Convert.ToString(item_tmp.OtherName)))
                {
                    //if (item_tmp.OtherName.Length >= 3)
                    //{
                    //    tmp_all = Chinese2Spell.Convert(item_tmp.OtherName.Substring(0, 3));
                    //}
                    //else
                    //{
                    tmp_all = Chinese2Spell.Convert(item_tmp.OtherName);
                    //}

                    item_tmp.OtherPinyinAll = tmp_all.ToUpper();   //ShenZhen

                    item_tmp.OtherPinyin = Chinese2Spell.getFirstLetter(tmp_all);  //SZ
                }

                list_projects.Add(item_tmp);
            }
            #endregion
            return list_projects;
        }

        /// <summary>
        /// 分页查询楼盘信息
        /// </summary>
        /// <param name="psc"></param>
        /// <param name="cityId"></param>
        /// <param name="AreaId"></param>
        /// <param name="tableName"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public static List<DataProject> PagingQueryDataProjectList(IProxy psc, int cityId,int AreaId, string tableName, int page)
        {
            var projects = psc.PagingQueryProjectList(cityId, AreaId, tableName, page).ValidateStatus<DataProjectResponseModel>().DataProjects;

            #region 循环获取每个城市对应的区域数据，并将成City对象存入List集合中
            List<DataProject> list_projects = new List<DataProject>();
            foreach (DataProject item in projects)
            {
                DataProject item_tmp = new DataProject()
                {
                    CityID = item.CityID,
                    OtherName = item.OtherName,
                    PinYin = item.PinYin,
                    PinYinAll = item.PinYinAll,
                    ProjectId = item.ProjectId,
                    ProjectName = item.ProjectName
                };

                string tmp_all = "";
                tmp_all = Chinese2Spell.Convert(item_tmp.ProjectName);

                item_tmp.PinYinAll = tmp_all.ToUpper();   //ShenZhen
                item_tmp.PinYin = Chinese2Spell.getFirstLetter(tmp_all);  //SZ

                if (!string.IsNullOrEmpty(item_tmp.OtherName))
                {
                    tmp_all = Chinese2Spell.Convert(item_tmp.OtherName);
                    item_tmp.OtherPinyinAll = tmp_all.ToUpper();   //ShenZhen
                    item_tmp.OtherPinyin = Chinese2Spell.getFirstLetter(tmp_all);  //SZ
                }

                list_projects.Add(item_tmp);
            }
            #endregion
            return list_projects;
        }

        /// <summary>
        /// 分页查询楼盘网络名列表
        /// </summary>
        public static List<SYS_ProjectMatch> PagingQueryNetworkNames(IProxy psc, int cityId, int pageNumber, int pageSize)
        {
            var projNetworkNames = psc.GetNetworkNames(cityId, pageNumber, pageSize).ValidateStatus<ProjectNameResponseModel>().NetworkNames;
            return projNetworkNames;
        }

    }
}
