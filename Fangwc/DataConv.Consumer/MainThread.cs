using DataConv.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using log4net;

namespace DataConv.Consumer
{
    public class MainThread
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(MainThread));
        public static void RunMain()
        {
            String[] cityId = ConfigurationManager.AppSettings["CityList"].Split(',');
            int length = cityId.Length;
            int[] arrays = new int[length];

            for (int i = 0; i < length; i++)
            {
                arrays[i] = Convert.ToInt32(cityId[i]);
            }
            string startTime = Convert.ToString(ConfigurationManager.AppSettings["StartTime"]);
            string endTime = Convert.ToString(ConfigurationManager.AppSettings["EndTime"]);
            int batchCommit = Convert.ToInt32(ConfigurationManager.AppSettings["BatchCommit"]);
            bool isRemoveData = Convert.ToBoolean(ConfigurationManager.AppSettings["RemoveData"]);

            logger.Info("配置参数加载完成，获取远程数据信息");

            //获取所有城市数据
            DataConv.Consumer.ProviderServiceRef.ProviderServiceClient psc =
                new DataConv.Consumer.ProviderServiceRef.ProviderServiceClient();

            int page = 1;
            List<DataConv.Consumer.ProviderServiceRef.City> list_city = new List<DataConv.Consumer.ProviderServiceRef.City>();
            while (true)
            {
                List<DataConv.Consumer.ProviderServiceRef.City> list = ServiceRef.QueryCityInfoList(psc, page);

                if (list.Count > 0)
                {
                    foreach (DataConv.Consumer.ProviderServiceRef.City item in list)
                    {
                        logger.DebugFormat("City name={0}, ID={1}", item.CityName, item.CityID);
                        list_city.Add(item);
                    }
                }
                else
                {
                    break;
                }
                page++;
            }
            
            Dictionary<string, int> PurposeMap = ServiceRef.QueryPurposeInfoMap(psc);
            Dictionary<string, int> FrontMap = ServiceRef.QueryFrontInfoMap(psc);
            Dictionary<string, int> BuildingTypeMap = ServiceRef.QueryBuildingTypeInfoMap(psc);
            Dictionary<string, int> HouseTypeMap = ServiceRef.QueryHouseTypeInfoMap(psc);
            Dictionary<string, int> StructureMap = ServiceRef.QueryStructureInfoMap(psc);
            Dictionary<string, int> MoneyUnitInfoMap = ServiceRef.QueryMoneyUnitInfoMap(psc);

            if (PurposeMap.Keys.Count == 0
                || FrontMap.Keys.Count == 0
                || BuildingTypeMap.Keys.Count == 0
                || HouseTypeMap.Keys.Count == 0
                || StructureMap.Keys.Count == 0)
            {
                logger.Info("基础数据获取不全，无法完成后续操作！");
                Console.ReadLine();
            }
            else
            {
                CaseDao caseDao = new CaseDao();
                Dictionary<string, string> paramMap = new Dictionary<string, string>();
                paramMap.Add("[#StartTime#]", startTime);
                paramMap.Add("[#EndTime#]", endTime);

                foreach (DataConv.Consumer.ProviderServiceRef.City item in list_city)
                {
                    Console.WriteLine(item.CityName + " 转换数据开始时间= " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    logger.InfoFormat("{0} 转换数据开始时间= {1}", item.CityName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    if (paramMap.ContainsKey("[#CityID#]"))
                    {
                        paramMap.Remove("[#CityID#]");
                    }
                    paramMap.Add("[#CityID#]", Convert.ToString(item.CityID));
                    //此处可以根据item，取出City的各个值
                    logger.Info("CityName = " + item.CityName + "\t" + item.ProjectTable + "\t" + item.CaseTable);
                    //该集合中的对象到15个时，执行一次插入，并清空
                    List<DataConv.Consumer.ProviderServiceRef.DataCase> lst_sc =
                        new List<DataConv.Consumer.ProviderServiceRef.DataCase>();
                    //循环获取房源信息【住宅案例_出售】
                    int pageIndex = 1;

                    ProviderServiceRef.Area[] a = psc.QueryAreaInfoMap(item.CityID);

                    //获取城市对应的行政区
                    Dictionary<string, int> dict = new Dictionary<string, int>();
                    foreach (ProviderServiceRef.Area aitem in a)
                    {
                        //logger.Debug(aitem.AreaName + "\t" + aitem.AreaId);
                        dict.Add(aitem.AreaName, aitem.AreaId);
                    }
                    item.AreaMap = dict;

                    //获取城市对应的楼盘信息表（标准数据，对照源）
                    //List<ProviderServiceRef.DataProject> lst_project = ServiceRef.QueryDataProjectList(psc, item.CityID, 0, item.ProjectTable);
                    List<ProviderServiceRef.DataProject> lst_project = new List<ProviderServiceRef.DataProject>();
                    while (true)
                    {
                        List<ProviderServiceRef.DataProject> list = 
                            ServiceRef.PagingQueryDataProjectList(psc, item.CityID, 0, 
                            item.ProjectTable, pageIndex);
                        if (list.Count == 0)
                        {
                            break;
                        }
                        foreach (ProviderServiceRef.DataProject item1 in list)
                        {
                            lst_project.Add(item1);
                        }
                        pageIndex++;
                    }

                    pageIndex = 1;
                    while (true)
                    {
                        List<DataCase> list = caseDao.PagingQueryDataCase(pageIndex, 10, paramMap);
                        if (list.Count == 0)
                        {
                            //取不到数据，就退出循环
                            break;
                        }
                        foreach (DataCase data in list)
                        {
                            int projectId = ValidatorUtils.IsValidProject(lst_project, data);
                            bool isValid = ValidatorUtils.IsValidData(item,
                                data, arrays, PurposeMap, FrontMap, BuildingTypeMap,
                                HouseTypeMap, StructureMap, MoneyUnitInfoMap);

                            if (isValid && projectId != -1)
                            {
                                #region 符合条件的数据
                                lst_sc.Add(new DataConv.Consumer.ProviderServiceRef.DataCase()
                                                            {
                                                                ProjectId = projectId,
                                                                CaseDate = data.CaseDate,
                                                                AreaId = data.AreaId == null ? -1 : data.AreaId,
                                                                AreaName = data.AreaName == null ? null : data.AreaName,
                                                                BuildingArea = data.BuildingArea = data.BuildingArea,
                                                                PurposeCode = data.PurposeCode == null ? -1 : data.PurposeCode,
                                                                BuildingDate = data.BuildingDate == null ? null : data.BuildingDate,
                                                                BuildingTypeCode = data.BuildingTypeCode == null ? -1 : data.BuildingTypeCode,
                                                                CaseTypeCode = data.CaseTypeCode == null ? -1 : data.CaseTypeCode,
                                                                CityID = data.CityID == null ? -1 : data.CityID,
                                                                CaseTypeName = data.CaseTypeName == null ? null : data.CaseTypeName,
                                                                BuildingTypeName = data.BuildingTypeName == null ? null : data.BuildingTypeName,
                                                                FloorNumber = data.FloorNumber,
                                                                FrontCode = data.FrontCode == null ? -1 : data.FrontCode,
                                                                FrontName = data.FrontName == null ? null : data.FrontName,
                                                                HouseTypeCode = data.HouseTypeCode == null ? -1 : data.HouseTypeCode,
                                                                HouseTypeName = data.HouseTypeName == null ? null : data.HouseTypeName,
                                                                MoneyUnitCode = data.MoneyUnitCode == null ? -1 : data.MoneyUnitCode,
                                                                MoneyUnitName = data.MoneyUnitName == null ? null : data.MoneyUnitName,
                                                                PeiTao = data.PeiTao == null ? null : data.PeiTao,
                                                                PurposeName = data.PurposeName == null ? null : data.PurposeName,
                                                                ProjectName = data.ProjectName == null ? null : data.ProjectName,
                                                                RecordWeek = data.RecordWeek,
                                                                RemainYear = data.RemainYear,
                                                                Remark = data.Remark == null ? null : data.Remark,
                                                                SourceLink = data.SourceLink == null ? null : data.SourceLink,
                                                                SourceName = data.SourceName == null ? null : data.SourceName,
                                                                SightCode = data.SightCode == null ? -1 : data.SightCode,
                                                                SourcePhone = data.SourcePhone == null ? null : data.SourcePhone,
                                                                StructureCode = data.StructureCode == null ? -1 : data.StructureCode,
                                                                StructureName = data.StructureName == null ? null : data.StructureName,
                                                                CreateDate = DateTime.Now,
                                                                Creator = data.Creator == null ? null : data.Creator,
                                                                SurveyId = data.SurveyId == null ? 0 : data.SurveyId,
                                                                TotalFloor = data.TotalFloor,
                                                                TotalPrice = data.TotalPrice = data.TotalPrice,
                                                                UnitPrice = data.UnitPrice = data.UnitPrice,
                                                                Valid = data.Valid == null ? 1 : data.Valid,
                                                                ZhuangXiu = data.ZhuangXiu == null ? null : data.ZhuangXiu,
                                                                Depreciation = data.Depreciation
                                                            });
                                #endregion
                            }
                            else
                            {
                                #region 失败信息，需要入库
                                logger.InfoFormat("<<< INVALID DATA CityName={0}, ProjectName={1}, ProjectId={2}, isValid={3}",
                                data.CityID, data.ProjectName, projectId, isValid);
                                caseDao.AddExceptionData(data);
                                #endregion
                            }
                            //if (lst_sc.Count == batchCommit)
                            //{
                            //    #region List中的对象数到15条，就执行一次入库
                            //    BatchCaseData(batchCommit, psc, lst_sc, item.CaseTable);
                            //    lst_sc = new List<ProviderServiceRef.DataCase>();
                            //    #endregion
                            //}
                        }
                        //if (lst_sc.Count > 0)
                        //{
                        //    #region List中的对象数到15条，就执行一次入库
                        //    BatchCaseData(batchCommit, psc, lst_sc, item.CaseTable);
                        //    lst_sc = new List<ProviderServiceRef.DataCase>();
                        //    #endregion
                        //}
                        pageIndex++;
                    }
                    
                    if (lst_sc.Count > 0)
                    {
                        #region List中的对象数到15条，就执行一次入库
                        BatchCaseData(batchCommit, psc, lst_sc, item.CaseTable);
                        lst_sc = new List<ProviderServiceRef.DataCase>();
                        #endregion
                    }
                    if (isRemoveData)
                    {
                        //从源库删除这些已经处理转换过的案例
                        int cnt = caseDao.RemoveDataCase(paramMap);
                        logger.Info("当前批量删除已经处理转换过的案例DataCase[" + cnt + "]行数据...");
                    }

                    Console.WriteLine(item.CityName + " 转换数据结束时间= " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    logger.InfoFormat("{0} 转换数据结束时间= {1}", item.CityName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                Console.WriteLine("================>> 数据转换完成，时间 = " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                logger.InfoFormat("================================================>> 数据转换完成，时间 = {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            }
        }

        private static void BatchCaseData(int batchCommit, DataConv.Consumer.ProviderServiceRef.ProviderServiceClient psc, List<DataConv.Consumer.ProviderServiceRef.DataCase> lst_sc, string tableName)
        {
            if (lst_sc.Count > 0)
            {
                RemoveDuplicate(lst_sc);//去除重复数据
                if (lst_sc.Count > 0)
                    PriceFilter(lst_sc);//价格过滤
            }
            int length = lst_sc.Count;
            //把List转成数组，WCF必须传数组过去才行
            DataConv.Consumer.ProviderServiceRef.DataCase[] dc_tmp =
                new DataConv.Consumer.ProviderServiceRef.DataCase[length];
            for (int n = 0; n < length; n++)
            {
                dc_tmp[n] = lst_sc[n];
            }
            //批量入库，返回成功插入数据库的记录数
            int insertCount = psc.BatchInsertDataCase(dc_tmp, tableName);

            logger.Info("当前批量插入[" + insertCount + "]行数据...");
        }

        private static void RemoveDuplicate(List<DataConv.Consumer.ProviderServiceRef.DataCase> lst_sc)
        {
            if (lst_sc == null || lst_sc.Count == 0)
                return;
            List<int> lst_sc_dup = new List<int>();
            Dictionary<string, DataConv.Consumer.ProviderServiceRef.DataCase> tempDict = new Dictionary<string, ProviderServiceRef.DataCase>();
            for (int i = 0; i < lst_sc.Count; i++)
            {
                var item = lst_sc[i];
                string key = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}", item.ProjectName, item.AreaName, item.FloorNumber, item.TotalFloor, item.PurposeName, item.BuildingArea, item.UnitPrice, item.TotalPrice, item.FrontCode, item.BuildingTypeName, item.HouseTypeCode);
                if (tempDict.ContainsKey(key))
                {
                    lst_sc_dup.Add(i);
                }
                else
                {
                    tempDict.Add(key, item);
                }
            }
            for (int i = lst_sc_dup.Count - 1; i >= 0; i--)
            {
                lst_sc.RemoveAt(lst_sc_dup[i]);
            }
        }

        private static void PriceFilter(List<DataConv.Consumer.ProviderServiceRef.DataCase> lst_sc)
        {
            if (lst_sc == null || lst_sc.Count == 0)
                return;
            
            Dictionary<string, decimal?> s11 = new Dictionary<string, decimal?>();
            Dictionary<string, decimal?> s12 = new Dictionary<string, decimal?>();
            Dictionary<string, decimal?> s21 = new Dictionary<string, decimal?>();
            Dictionary<string, decimal?> s22 = new Dictionary<string, decimal?>();
            for (int i = 0; i < lst_sc.Count; i++)
            {
                var item = lst_sc[i];
                string key1 = string.Format("{0}{1}",item.ProjectName,item.PurposeName);
                string key2 = string.Format("{0}{1}", item.ProjectName, item.BuildingTypeName);
                //PurposeName
                if (s11.ContainsKey(key1))
                {
                    s11[key1] += item.UnitPrice * item.BuildingArea;
                    s12[key1] += item.BuildingArea;
                }
                else
                {
                    s11.Add(key1, item.UnitPrice * item.BuildingArea);
                    s12.Add(key1, item.BuildingArea);
                }
                //BuildingTypeName
                if (s21.ContainsKey(key2))
                {
                    s21[key2] += item.UnitPrice * item.BuildingArea;
                    s22[key2] += item.BuildingArea;
                }
                else
                {
                    s21.Add(key2, item.UnitPrice * item.BuildingArea);
                    s22.Add(key2, item.BuildingArea);
                }
            }
            List<int> lst_sc_del = new List<int>();
            for (int i = 0; i < lst_sc.Count; i++)
            {
                var item = lst_sc[i];
                string key1 = string.Format("{0}{1}", item.ProjectName, item.PurposeName);
                string key2 = string.Format("{0}{1}", item.ProjectName, item.BuildingTypeName);
                //PurposeName
                var avg = s11[key1] / s12[key1];
                var r = item.UnitPrice <= avg * (decimal)1.2 && item.UnitPrice >= avg * (decimal)0.8;
                if (!r)
                {
                    lst_sc_del.Add(i);
                    continue;
                }
                //BuildingTypeName
                avg = s21[key2] / s22[key2];
                r = item.UnitPrice <= avg * (decimal)1.2 && item.UnitPrice >= avg * (decimal)0.8;
                if (!r)
                {
                    lst_sc_del.Add(i);
                    continue;
                }
            }
            for (int i = lst_sc_del.Count - 1; i >= 0; i--)
            {
                lst_sc.RemoveAt(lst_sc_del[i]);
            }
        }

    }
}
