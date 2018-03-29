
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Common.Logging;
using CDI.Models;
using CDI.Client.Dao;
using System.Threading;
using System.Threading.Tasks;


namespace CDI.Client
{
    public class MainThread
    {
        private static readonly ILog logger = CurrentData.Instance.Logger;

        public static Dictionary<string, int> RunMain(string startTime, string endTime, int sCityId, int[] cityIds, CancellationToken token)
        {
            Dictionary<string, int> caseCount = new Dictionary<string, int>();
            String[] cityId = ConfigurationManager.AppSettings["CityList"].Split(',');
            int length = cityId.Length;
            int[] arrays = new int[length];

            for (int i = 0; i < length; i++)
            {
                arrays[i] = Convert.ToInt32(cityId[i]);
            }
            bool isRemoveData = Convert.ToBoolean(ConfigurationManager.AppSettings["RemoveData"]);

            logger.Info("配置参数加载完成，获取远程数据信息");

            //获取所有城市数据
            IProxy psc = new WCFProxy();

            int page = 1;
            List<City> list_city = new List<City>();
            while (true)
            {
                List<City> list = ServiceRef.QueryCityInfoList(psc, page);

                if (list.Count > 0)
                {
                    foreach (City item in list)
                    {
                        logger.DebugFormat("City name={0}, ID={1}", item.CityName, item.CityID);
                        list_city.Add(item);
                        if (sCityId != -1 && sCityId == item.CityID)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    break;
                }
                page++;
            }

            #region 用户选择城市列表
            //var selectedCitys = App.Current.Dispatcher.Invoke(new Func<List<City>, object>((citys) =>
            //{
            //    CityListWindow cityWin = new CityListWindow();
            //    cityWin.Owner = App.Current.MainWindow;
            //    cityWin.DataContext = citys;
            //    var r = cityWin.ShowDialog();
            //    if (r.HasValue && r.Value)
            //    {

            //        return null;
            //    }
            //    else
            //    {
            //        logger.Info("用户取消");
            //        return null;
            //    }
            //}), list_city);
            //if (selectedCitys == null)
            //{
            //    return;
            //}
            #endregion

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
                return caseCount;
            }

            CaseDao caseDao = new CaseDao();
            Dictionary<string, string> paramMap = new Dictionary<string, string>();
            paramMap.Add("[#StartTime#]", startTime);
            paramMap.Add("[#EndTime#]", endTime);

            ImportRecord record = new ImportRecord();
            record.CaseBeginDate = DateTime.Parse(startTime);
            record.CaseEndDate = DateTime.Parse(endTime);
            record.ImportTime = DateTime.Now;
            record.ImportUser = CurrentData.Instance.UserName;
            record.ImportCaseNumber = 0;
            record.ExceptionCaseNumber = 0;
            int failCount = 0;
            foreach (City item in list_city)
            {
                if (sCityId == -1)
                {
                    if (cityIds.Contains(item.CityID))
                    {
                        continue;
                    }
                }
                else
                {
                    if (item.CityID != sCityId)
                    {
                        continue;
                    }
                }
                if (token.IsCancellationRequested)
                {
                    Console.WriteLine("Task cancel detected");
                    //token.ThrowIfCancellationRequested();
                    //throw new OperationCanceledException(token);
                    return caseCount;
                }
                failCount = 0;
                record.ID = item.CityID;
                record.CityName = item.CityName;
                List<DataCase> exceptionList = new List<DataCase>();

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
                List<DataCase> lst_sc =
                    new List<DataCase>();
                //循环获取房源信息【住宅案例_出售】
                int pageIndex = 1;

                var a = psc.QueryAreaInfoMap(item.CityID).ValidateStatus<AreaResponseModel>().Areas;

                //获取城市对应的行政区
                Dictionary<string, int> dict = new Dictionary<string, int>();
                foreach (Area aitem in a)
                {
                    //logger.Debug(aitem.AreaName + "\t" + aitem.AreaId);
                    dict.Add(aitem.AreaName, aitem.AreaId);
                }
                item.AreaMap = dict;

                //获取城市对应的楼盘信息表（标准数据，对照源）
                //List<DataProject> lst_project = ServiceRef.QueryDataProjectList(psc, item.CityID, 0, item.ProjectTable);
                List<DataProject> lst_project = new List<DataProject>();
                while (true)
                {
                    List<DataProject> list = ServiceRef.PagingQueryDataProjectList(psc, item.CityID, 0, item.ProjectTable, pageIndex);
                    if (list.Count == 0)
                    {
                        break;
                    }
                    lst_project.AddRange(list);
                    pageIndex++;
                }
                //获取当前城市楼盘网络别名列表
                List<SYS_ProjectMatch> projNetworkNames = new List<SYS_ProjectMatch>();
                pageIndex = 1;
                while (true)
                {
                    List<SYS_ProjectMatch> list = ServiceRef.PagingQueryNetworkNames(psc, item.CityID, pageIndex, 500);
                    if (list.Count == 0)
                        break;
                    projNetworkNames.AddRange(list);
                    pageIndex++;
                }

                Dictionary<string, int> cacheProjectIds = new Dictionary<string, int>();
                pageIndex = 1;
                while (true)
                {
                    if (token.IsCancellationRequested)
                    {
                        Console.WriteLine("Task cancel detected");
                        //token.ThrowIfCancellationRequested();
                        //throw new OperationCanceledException(token);
                        return caseCount;
                    }
                    bool flag = false;
                    List<DataCase> list = caseDao.PagingQueryDataCase(pageIndex, 500, paramMap, out flag);
                    if (flag)//重试一次
                    {
                        flag = false;
                        list = caseDao.PagingQueryDataCase(pageIndex, 500, paramMap, out flag);
                        if (flag)
                        {
                            string str = "因查询数据库出错，本次导入任务终止，不影响异常数据和正式库，下次可以重新导入！";
                            logger.InfoFormat(str);
                            Console.WriteLine(str);
                            return caseCount;
                        }
                    }
                    if (list.Count == 0)
                    {
                        //取不到数据，就退出循环
                        break;
                    }
                    RemoveDuplicate(list);//原始数据先去重一次
                    foreach (DataCase data in list)
                    {
                        int projectId = ValidatorUtils.IsValidProject(lst_project, projNetworkNames, data, cacheProjectIds);
                        bool isValid = ValidatorUtils.IsValidData(item,
                            data, arrays, PurposeMap, FrontMap, BuildingTypeMap,
                            HouseTypeMap, StructureMap, MoneyUnitInfoMap);

                        if (isValid && projectId != -1)
                        {
                            #region 符合条件的数据
                            lst_sc.Add(new DataCase()
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
                            failCount++;
                            exceptionList.Add(data);
                            logger.InfoFormat("<<< INVALID DATA CityName={0}, ProjectName={1}, ProjectId={2}, isValid={3}",
                                    data.CityID, data.ProjectName, projectId, isValid);
                        }
                    }
                    pageIndex++;
                }
                record.ExceptionCaseNumber = failCount;
                if (token.IsCancellationRequested)
                {
                    Console.WriteLine("Task cancel detected");
                    return caseCount;
                }
                if (exceptionList.Count > 0)
                {
                    RemoveDuplicate(exceptionList);//去除异常数据中的重复数据
                    foreach (DataCase data in exceptionList)
                    {
                        #region 失败信息，需要入库
                        caseDao.AddExceptionData(data);
                        #endregion
                    }
                    exceptionList = null;
                }
                if (lst_sc.Count > 0)
                {
                    #region List中的对象数到15条，就执行一次入库
                    record.ImportCaseNumber = BatchCaseData(psc, lst_sc, item.CaseTable);
                    lst_sc = new List<DataCase>();
                    #endregion
                    caseCount.Add(item.CityName, record.ImportCaseNumber);
                }
                if (record.ImportCaseNumber>0 || record.ExceptionCaseNumber>0)
                {
                    int r = record.Insert();
                    if (r != 1)
                    {
                        logger.Error("入库记录保存失败");
                    }
                }
                lst_project = null;
                projNetworkNames = null;
                cacheProjectIds = null;
                if (isRemoveData)
                {
                    //从源库删除这些已经处理转换过的案例
                    Task.Factory.StartNew(() => {
                        int cnt = caseDao.RemoveDataCase(paramMap);
                        logger.Info("当前批量删除已经处理转换过的案例DataCase[" + cnt + "]行数据...");
                    });
                }

                Console.WriteLine(item.CityName + " 转换数据结束时间= " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                logger.InfoFormat("{0} 转换数据结束时间= {1}", item.CityName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            Console.WriteLine("================>> 数据转换完成，时间 = " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            logger.InfoFormat("================================================>> 数据转换完成，时间 = {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            return caseCount;
        }

        private static int BatchCaseData(IProxy psc, List<DataCase> lst_sc, string tableName)
        {
            if (lst_sc.Count > 0)
            {
                RemoveDuplicate(lst_sc);//去除重复数据
                if (lst_sc.Count > 0)
                    PriceFilter(lst_sc);//价格过滤
            }
            
            //批量入库，返回成功插入数据库的记录数
            int insertCount = 0;
            int batchCount = 5000;
            int n = lst_sc.Count / batchCount;
            if (lst_sc.Count % batchCount != 0)
                n++;
            for (int i = 0; i < n; i++)
            {
                DataCase[] dc_tmp = lst_sc.Skip(i * batchCount).Take(batchCount).ToArray();
                var c = psc.BatchInsertDataCase(dc_tmp, tableName).ValidateStatus<BatchInsertResponseModel>().Count;
                insertCount += c;
                logger.Info("当前批量插入[" + c + "]行数据...");
            }

            return insertCount;
        }

        private static void RemoveDuplicate(List<DataCase> lst_sc)
        {
            if (lst_sc == null || lst_sc.Count == 0)
                return;
            List<int> lst_sc_dup = new List<int>();
            Dictionary<string, DataCase> tempDict = new Dictionary<string, DataCase>();
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

        private static void PriceFilter(List<DataCase> lst_sc)
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
                string key1 = string.Format("{0}{1}", item.ProjectName, item.PurposeName);
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
