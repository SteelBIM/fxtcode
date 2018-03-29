using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataConv.Consumer
{
    public class ServiceRef
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(ServiceRef));
        /// <summary>
        /// 获取所有城市数据
        /// </summary>
        /// <returns></returns>
        public static List<ProviderServiceRef.City> QueryCityInfoList(
            ProviderServiceRef.ProviderServiceClient psc, int page)
        {

            //ProviderServiceRef.City[] cities = psc.QueryCityInfoList();
            ProviderServiceRef.City[] cities = psc.PagingQueryCityList(page);

            #region 循环获取每个城市对应的区域数据，并将成City对象存入List集合中
            List<ProviderServiceRef.City> list_city = new List<ProviderServiceRef.City>();
            foreach (ProviderServiceRef.City item in cities)
            {
                // 查询城市对应案例表信息
                list_city.Add(new ProviderServiceRef.City()
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
        public static Dictionary<string, int> QueryPurposeInfoMap(
            ProviderServiceRef.ProviderServiceClient psc)
        {
            Dictionary<string, int> PurposeMap = new Dictionary<string, int>();
            psc = new ProviderServiceRef.ProviderServiceClient();
            ProviderServiceRef.SysCode[] s = psc.QueryPurposeInfoMap();
            foreach (ProviderServiceRef.SysCode item in s)
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
        public static Dictionary<string, int> QueryFrontInfoMap(
            ProviderServiceRef.ProviderServiceClient psc)
        {
            Dictionary<string, int> FrontMap = new Dictionary<string, int>();
            ProviderServiceRef.SysCode[] s1 = psc.QueryFrontInfoMap();
            foreach (ProviderServiceRef.SysCode item in s1)
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
        public static Dictionary<string, int> QueryBuildingTypeInfoMap(
            ProviderServiceRef.ProviderServiceClient psc)
        {
            Dictionary<string, int> BuildingTypeMap = new Dictionary<string, int>();
            ProviderServiceRef.SysCode[] s2 = psc.QueryBuildingTypeInfoMap();
            foreach (ProviderServiceRef.SysCode item in s2)
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
        public static Dictionary<string, int> QueryHouseTypeInfoMap(
            ProviderServiceRef.ProviderServiceClient psc)
        {
            Dictionary<string, int> HouseTypeMap = new Dictionary<string, int>();
            ProviderServiceRef.SysCode[] s3 = psc.QueryHouseTypeInfoMap();
            foreach (ProviderServiceRef.SysCode item in s3)
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
        public static Dictionary<string, int> QueryStructureInfoMap(
            ProviderServiceRef.ProviderServiceClient psc)
        {
            Dictionary<string, int> StructureMap = new Dictionary<string, int>();
            ProviderServiceRef.SysCode[] s4 = psc.QueryStructureInfoMap();
            foreach (ProviderServiceRef.SysCode item in s4)
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
        public static Dictionary<string, int> QueryMoneyUnitInfoMap(
            ProviderServiceRef.ProviderServiceClient psc)
        {
            Dictionary<string, int> StructureMap = new Dictionary<string, int>();
            ProviderServiceRef.SysCode[] s4 = psc.QueryMoenyUnitInfoMap();
            foreach (ProviderServiceRef.SysCode item in s4)
            {
                StructureMap.Add(item.CodeName, item.Code);
                logger.Debug(item.CodeName + "\t" + item.Code);
            }
            return StructureMap;
        }

        public static List<ProviderServiceRef.DataProject> QueryDataProjectList(
            ProviderServiceRef.ProviderServiceClient psc, int cityId, 
            int AreaId, string tableName)
        {
            ProviderServiceRef.DataProject[] projects = psc.QueryDataProjectList(cityId, AreaId, tableName);

            #region 循环获取每个城市对应的区域数据，并将成City对象存入List集合中
            List<ProviderServiceRef.DataProject> list_projects = new List<ProviderServiceRef.DataProject>();
            foreach (ProviderServiceRef.DataProject item in projects)
            {
                ProviderServiceRef.DataProject item_tmp = new ProviderServiceRef.DataProject()
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
                //    tmp_all = DataConv.Library.Chinese2Spell.Convert(item_tmp.ProjectName.Substring(0, 3));
                //}
                //else
                //{
                tmp_all = DataConv.Library.Chinese2Spell.Convert(item_tmp.ProjectName);
                //}

                item_tmp.PinYinAll = tmp_all.ToUpper();   //ShenZhen
                item_tmp.PinYin = DataConv.Library.Chinese2Spell.getFirstLetter(tmp_all);  //SZ
                //}

                //otherName
                if (!string.IsNullOrEmpty(item_tmp.OtherName))
                //if (!"".Equals(Convert.ToString(item_tmp.OtherName)))
                {
                    //if (item_tmp.OtherName.Length >= 3)
                    //{
                    //    tmp_all = DataConv.Library.Chinese2Spell.Convert(item_tmp.OtherName.Substring(0, 3));
                    //}
                    //else
                    //{
                    tmp_all = DataConv.Library.Chinese2Spell.Convert(item_tmp.OtherName);
                    //}

                    item_tmp.OtherPinyinAll = tmp_all.ToUpper();   //ShenZhen

                    item_tmp.OtherPinyin = DataConv.Library.Chinese2Spell.getFirstLetter(tmp_all);  //SZ
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
        public static List<ProviderServiceRef.DataProject> PagingQueryDataProjectList(
            ProviderServiceRef.ProviderServiceClient psc, int cityId,
            int AreaId, string tableName, int page)
        {
            ProviderServiceRef.DataProject[] projects = psc.PagingQueryProjectList(cityId, AreaId, tableName, page);

            #region 循环获取每个城市对应的区域数据，并将成City对象存入List集合中
            List<ProviderServiceRef.DataProject> list_projects = new List<ProviderServiceRef.DataProject>();
            foreach (ProviderServiceRef.DataProject item in projects)
            {
                ProviderServiceRef.DataProject item_tmp = new ProviderServiceRef.DataProject()
                {
                    CityID = item.CityID,
                    OtherName = item.OtherName,
                    PinYin = item.PinYin,
                    PinYinAll = item.PinYinAll,
                    ProjectId = item.ProjectId,
                    ProjectName = item.ProjectName
                };

                string tmp_all = "";
                tmp_all = DataConv.Library.Chinese2Spell.Convert(item_tmp.ProjectName);

                item_tmp.PinYinAll = tmp_all.ToUpper();   //ShenZhen
                item_tmp.PinYin = DataConv.Library.Chinese2Spell.getFirstLetter(tmp_all);  //SZ

                if (!string.IsNullOrEmpty(item_tmp.OtherName))
                {
                    tmp_all = DataConv.Library.Chinese2Spell.Convert(item_tmp.OtherName);
                    item_tmp.OtherPinyinAll = tmp_all.ToUpper();   //ShenZhen
                    item_tmp.OtherPinyin = DataConv.Library.Chinese2Spell.getFirstLetter(tmp_all);  //SZ
                }

                list_projects.Add(item_tmp);
            }
            #endregion
            return list_projects;
        }


    }
}
