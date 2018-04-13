using CBSS.Core.Utility;
using CBSS.IBS.Contract.IBSResource;
using CBSS.Tbx.Contract.DataModel;
using CBSS.Tbx.Contract.ViewModel;
using CBSS.Tbx.IBLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.BLL
{
    public partial class TbxService : ITbxService
    {
        //public IEnumerable<v_ListSubModules_0> GetHearResource(int bookId, int catalogId)
        //{
        //    var resource = ibsService.GetALLFollowRead(bookId, catalogId);
        //    List<FollowReadModule> list = resource.Data as List<FollowReadModule>;
        //    List<v_ListSubModules_0> hearResources = new List<v_ListSubModules_0>();
        //    list.ForEach(a =>
        //    {
        //        int i = 1;
        //        a.contents.ForEach(x =>
        //        {
        //            v_ListSubModules_0 item = new v_ListSubModules_0();
        //            item.SecondModularID = a.moduleType;
        //            item.ModularName = StringEnumHelper.GetStringValue(a.moduleType);
        //            item.ModularEN = a.moduleName;
        //            item.RepeatNumber = 3;
        //            item.SecondTitleID = 1;
        //            item.SerialNumber = x.sort;
        //            if (a.moduleType == 1003)
        //            {
        //                item.TextSerialNumber = i;
        //                i++;
        //            }
        //            hearResources.Add(item);
        //        });
        //    });
        //    return hearResources;
        //}

        public IEnumerable<v_SecondModule> GetHearResource(v_ModuleInfo param, List<v_ListSubModules_0> hearResources)
        {
            #region 目录对象
            IList<v_SecondModule> subModuleList = new List<v_SecondModule>();
            List<v_ListSubModules> listSubModules = new List<v_ListSubModules>();
            string SecondModularID = "";
            for (int i = 0; i < hearResources.Count; i++)
            {
                if (i == 0)
                {
                    SecondModularID = hearResources[i].SecondModularID.ToString();
                    listSubModules.Add(new v_ListSubModules()
                    {
                        SecondModularID = hearResources[i].SecondModularID.ToString(),
                        ModularName = hearResources[i].ModularName.ToString(),
                        ModularEN = hearResources[i].ModularEN.ToString(),
                        RepeatNumber = hearResources[i].RepeatNumber.ToString(),
                        SerialNumber = hearResources[i].SerialNumber.ToString(),
                        TextSerialNumber = hearResources[i].TextSerialNumber.ToString()
                    });
                }
                else
                {
                    if (SecondModularID != hearResources[i].SecondModularID.ToString())
                    {
                        listSubModules.Add(new v_ListSubModules()
                        {
                            SecondModularID = hearResources[i].SecondModularID.ToString(),
                            ModularName = hearResources[i].ModularName.ToString(),
                            ModularEN = hearResources[i].ModularEN.ToString(),
                            RepeatNumber = hearResources[i].RepeatNumber.ToString(),
                            SerialNumber = hearResources[i].SerialNumber.ToString(),
                            TextSerialNumber = hearResources[i].TextSerialNumber.ToString()
                        });
                        SecondModularID = hearResources[i].SecondModularID.ToString();
                    }
                }
            }
            listSubModules = listSubModules.Distinct(new ListSubModuleCompare()).ToList();
            #endregion
            string sqlString = "";
            string questString = "";
            #region 未完成模块目标的图标出现小红点，并显示还剩的未完成的目标次数；用户最优5次的成绩
            foreach (var model in listSubModules)
            {
                #region 未完成模块目标的图标出现小红点，并显示还剩的未完成的目标次数

                sqlString += "SELECT MAX(PlayTimes) FROM [FZ_CBSS_HearResourceRecords].[dbo].[TB_UserHearResources] where 1=1";
                sqlString += " and UserID = " + param.UserID.ToInt();
                sqlString += " and BookID = " + param.BookID.ToInt();
                sqlString += " and FirstTitleID = " + param.FirstTitleID.ToInt();
                if (!string.IsNullOrEmpty(param.SecondTitleID))
                {
                    sqlString += " and SecondTitleID = " + param.SecondTitleID.ToInt();
                }
                sqlString += " and SerialNumber = " + model.SerialNumber;
                if (model.TextSerialNumber != null)
                {
                    sqlString += " and TextSerialNumber = " + model.TextSerialNumber;
                }

                sqlString += ";";

                //DataSet dataSet = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sqlString);//反复连接查询改成拼接sql一次性查询
                //if (string.IsNullOrEmpty(dataSet.Tables[0].Rows[0][0].ToString()))
                //{
                //    secondModule.FinishedTimes = 0;
                //}
                //else
                //{
                //    secondModule.FinishedTimes = Convert.ToInt32(dataSet.Tables[0].Rows[0][0].ToString());
                //}
                #endregion

                #region 用户最优5次的成绩
                // questString = "";
                questString += "SELECT TOP 5  CreateTime,AverageScore FROM [FZ_CBSS_HearResourceRecords].[dbo].[TB_UserHearResources] where 1=1";
                questString += " and UserID = " + param.UserID.ToInt();
                questString += " and BookID = " + param.BookID.ToInt();
                questString += " and FirstTitleID = " + param.FirstTitleID.ToInt();
                if (!string.IsNullOrEmpty(param.SecondTitleID))
                {
                    questString += " and SecondTitleID = " + param.SecondTitleID.ToInt();
                }
                questString += " and SerialNumber = " + model.SerialNumber;
                if (model.TextSerialNumber != null)
                {
                    questString += " and TextSerialNumber = " + model.TextSerialNumber;
                }
                questString += " ORDER BY AverageScore DESC";
                questString += ";";

                //DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, questString);//反复连接查询改成拼接sql一次性查询
                //IList<TB_UserHearResources> resourcesList = DataSetToIList<TB_UserHearResources>(ds, 0);
                //foreach (var re in resourcesList)
                //{
                //    SubModuleDetails subModuleDetails = new SubModuleDetails();
                //    subModuleDetails.Date = re.CreateTime;
                //    subModuleDetails.AverageScore = re.AverageScore;
                //    secondModule.History.Add(subModuleDetails);
                //}
                // subModuleList.Add(secondModule);
                #endregion
            }
            DataSet dataSet = HearResourceRepository.SelectDataSet(sqlString);
            DataSet ds = HearResourceRepository.SelectDataSet(questString);//反复连接查询改成拼接sql一次性查询
            for (int i = 0; i < listSubModules.Count; i++)
            {
                var model = listSubModules[i];
                v_SecondModule secondModule = new v_SecondModule();
                secondModule.SecondModuleID = model.SecondModularID;
                secondModule.SecondModuleName = model.ModularName;
                secondModule.EnglishName = model.ModularEN;
                secondModule.RequiredTimes = int.Parse(model.RepeatNumber);
                secondModule.History = new List<v_SubModuleDetails>();
                if (string.IsNullOrEmpty(dataSet.Tables[i].Rows[0][0].ToString()))
                {
                    secondModule.FinishedTimes = 0;
                }
                else
                {
                    secondModule.FinishedTimes = Convert.ToInt32(dataSet.Tables[i].Rows[0][0].ToString());
                }

                List<TB_HearResources> resourcesList = DataTableHelper<TB_HearResources>.ConvertToModel(dataSet.Tables[i]).ToList();
                foreach (var re in resourcesList)
                {
                    v_SubModuleDetails subModuleDetails = new v_SubModuleDetails();
                    subModuleDetails.Date = re.CreateTime;
                    subModuleDetails.AverageScore = re.AverageScore;
                    secondModule.History.Add(subModuleDetails);
                }
                subModuleList.Add(secondModule);
            }
            return subModuleList;
            #endregion
        }


        public IEnumerable<TB_HearResources> GetUserHearResourcesList(string where)
        {
            return HearResourceRepository.SelectSearch<TB_HearResources>(where);
        }

        public bool AddResources(TB_HearResources tB_UserHearResources)
        {
            return (int)HearResourceRepository.Insert(tB_UserHearResources)>0;
        }


        public  void GetUserHearResourcesInfo(v_ModuleInfo submitData, List<v_ListenSpeakInfo> hr, out int finishedTimes)
        {

            finishedTimes = 0;
            string questString = " 1=1";
            int? textSerialNumber = hr[0].TextSerialNumber;
            questString += " and UserID = '" + (submitData.UserID) + "'";
            questString += " and BookID = '" + (submitData.BookID) + "'";
            questString += " and FirstTitleID = '" + (submitData.FirstTitleID) + "'";
            if (!string.IsNullOrEmpty(submitData.SecondTitleID))
            {
                questString += " and SecondTitleID = '" + (submitData.SecondTitleID) + "'";
            }
            questString += " and SerialNumber = " + hr[0].SerialNumber;
            if (textSerialNumber != null)
            {
                questString += " and TextSerialNumber = " + hr[0].TextSerialNumber;
            }
            //questString += " ORDER BY PlayTimes DESC ";
            string sql = "select ISNULL(max(PlayTimes),0) from [FZ_HearResources].[dbo].[TB_UserHearResources] where " + questString;
            finishedTimes = Convert.ToInt32(HearResourceRepository.SelectString(sql));
        }

        public List<HearResourceInfoModel> GetHearResourceByWeb(string UserID, int BookID, int FirstTitleID, string SecondTitleID, List<v_ListSubModules_0> list)
        {
            List<HearResourceInfoModel> resourcesList = new List<HearResourceInfoModel>();
            if (list != null && list.Count > 0)
            {

                list.ForEach(a =>
                {
                    string questString = "";
                    questString = "SELECT TOP 1 * FROM [FZ_HearResources].[dbo].[TB_UserHearResources] where 1=1";
                    questString += " and UserID = " + UserID;
                    questString += " and BookID = " + BookID;
                    questString += " and SerialNumber = " + a.SerialNumber;
                    questString += " and FirstTitleID = " + FirstTitleID;
                    if (!string.IsNullOrEmpty(SecondTitleID)&& SecondTitleID !="0")
                    {
                        questString += " and SecondTitleID = " + SecondTitleID;
                    }
                    if (a.TextSerialNumber != 0)
                    {
                        questString += " and TextSerialNumber = " + a.TextSerialNumber;
                    }
                    questString += " ORDER BY AverageScore desc";
                    DataSet ds = HearResourceRepository.SelectDataSet(questString);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count != 0)
                    {
                        TB_HearResources hearResourInfo = DataTableHelper<TB_HearResources>.ConvertToModel(ds.Tables[0]).FirstOrDefault(); ;
                        HearResourceInfoModel resourcesInfo = new HearResourceInfoModel();
                        resourcesInfo.TextDesc = a.Content;
                        resourcesInfo.TextSerialNumber = hearResourInfo.TextSerialNumber;
                        resourcesInfo.SerialNumber = hearResourInfo.SerialNumber;
                        resourcesInfo.TotalScore = hearResourInfo.TotalScore;
                        resourcesInfo.AverageScore = hearResourInfo.AverageScore;
                        resourcesInfo.CreateDate = hearResourInfo.CreateTime;
                        resourcesInfo.VideoFileID = hearResourInfo.VideoFileID;
                        resourcesList.Add(resourcesInfo);
                    }
                });
                resourcesList.Sort();

            }
            return resourcesList;
        }
    }
}
