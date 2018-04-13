using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Kingsun.IBS.IBLL.IBSLearningReport;
using Kingsun.IBS.Model.IBSLearnReport;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Common.Base;
using Kingsun.SynchronousStudy.Common.Enums;
using Kingsun.IBS.IBLL;
using Kingsun.SynchronousStudy.Models;

namespace Kingsun.IBS.BLL.IBSLearningReport
{
    public class IBSLearningReportBLL : IIBSLearningReport
    {


        /// <summary>
        /// 正常业务库
        /// </summary>
        public BaseManagement bm = new BaseManagement();

        static RedisHashHelper hashRedis = new RedisHashHelper();
        //消息队列
        static RedisListHelper listRedis = new RedisListHelper();
        IIBSData_ClassUserRelationBLL classBLL = new IBSData_ClassUserRelationBLL();
        private IIBSData_UserInfoBLL userBLL = new IBSData_UserInfoBLL();


        private IIBSInitUserVideoBLL initBLL = new IBSInitUserVideoBLL();
        private IIBSInitStuCatalogBLL StuCatalog = new IBSInitStuCatalogBLL();



        

        #region 初始化学习报告记录
        public void InitLearningReport(string kingsun)
        {

            initBLL.InitializeUserVideoInfo(kingsun);
            System.Threading.Thread.Sleep(1000);
            initBLL.InitializeBook(kingsun);
            System.Threading.Thread.Sleep(1000);
            initBLL.InitializeRdsStudyReportModule(kingsun);

        }

        public void RemoveRedis()
        {
            initBLL.RemoveRedis();
        }

        public void InitLearningReportModuleTitle(string kingsun)
        {
            initBLL.InitializeRdsStudyReportModuleTitle(kingsun);
        }

        public void InitLearningReportBookCatalogues(string kingsun)
        {
            initBLL.initStudyReportBookCatalogues(kingsun);
        }

        public void InitLearningReportUserInfo(string kingsun)
        {
            StuCatalog.InitializeUserInfo(kingsun);
        }

        #endregion

        #region 初始化发布过后的数据
        public void TodayInitLearningReport(string kingsun)
        {

            initBLL.TodayInitializeUserVideoInfo(kingsun);
            System.Threading.Thread.Sleep(1000);
            initBLL.TodayInitializeBook(kingsun);
            System.Threading.Thread.Sleep(1000);
            initBLL.TodayInitializeRdsStudyReportModule(kingsun);

        }


        public void TodayInitLearningReportModuleTitle(string kingsun)
        {
            initBLL.TodayInitializeRdsStudyReportModuleTitle(kingsun);
        }

        public void TodayInitLearningReportBookCatalogues(string kingsun)
        {
            initBLL.TodayinitStudyReportBookCatalogues(kingsun);
        }

        public void TodayInitLearningReportUserInfo(string kingsun)
        {
            StuCatalog.TodayInitializeUserInfo(kingsun);
        }
        #endregion

        public void ExecuteLearningReport()
        {
            var listCount = listRedis.Count("LearningReportQueue");
            int Count = Convert.ToInt32(listCount) > 1000 ? 1000 : Convert.ToInt32(listCount);
            using (var Redis = RedisManager.GetClient(0))
            {
                for (int i = 0; i < Count; i++)
                {
                    var model = Redis.RemoveStartFromList("LearningReportQueue");
                    ;
                    try
                    {
                        RedisVideoInfo data = JsonHelper.DecodeJson<RedisVideoInfo>(model);
                        switch (data.ModuleType)
                        {
                            case "1":
                                ExecuteInterestDebbingLearningReport(data);
                                break;
                            case "2":
                                ExecuteExampaperLearningReport(data);
                                break;
                            case "3":
                                ExecuteHearResourcesLearningReport(data);
                                break;

                            //优学
                            case "4":
                                ExecuteInterestDebbingLearningReport(data, 4);
                                break;
                            case "5":
                                ExecuteExampaperLearningReport(data, 5);
                                break;
                            case "6":
                                ExecuteHearResourcesLearningReport(data, 6);
                                break;
                        }

                    }
                    catch (Exception ex)
                    {
                        Log4Net.LogHelper.Error(ex, "学习报告同步异常,异常数据：" + model);
                    }
                }
            }
        }



        #region 班级变更后,学习报告处理
        /// <summary>
        /// 学生关系变更后，学习报告数据变更
        /// </summary>
        public void ExecuteStudentClassChangeLearningReport()
        {
            var listCount = listRedis.Count("StudentClassRelationKey");
            int Count = Convert.ToInt32(listCount) > 1000 ? 1000 : Convert.ToInt32(listCount);
            using (var Redis = RedisManager.GetClient(0))
            {
                for (int i = 0; i < Count; i++)
                {
                    var model = Redis.RemoveStartFromList("StudentClassRelationKey"); 
                    try
                    {
                        if (!string.IsNullOrEmpty(model))
                        {
                            StudentClassRelationKey data = JsonHelper.DecodeJson<StudentClassRelationKey>(model);

                            switch (data.type)
                            {

                                case 1: //绑定
                                    AddNewLearningReport(data);
                                    break;
                                case 2: //解绑
                                    CheckAndUpdateLearningReport(data);
                                    break;
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        Log4Net.LogHelper.Error(ex, "班级变更的学习报告变更失败");
                    }
                }
            }

        }

        /// <summary>
        /// 查询原班级是否有学习报告  以及转移学生是否为班级最后一份学习报告
        /// </summary>
        /// <param name="data"></param>
        private void CheckAndUpdateLearningReport(StudentClassRelationKey data)
        {

            //通过原班级ID查询原班级现有学生
            var classinfo = classBLL.GetClassUserRelationByClassId(data.ClassID);
            if (classinfo != null)
            {
                data.ClassID = classinfo.ClassNum;
                bool InterestDebbingState = false;
                bool ExamPaperState = false;
                bool HearResourcesState = false;
                //遍历学生查询学生是否有学习报告
                classinfo.ClassStuList.ForEach(a =>
                {

                    var InterestRecord = hashRedis.Get<Rds_StudyReport_ModuleTitle>("Rds_StudyReport_ModuleTitle_" + a.StuID.ToString().Substring(0, 2),
                     a.StuID + "_" + (int)SubjectEnum.English + "_" + (int)ModuleTypeEnum.InterestDebbing);
                    //若原班级有学生有学习报告则无需处理
                    if (InterestRecord != null)
                    {
                        InterestDebbingState = true;
                    }

                    //若原班级有学生有学习报告则无需处理
                    var ExamRecord = hashRedis.Get<Rds_StudyReport_ModuleTitle>("Rds_StudyReport_ModuleTitle_" + a.StuID.ToString().Substring(0, 2),
                     a.StuID + "_" + (int)SubjectEnum.English + "_" + (int)ModuleTypeEnum.ExamPaper);
                    if (ExamRecord != null)
                    {
                        ExamPaperState = true;
                    }

                    var HearResources = hashRedis.Get<Rds_StudyReport_ModuleTitle>("Rds_StudyReport_ModuleTitle_" + a.StuID.ToString().Substring(0, 2),
                        a.StuID + "_" + (int)SubjectEnum.English + "_" + (int)ModuleTypeEnum.HearResources);
                    //若原班级有学生有学习报告则无需处理
                    if (HearResources != null)
                    {
                        HearResourcesState = true;
                    }

                });

                //如果原班级无人产生说说看报告则删除转移学生产生的记录
                if (!HearResourcesState)
                {
                    var hearResources = hashRedis.Get<Rds_StudyReport_ModuleTitle>("Rds_StudyReport_ModuleTitle_" + data.UserID.ToString().Substring(0, 2),
                        data.UserID + "_" + (int)SubjectEnum.English + "_" + (int)ModuleTypeEnum.HearResources);
                    if (hearResources != null)
                    {
                        hearResources.detail.ForEach(x =>
                        {
                            hashRedis.Remove("Rds_StudyReport_Book", data.ClassID + "_" + (int)SubjectEnum.English + "_" + (int)ModuleTypeEnum.HearResources + "_" + x.BookID);
                            if (x.SecondTitleID > 0)
                            {
                                hashRedis.Remove("Rds_StudyReport_Module",
                                    data.ClassID + "_" + (int)SubjectEnum.English + "_" + (int)ModuleTypeEnum.HearResources + "_" + x.BookID + "_" +
                                    x.FirstTitleID + x.SecondTitleID);
                            }
                            else
                            {
                                hashRedis.Remove("Rds_StudyReport_Module",
                                    data.ClassID + "_" + (int)SubjectEnum.English + "_" + (int)ModuleTypeEnum.HearResources + "_" + x.BookID + "_" +
                                    x.FirstTitleID);
                            }

                        });

                    }
                    hashRedis.Remove("Rds_StudyReport_Class", data.ClassID + "_" + (int)SubjectEnum.English + "_" + (int)ModuleTypeEnum.HearResources);

                }

                //如果原班级无人产生报告则删除转移学生产生的记录
                if (!InterestDebbingState)
                {
                    var learningRecord = hashRedis.Get<Rds_StudyReport_ModuleTitle>("Rds_StudyReport_ModuleTitle_" + data.UserID.ToString().Substring(0, 2),
                    data.UserID + "_" + (int)SubjectEnum.English + "_" + (int)ModuleTypeEnum.InterestDebbing);
                    if (learningRecord != null)
                    {
                        learningRecord.detail.ForEach(x =>
                        {
                            hashRedis.Remove("Rds_StudyReport_Book", data.ClassID + "_" + (int)SubjectEnum.English + "_" + (int)ModuleTypeEnum.InterestDebbing + "_" + x.BookID);
                            if (x.SecondTitleID > 0)
                            {
                                hashRedis.Remove("Rds_StudyReport_Module",
                            data.ClassID + "_" + (int)SubjectEnum.English + "_" + (int)ModuleTypeEnum.InterestDebbing + "_" + x.BookID + "_" +
                            x.FirstTitleID + x.SecondTitleID);
                            }
                            else
                            {
                                hashRedis.Remove("Rds_StudyReport_Module",
                           data.ClassID + "_" + (int)SubjectEnum.English + "_" + (int)ModuleTypeEnum.InterestDebbing + "_" + x.BookID + "_" +
                           x.FirstTitleID);
                            }

                        });

                    }
                    hashRedis.Remove("Rds_StudyReport_Class", data.ClassID + "_" + (int)SubjectEnum.English + "_" + (int)ModuleTypeEnum.InterestDebbing);

                }

                //如果原班级无人产生报告则删除转移学生产生的记录
                if (!ExamPaperState)
                {
                    hashRedis.Remove("Rds_StudyReport_Class", data.ClassID + "_" + (int)SubjectEnum.English + "_" + (int)ModuleTypeEnum.ExamPaper);
                    var learningRecord = hashRedis.Get<Rds_StudyReport_ModuleTitle>("Rds_StudyReport_ModuleTitle_" + data.UserID.ToString().Substring(0, 2),
                   data.UserID + "_" + (int)SubjectEnum.English + "_" + (int)ModuleTypeEnum.InterestDebbing);
                    if (learningRecord != null)
                    {
                        learningRecord.detail.ForEach(x =>
                        {
                            hashRedis.Remove("Rds_StudyReport_Book", data.ClassID + "_" + (int)SubjectEnum.English + "_" + (int)ModuleTypeEnum.InterestDebbing + "_" + x.BookID);

                        });

                    }
                }
            }
        }



        /// <summary>
        /// 班级变更后，新增学习报告记录
        /// </summary>
        /// <param name="data"></param>
        private void AddNewLearningReport(StudentClassRelationKey data)
        {
            var classinfo = classBLL.GetClassUserRelationByClassId(data.ClassID);
            if (classinfo != null)
            {
                data.ClassID = classinfo.ClassNum;
            }
            else
            {
                return;
            }

            #region 趣配音
            var learningRecord = hashRedis.Get<Rds_StudyReport_ModuleTitle>("Rds_StudyReport_ModuleTitle_" + data.UserID.ToString().Substring(0, 2),
                       data.UserID + "_" + (int)SubjectEnum.English + "_" + (int)ModuleTypeEnum.InterestDebbing);
            if (learningRecord != null)
            {
                if (learningRecord.detail.Count > 0)
                {
                    RedisVideoInfo info = new RedisVideoInfo();
                    info.ClassID = data.ClassID;
                    #region 修改班级报告
                    //更新或新增班级报告
                    CreateOrUpdateRds_StudyReport_Class(info, (int)ModuleTypeEnum.InterestDebbing);
                    #endregion

                    learningRecord.detail.ForEach(a =>
                    {

                        info.BookId = a.BookID.ToString();
                        info.VideoNumber = a.VideoNumber.ToString();
                        #region 修改书本报告
                        //更新或新增书本报告
                        CreateOrUpdateRds_StudyReport_Book(info, (int)ModuleTypeEnum.InterestDebbing);
                        #endregion

                        #region 修改模块报告


                        var title = hashRedis.Get<Rds_StudyReport_BookCatalogues_BookID>("Rds_StudyReport_BookCatalogues_" + a.BookID,
                                a.FirstTitleID + "_" + a.SecondTitleID);
                        if (title != null)
                        {
                            info.FirstTitle = title.FirstTitle;
                            info.SecondTitle = title.SecondTitle;
                            info.FirstModular = title.FirstModular;
                        }


                        info.FirstTitleID = a.FirstTitleID > 0 ? a.FirstTitleID.ToString() : "";
                        info.FirstModularID = a.FirstModularID > 0 ? a.FirstModularID.ToString() : "";
                        info.SecondTitleID = a.SecondTitleID > 0 ? a.SecondTitleID.ToString() : "";
                        CreateOrUpdateRds_StudyReport_ModuleForInterestDebbing(info, (int)ModuleTypeEnum.InterestDebbing);
                        #endregion
                    });
                }

            }
            #endregion

            #region 单元测试
            var examRecord = hashRedis.Get<Rds_StudyReport_ModuleTitle>("Rds_StudyReport_ModuleTitle_" + data.UserID.ToString().Substring(0, 2),
                       data.UserID + "_" + (int)SubjectEnum.English + "_" + (int)ModuleTypeEnum.ExamPaper);
            if (examRecord != null)
            {
                if (examRecord.detail.Count > 0)
                {
                    RedisVideoInfo info = new RedisVideoInfo();
                    info.ClassID = data.ClassID;

                    #region 修改班级报告

                    //更新或新增班级报告
                    CreateOrUpdateRds_StudyReport_Class(info, (int)ModuleTypeEnum.ExamPaper);

                    #endregion

                    examRecord.detail.ForEach(a =>
                    {

                        info.BookId = a.BookID.ToString();
                        #region 修改书本报告

                        //更新或新增书本报告
                        CreateOrUpdateRds_StudyReport_Book(info, (int)ModuleTypeEnum.ExamPaper);

                        #endregion
                    });

                }
            }

            #endregion

            #region 说说看
            var HearResources = hashRedis.Get<Rds_StudyReport_ModuleTitle>("Rds_StudyReport_ModuleTitle_" + data.UserID.ToString().Substring(0, 2),
                data.UserID + "_" + (int)SubjectEnum.English + "_" + (int)ModuleTypeEnum.HearResources);
            if (HearResources != null)
            {
                if (HearResources.detail.Count > 0)
                {
                    RedisVideoInfo info = new RedisVideoInfo();
                    info.ClassID = data.ClassID;
                    #region 修改班级报告
                    //更新或新增班级报告
                    CreateOrUpdateRds_StudyReport_Class(info, (int)ModuleTypeEnum.HearResources);
                    #endregion

                    HearResources.detail.ForEach(a =>
                    {

                        info.BookId = a.BookID.ToString();
                        info.VideoNumber = a.VideoNumber.ToString();
                        #region 修改书本报告
                        //更新或新增书本报告
                        CreateOrUpdateRds_StudyReport_Book(info, (int)ModuleTypeEnum.HearResources);
                        #endregion

                        #region 修改模块报告


                        var title = hashRedis.Get<Rds_StudyReport_BookCatalogues_BookID>("Rds_StudyReport_BookCatalogues_" + a.BookID,
                                a.FirstTitleID + "_" + a.SecondTitleID);
                        if (title != null)
                        {
                            info.FirstTitle = title.FirstTitle;
                            info.SecondTitle = title.SecondTitle;
                            info.FirstModular = title.FirstModular;
                        }


                        info.FirstTitleID = a.FirstTitleID > 0 ? a.FirstTitleID.ToString() : "";
                        info.FirstModularID = a.FirstModularID > 0 ? a.FirstModularID.ToString() : "";
                        info.SecondTitleID = a.SecondTitleID > 0 ? a.SecondTitleID.ToString() : "";
                        CreateOrUpdateRds_StudyReport_ModuleForInterestDebbing(info, (int)ModuleTypeEnum.HearResources);
                        #endregion
                    });
                }

            }

            #endregion
        }

        #endregion



        #region 定时服务，生成学习报告

        /// <summary>
        /// 优学说说看学习报告
        /// </summary>
        private void ExecuteHearResourcesLearningReport(RedisVideoInfo data,int ModuleType)
        {
            var user = userBLL.GetUserInfoByUserId(Convert.ToInt32(data.UserId));
            if (user != null)
            {
                var classinfo = user.ClassSchList.FirstOrDefault();
                if (classinfo != null)
                {
                    var ibsclass = classBLL.GetClassUserRelationByClassId(classinfo.ClassID);
                    if (ibsclass != null)
                    {
                        data.ClassID = ibsclass.ClassNum;
                    }
                }
            }
            if (!string.IsNullOrEmpty(data.ClassID) && data.ClassID != "0")
            {

                CreateOrUpdateRds_StudyReport_Class(data, ModuleType);
                CreateOrUpdateRds_StudyReport_Book(data, ModuleType);
                CreateOrUpdateRds_StudyReport_ModuleForInterestDebbing(data, ModuleType);
            }
            ///以ClassID+SubjectID+BookID+VideoNumber作为Key的模块报告
            CreateOrUpdateRds_StudyReport_ModuleTitleForHearResources(data, ModuleType);
        }

        /// <summary>
        /// 说说看学习报告
        /// </summary>
        private void ExecuteHearResourcesLearningReport(RedisVideoInfo data)
        {
            var user = userBLL.GetUserInfoByUserId(Convert.ToInt32(data.UserId));
            if (user != null)
            {
                var classinfo = user.ClassSchList.FirstOrDefault();
                if (classinfo != null)
                {
                    var ibsclass = classBLL.GetClassUserRelationByClassId(classinfo.ClassID);
                    if (ibsclass != null)
                    {
                        data.ClassID = ibsclass.ClassNum;
                    }
                }
            }
            if (!string.IsNullOrEmpty(data.ClassID) && data.ClassID != "0")
            {

                CreateOrUpdateRds_StudyReport_Class(data, (int)ModuleTypeEnum.HearResources);
                CreateOrUpdateRds_StudyReport_Book(data, (int)ModuleTypeEnum.HearResources);
                CreateOrUpdateRds_StudyReport_ModuleForInterestDebbing(data, (int)ModuleTypeEnum.HearResources);
            }
            ///以ClassID+SubjectID+BookID+VideoNumber作为Key的模块报告
            CreateOrUpdateRds_StudyReport_ModuleTitleForHearResources(data, (int)ModuleTypeEnum.HearResources);
        }


        /// <summary>
        // 优学单元测试（暂无）
        /// </summary>
        /// <param name="data"></param>
        private void ExecuteExampaperLearningReport(RedisVideoInfo data,int ModuleType)
        {

            /* var user = userBLL.GetUserInfoByUserId(Convert.ToInt32(data.UserId));
             if (user != null)
             {
                 var classinfo = user.ClassSchList.FirstOrDefault();
                 if (classinfo != null)
                 {
                     var ibsclass = classBLL.GetClassUserRelationByClassId(classinfo.ClassID);
                     if (ibsclass != null)
                     {
                         data.ClassID = ibsclass.ClassNum;
                     }
                 }
             }

             if (!string.IsNullOrEmpty(data.ClassID) && data.ClassID != "0")
             {*/

            //CreateOrUpdateRds_StudyReport_Class(data, (int)ModuleTypeEnum.ExamPaper);
            // CreateOrUpdateRds_StudyReport_Book(data, (int)ModuleTypeEnum.ExamPaper);
            try
            {
                CreateOrUpdateRds_StudyReport_ModuleTitleForExamPaper(data, ModuleType);
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "ExecuteExampaperLearningReport! data=" + data.ToJson());
            }

           

            //}
        }

        /// <summary>
        // 单元测试
        /// </summary>
        /// <param name="data"></param>
        private void ExecuteExampaperLearningReport(RedisVideoInfo data)
        {

            /* var user = userBLL.GetUserInfoByUserId(Convert.ToInt32(data.UserId));
             if (user != null)
             {
                 var classinfo = user.ClassSchList.FirstOrDefault();
                 if (classinfo != null)
                 {
                     var ibsclass = classBLL.GetClassUserRelationByClassId(classinfo.ClassID);
                     if (ibsclass != null)
                     {
                         data.ClassID = ibsclass.ClassNum;
                     }
                 }
             }

             if (!string.IsNullOrEmpty(data.ClassID) && data.ClassID != "0")
             {*/

            //CreateOrUpdateRds_StudyReport_Class(data, (int)ModuleTypeEnum.ExamPaper);
            // CreateOrUpdateRds_StudyReport_Book(data, (int)ModuleTypeEnum.ExamPaper);
           
            try
            {
                CreateOrUpdateRds_StudyReport_ModuleTitleForExamPaper(data, (int)ModuleTypeEnum.ExamPaper);
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "ExecuteExampaperLearningReport! data=" + data.ToJson());
            }


            //}
        }


        /// <summary>
        /// 优学趣配音
        /// </summary>
        /// <param name="data"></param>
        private void ExecuteInterestDebbingLearningReport(RedisVideoInfo data,int ModuleType)
        {
            try
            {
                //更新或新增班级报告
                CreateOrUpdateRds_StudyReport_Class(data, ModuleType);

                //更新或新增书本报告
                CreateOrUpdateRds_StudyReport_Book(data, ModuleType);

                CreateOrUpdateRds_StudyReport_ModuleForInterestDebbing(data, ModuleType);

                ///以ClassID+SubjectID+BookID+VideoNumber作为Key的模块报告
                CreateOrUpdateRds_StudyReport_ModuleTitleForInterestDebbing(data, ModuleType);
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "ExecuteInterestDebbingLearningReport! data=" + data.ToJson());
            }
        }


        /// <summary>
        /// 趣配音
        /// </summary>
        /// <param name="data"></param>
        private void ExecuteInterestDebbingLearningReport(RedisVideoInfo data)
        {
            try
            {
                //更新或新增班级报告
                CreateOrUpdateRds_StudyReport_Class(data, (int)ModuleTypeEnum.InterestDebbing);

                //更新或新增书本报告
                CreateOrUpdateRds_StudyReport_Book(data, (int)ModuleTypeEnum.InterestDebbing);

                CreateOrUpdateRds_StudyReport_ModuleForInterestDebbing(data, (int)ModuleTypeEnum.InterestDebbing);

                ///以ClassID+SubjectID+BookID+VideoNumber作为Key的模块报告
                CreateOrUpdateRds_StudyReport_ModuleTitleForInterestDebbing(data, (int)ModuleTypeEnum.InterestDebbing);
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "ExecuteInterestDebbingLearningReport! data="+data.ToJson());
            }
        }


        /// <summary>
        /// 单元测试报告新增
        /// </summary>
        /// <param name="data"></param>
        /// <param name="ModuleType"></param>
        private void CreateOrUpdateRds_StudyReport_ModuleTitleForExamPaper(RedisVideoInfo data, int ModuleType)
        {
            var learningRecord = hashRedis.Get<Rds_StudyReport_ModuleTitle>("Rds_StudyReport_ModuleTitle_" + data.UserId.Substring(0, 2),
                data.UserId + "_" + (int)SubjectEnum.English + "_" + ModuleType);
            if (learningRecord != null)
            {
                var record =
                    learningRecord.detail.FirstOrDefault(
                        a =>
                            a.BookID == Convert.ToInt32(data.BookId) &&
                            a.VideoNumber == Convert.ToInt32(data.VideoNumber));
                if (record != null)
                {
                    if (double.Parse(data.TotalScore) > record.BestScore)
                    {
                        //具体操作
                        record.BestScore = double.Parse(data.TotalScore);
                        record.CreateTime = data.CreateTime;
                        record.VideoNumber = data.VideoNumber.ToInt();
                        record.VideoID = data.VideoID;
                        record.DubbingNum = data.DubbingNum;
                    }
                    else
                    {
                        record.DubbingNum += 1;
                    }
                }
                else
                {
                    learningRecord.detail.Add(new Rds_StudyReport_BookDetail()
                    {
                        //具体操作
                        BestScore = double.Parse(data.TotalScore),
                        CreateTime = data.CreateTime,
                        VideoNumber = data.VideoNumber.ToInt(),
                        VideoID = data.VideoID,
                        BookID = data.BookId.ToInt(),
                        DubbingNum = data.DubbingNum
                    });
                }
            }
            else
            {
                learningRecord = new Rds_StudyReport_ModuleTitle();
                learningRecord.UserID = data.UserId.ToInt();
                learningRecord.detail.Add(new Rds_StudyReport_BookDetail()
                {
                    //具体操作
                    BestScore = double.Parse(data.TotalScore),
                    CreateTime = data.CreateTime,
                    VideoID = data.VideoID,
                    VideoNumber = data.VideoNumber.ToInt(),
                    BookID = data.BookId.ToInt(),
                    DubbingNum = data.DubbingNum
                });

            }
            hashRedis.Set<Rds_StudyReport_ModuleTitle>("Rds_StudyReport_ModuleTitle_" + data.UserId.Substring(0, 2),
                data.UserId + "_" + (int)SubjectEnum.English + "_" + ModuleType, learningRecord);
        }

        /// <summary>
        /// 趣配音报告新增
        /// </summary>
        /// <param name="data"></param>
        /// <param name="ModuleType"></param>
        private void CreateOrUpdateRds_StudyReport_ModuleTitleForInterestDebbing(RedisVideoInfo data, int ModuleType)
        {
            try
            {
                var learningRecord = hashRedis.Get<Rds_StudyReport_ModuleTitle>("Rds_StudyReport_ModuleTitle_" + data.UserId.Substring(0, 2),
                    data.UserId + "_" + (int)SubjectEnum.English + "_" + ModuleType);
                if (learningRecord != null)
                {
                    var record =
                        learningRecord.detail.FirstOrDefault(
                            a =>
                                a.BookID == Convert.ToInt32(data.BookId) &&
                                a.VideoNumber == Convert.ToInt32(data.VideoNumber));
                    if (record != null)
                    {
                        record.DubbingNum += 1;
                        if (double.Parse(data.TotalScore) > record.BestScore)
                        {
                            //具体操作
                            record.BestScore = double.Parse(data.TotalScore);
                            record.CreateTime = data.CreateTime;
                            record.VideoNumber = data.VideoNumber.ToInt();
                            record.VideoID = data.VideoID;
                        }
                    }
                    else
                    {
                        learningRecord.detail.Add(new Rds_StudyReport_BookDetail()
                        {
                            //具体操作
                            BestScore = double.Parse(data.TotalScore),
                            CreateTime = data.CreateTime,
                            VideoNumber = data.VideoNumber.ToInt(),
                            BookID = data.BookId.ToInt(),
                            VideoID = data.VideoID,
                            FirstTitleID = data.FirstTitleID.ToInt(),
                            SecondTitleID = data.SecondTitleID.ToInt(),
                            FirstModularID = string.IsNullOrEmpty(data.FirstModularID) ? 0 : data.FirstModularID.ToInt(),
                            DubbingNum = 1,
                            SecondModularID = string.IsNullOrEmpty(data.SecondModularID) ? 0 : data.SecondModularID.ToInt()
                        });
                        var title = hashRedis.Get<Rds_StudyReport_BookCatalogues_BookID>("Rds_StudyReport_BookCatalogues_" + data.BookId,
                                                 data.FirstTitleID + "_" + data.SecondTitleID + "_" + ModuleType);
                        if (title != null)
                        {
                            title.FirstTitle = data.FirstTitle;
                            title.SecondTitle = data.SecondTitle;
                            title.FirstModular = data.FirstModular;

                            var video = title.Videos.FirstOrDefault(a => a.VideoNumber == data.VideoNumber.ToInt());
                            if (video != null)
                            {
                                video.VideoTitle = data.VideoTitle;
                                video.IsEnableOss = data.IsEnableOss;
                                video.VideoImageAddress = data.VideoImageAddress;
                                video.VideoNumber = data.VideoNumber.ToInt();
                                video.SecondModularID = string.IsNullOrEmpty(data.SecondModularID) ? 0 : data.SecondModularID.ToInt();
                            }
                            else
                            {
                                title.Videos.Add(new Rds_StudyReport_BookCatalogues_Video()
                                {
                                    VideoNumber = data.VideoNumber.ToInt(),
                                    VideoTitle = data.VideoTitle,
                                    VideoImageAddress = data.VideoImageAddress,
                                    IsEnableOss = data.IsEnableOss,
                                    SecondModularID = string.IsNullOrEmpty(data.SecondModularID) ? 0 : data.SecondModularID.ToInt()
                                });
                            }
                        }
                        else
                        {
                            title = new Rds_StudyReport_BookCatalogues_BookID();
                            title.FirstTitleID = data.FirstTitleID.ToInt();
                            title.FirstTitle = data.FirstTitle;
                            title.SecondTitleID = data.SecondTitleID.ToInt();
                            title.SecondTitle = data.SecondTitle;
                            title.FirstModularID = string.IsNullOrEmpty(data.FirstModularID) ? 0 : data.FirstModularID.ToInt();
                            title.FirstModular = data.FirstModular;
                            var video = title.Videos.FirstOrDefault(a => a.VideoNumber == data.VideoNumber.ToInt());
                            if (video != null)
                            {
                                video.VideoTitle = data.VideoTitle;
                                video.IsEnableOss = data.IsEnableOss;
                                video.VideoImageAddress = data.VideoImageAddress;
                                video.SecondModularID = string.IsNullOrEmpty(data.SecondModularID) ? 0 : data.SecondModularID.ToInt();
                            }
                            else
                            {
                                title.Videos.Add(new Rds_StudyReport_BookCatalogues_Video()
                                {
                                    VideoNumber = data.VideoNumber.ToInt(),
                                    VideoTitle = data.VideoTitle,
                                    VideoImageAddress = data.VideoImageAddress,
                                    IsEnableOss = data.IsEnableOss,
                                    SecondModularID = string.IsNullOrEmpty(data.SecondModularID) ? 0 : data.SecondModularID.ToInt()
                                });
                            }
                        }
                        hashRedis.Set<Rds_StudyReport_BookCatalogues_BookID>("Rds_StudyReport_BookCatalogues_" + data.BookId,
                              data.FirstTitleID + "_" + data.SecondTitleID + "_" + ModuleType, title);
                    }
                }
                else
                {
                    learningRecord = new Rds_StudyReport_ModuleTitle();
                    learningRecord.UserID = data.UserId.ToInt();
                    learningRecord.detail.Add(new Rds_StudyReport_BookDetail()
                    {
                        //具体操作
                        BestScore = double.Parse(data.TotalScore),
                        CreateTime = data.CreateTime,
                        VideoNumber = data.VideoNumber.ToInt(),
                        BookID = data.BookId.ToInt(),
                        VideoID = data.VideoID,
                        FirstTitleID = data.FirstTitleID.ToInt(),
                        SecondTitleID = data.SecondTitleID.ToInt(),
                        FirstModularID = string.IsNullOrEmpty(data.FirstModularID) ? 0 : data.FirstModularID.ToInt(),
                        DubbingNum = 1,
                        SecondModularID = string.IsNullOrEmpty(data.SecondModularID) ? 0 : data.SecondModularID.ToInt()
                    });
                    var title = hashRedis.Get<Rds_StudyReport_BookCatalogues_BookID>("Rds_StudyReport_BookCatalogues_" + data.BookId,
                              data.FirstTitleID + "_" + data.SecondTitleID + "_" + ModuleType);
                    if (title != null)
                    {
                        title.FirstTitle = data.FirstTitle;
                        title.SecondTitle = data.SecondTitle;
                        title.FirstModular = data.FirstModular;
                        var video = title.Videos.FirstOrDefault(a => a.VideoNumber == data.VideoNumber.ToInt());
                        if (video != null)
                        {
                            video.VideoTitle = data.VideoTitle;
                            video.IsEnableOss = data.IsEnableOss;
                            video.VideoImageAddress = data.VideoImageAddress;
                            video.SecondModularID = string.IsNullOrEmpty(data.SecondModularID) ? 0 : data.SecondModularID.ToInt();
                        }
                        else
                        {
                            title.Videos.Add(new Rds_StudyReport_BookCatalogues_Video()
                            {
                                VideoNumber = data.VideoNumber.ToInt(),
                                VideoTitle = data.VideoTitle,
                                VideoImageAddress = data.VideoImageAddress,
                                IsEnableOss = data.IsEnableOss,
                                SecondModularID = string.IsNullOrEmpty(data.SecondModularID) ? 0 : data.SecondModularID.ToInt()
                            });
                        }
                    }
                    else
                    {
                        title = new Rds_StudyReport_BookCatalogues_BookID();
                        title.FirstTitleID = data.FirstTitleID.ToInt();
                        title.FirstTitle = data.FirstTitle;
                        title.SecondTitleID = data.SecondTitleID.ToInt();
                        title.SecondTitle = data.SecondTitle;
                        title.FirstModularID = string.IsNullOrEmpty(data.FirstModularID) ? 0 : data.FirstModularID.ToInt();
                        title.FirstModular = data.FirstModular;
                        var video = title.Videos.FirstOrDefault(a => a.VideoNumber == data.VideoNumber.ToInt());
                        if (video != null)
                        {
                            video.VideoTitle = data.VideoTitle;
                            video.IsEnableOss = data.IsEnableOss;
                            video.VideoImageAddress = data.VideoImageAddress;
                            video.SecondModularID = string.IsNullOrEmpty(data.SecondModularID) ? 0 : data.SecondModularID.ToInt();
                        }
                        else
                        {
                            title.Videos.Add(new Rds_StudyReport_BookCatalogues_Video()
                            {
                                VideoNumber = data.VideoNumber.ToInt(),
                                VideoTitle = data.VideoTitle,
                                VideoImageAddress = data.VideoImageAddress,
                                IsEnableOss = data.IsEnableOss,
                                SecondModularID = string.IsNullOrEmpty(data.SecondModularID) ? 0 : data.SecondModularID.ToInt()
                            });
                        }
                    }
                    hashRedis.Set<Rds_StudyReport_BookCatalogues_BookID>("Rds_StudyReport_BookCatalogues_" + data.BookId,
                          data.FirstTitleID + "_" + data.SecondTitleID + "_" + ModuleType, title);
                }
                hashRedis.Set<Rds_StudyReport_ModuleTitle>("Rds_StudyReport_ModuleTitle_" + data.UserId.Substring(0, 2),
                    data.UserId + "_" + (int)SubjectEnum.English + "_" + ModuleType, learningRecord);
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "CreateOrUpdateRds_StudyReport_ModuleTitleForInterestDebbing!data="+data.ToJson());
            }
        }



        /// <summary>
        /// 趣配音报告新增
        /// </summary>
        /// <param name="data"></param>
        /// <param name="ModuleType"></param>
        private void CreateOrUpdateRds_StudyReport_ModuleTitleForHearResources(RedisVideoInfo data, int ModuleType)
        {
            try
            {
                var learningRecord = hashRedis.Get<Rds_StudyReport_ModuleTitle>("Rds_StudyReport_ModuleTitle_" + data.UserId.Substring(0, 2),
                    data.UserId + "_" + (int)SubjectEnum.English + "_" + ModuleType);
                if (learningRecord != null)
                {
                    //Rds_StudyReport_BookDetail record=new Rds_StudyReport_BookDetail();
                    //if (string.IsNullOrEmpty(data.SecondTitleID))
                    //{
                    //    record =
                    //        learningRecord.detail.FirstOrDefault(
                    //            a =>
                    //                a.BookID == Convert.ToInt32(data.BookId) &&
                    //                a.FirstTitleID == Convert.ToInt32(data.FirstTitleID) &&
                    //                a.SecondTitleID == 0
                    //                && a.FirstModularID == Convert.ToInt32(data.FirstModularID) &&
                    //                a.SecondModularID == Convert.ToInt32(data.SecondModularID)&&
                    //                a.VideoNumber == Convert.ToInt32(data.VideoNumber));
                    //}
                    //else
                    //{
                    //    record =
                    //        learningRecord.detail.FirstOrDefault(
                    //            a =>
                    //                a.BookID == Convert.ToInt32(data.BookId) &&
                    //                a.FirstTitleID == Convert.ToInt32(data.FirstTitleID) &&
                    //                a.SecondTitleID == Convert.ToInt32(data.SecondTitleID)
                    //                && a.FirstModularID == Convert.ToInt32(data.FirstModularID) &&
                    //                a.SecondModularID == Convert.ToInt32(data.SecondModularID)&&
                    //                a.VideoNumber == Convert.ToInt32(data.VideoNumber));
                    //}
                    int SecondTitleID = 0;
                    if (!string.IsNullOrEmpty(data.SecondTitleID)) 
                    {
                        SecondTitleID = Convert.ToInt32(data.SecondTitleID);
                    }
                   

                    var record =
                            learningRecord.detail.FirstOrDefault(
                                a =>
                                    a.BookID == Convert.ToInt32(data.BookId) &&
                                    a.FirstTitleID == Convert.ToInt32(data.FirstTitleID) &&
                                    a.SecondTitleID == SecondTitleID
                                    && a.FirstModularID == Convert.ToInt32(data.FirstModularID) &&
                                    a.SecondModularID == Convert.ToInt32(data.SecondModularID) &&
                                    a.VideoNumber == Convert.ToInt32(data.VideoNumber));
                    if (record != null)
                    {
                        record.DubbingNum += 1;
                        if (double.Parse(data.TotalScore) > record.BestScore)
                        {
                            //具体操作
                            record.BestScore = double.Parse(data.TotalScore);
                            record.CreateTime = data.CreateTime;
                            record.VideoNumber = data.VideoNumber.ToInt();
                            record.VideoID = data.VideoID;
                        }
                    }
                    else
                    {
                        learningRecord.detail.Add(new Rds_StudyReport_BookDetail()
                        {
                            //具体操作
                            BestScore = double.Parse(data.TotalScore),
                            CreateTime = data.CreateTime,
                            VideoNumber = data.VideoNumber.ToInt(),
                            BookID = data.BookId.ToInt(),
                            VideoID = data.VideoID,
                            FirstTitleID = data.FirstTitleID.ToInt(),
                            SecondTitleID = data.SecondTitleID.ToInt(),
                            FirstModularID = string.IsNullOrEmpty(data.FirstModularID) ? 0 : data.FirstModularID.ToInt(),
                            DubbingNum = 1,
                            SecondModularID = string.IsNullOrEmpty(data.SecondModularID) ? 0 : data.SecondModularID.ToInt()
                        });
                        var title = hashRedis.Get<Rds_StudyReport_BookCatalogues_BookID>("Rds_StudyReport_BookCatalogues_" + data.BookId,
                                                 data.FirstTitleID + "_" + data.SecondTitleID + "_" + ModuleType);
                        if (title != null)
                        {
                            title.FirstTitle = data.FirstTitle;
                            title.SecondTitle = data.SecondTitle;
                            title.FirstModular = data.FirstModular;

                            var video = title.Videos.FirstOrDefault(a => a.VideoNumber == data.VideoNumber.ToInt()&&a.SecondModularID==Convert.ToInt32(data.SecondModularID));
                            if (video != null)
                            {
                                video.VideoTitle = data.VideoTitle;
                                video.IsEnableOss = data.IsEnableOss;
                                video.VideoImageAddress = data.VideoImageAddress;
                                video.VideoNumber = data.VideoNumber.ToInt();
                                video.SecondModularID = string.IsNullOrEmpty(data.SecondModularID) ? 0 : data.SecondModularID.ToInt();
                            }
                            else
                            {
                                title.Videos.Add(new Rds_StudyReport_BookCatalogues_Video()
                                {
                                    VideoNumber = data.VideoNumber.ToInt(),
                                    VideoTitle = data.VideoTitle,
                                    VideoImageAddress = data.VideoImageAddress,
                                    IsEnableOss = data.IsEnableOss,
                                    SecondModularID = string.IsNullOrEmpty(data.SecondModularID) ? 0 : data.SecondModularID.ToInt()
                                });
                            }
                        }
                        else
                        {
                            title = new Rds_StudyReport_BookCatalogues_BookID();
                            title.FirstTitleID = data.FirstTitleID.ToInt();
                            title.FirstTitle = data.FirstTitle;
                            title.SecondTitleID = data.SecondTitleID.ToInt();
                            title.SecondTitle = data.SecondTitle;
                            title.FirstModularID = string.IsNullOrEmpty(data.FirstModularID) ? 0 : data.FirstModularID.ToInt();
                            title.FirstModular = data.FirstModular;
                            var video = title.Videos.FirstOrDefault(a => a.VideoNumber == data.VideoNumber.ToInt() && a.SecondModularID == Convert.ToInt32(data.SecondModularID));
                            if (video != null)
                            {
                                video.VideoTitle = data.VideoTitle;
                                video.IsEnableOss = data.IsEnableOss;
                                video.VideoImageAddress = data.VideoImageAddress;
                                video.SecondModularID = string.IsNullOrEmpty(data.SecondModularID) ? 0 : data.SecondModularID.ToInt();
                            }
                            else
                            {
                                title.Videos.Add(new Rds_StudyReport_BookCatalogues_Video()
                                {
                                    VideoNumber = data.VideoNumber.ToInt(),
                                    VideoTitle = data.VideoTitle,
                                    VideoImageAddress = data.VideoImageAddress,
                                    IsEnableOss = data.IsEnableOss,
                                    SecondModularID = string.IsNullOrEmpty(data.SecondModularID) ? 0 : data.SecondModularID.ToInt()
                                });
                            }
                        }
                        hashRedis.Set<Rds_StudyReport_BookCatalogues_BookID>("Rds_StudyReport_BookCatalogues_" + data.BookId,
                              data.FirstTitleID + "_" + data.SecondTitleID + "_" + ModuleType, title);
                    }
                }
                else
                {
                    learningRecord = new Rds_StudyReport_ModuleTitle();
                    learningRecord.UserID = data.UserId.ToInt();
                    learningRecord.detail.Add(new Rds_StudyReport_BookDetail()
                    {
                        //具体操作
                        BestScore = double.Parse(data.TotalScore),
                        CreateTime = data.CreateTime,
                        VideoNumber = data.VideoNumber.ToInt(),
                        BookID = data.BookId.ToInt(),
                        VideoID = data.VideoID,
                        FirstTitleID = data.FirstTitleID.ToInt(),
                        SecondTitleID = data.SecondTitleID.ToInt(),
                        FirstModularID = string.IsNullOrEmpty(data.FirstModularID) ? 0 : data.FirstModularID.ToInt(),
                        DubbingNum = 1,
                        SecondModularID = string.IsNullOrEmpty(data.SecondModularID) ? 0 : data.SecondModularID.ToInt()
                    });
                    var title = hashRedis.Get<Rds_StudyReport_BookCatalogues_BookID>("Rds_StudyReport_BookCatalogues_" + data.BookId,
                              data.FirstTitleID + "_" + data.SecondTitleID + "_" + ModuleType);
                    if (title != null)
                    {
                        title.FirstTitle = data.FirstTitle;
                        title.SecondTitle = data.SecondTitle;
                        title.FirstModular = data.FirstModular;
                        var video = title.Videos.FirstOrDefault(a => a.VideoNumber == data.VideoNumber.ToInt() && a.SecondModularID == Convert.ToInt32(data.SecondModularID));
                        if (video != null)
                        {
                            video.VideoTitle = data.VideoTitle;
                            video.IsEnableOss = data.IsEnableOss;
                            video.VideoImageAddress = data.VideoImageAddress;
                            video.SecondModularID = string.IsNullOrEmpty(data.SecondModularID) ? 0 : data.SecondModularID.ToInt();
                        }
                        else
                        {
                            title.Videos.Add(new Rds_StudyReport_BookCatalogues_Video()
                            {
                                VideoNumber = data.VideoNumber.ToInt(),
                                VideoTitle = data.VideoTitle,
                                VideoImageAddress = data.VideoImageAddress,
                                IsEnableOss = data.IsEnableOss,
                                SecondModularID = string.IsNullOrEmpty(data.SecondModularID) ? 0 : data.SecondModularID.ToInt()
                            });
                        }
                    }
                    else
                    {
                        title = new Rds_StudyReport_BookCatalogues_BookID();
                        title.FirstTitleID = data.FirstTitleID.ToInt();
                        title.FirstTitle = data.FirstTitle;
                        title.SecondTitleID = data.SecondTitleID.ToInt();
                        title.SecondTitle = data.SecondTitle;
                        title.FirstModularID = string.IsNullOrEmpty(data.FirstModularID) ? 0 : data.FirstModularID.ToInt();
                        title.FirstModular = data.FirstModular;
                        var video = title.Videos.FirstOrDefault(a => a.VideoNumber == data.VideoNumber.ToInt() && a.SecondModularID == Convert.ToInt32(data.SecondModularID));
                        if (video != null)
                        {
                            video.VideoTitle = data.VideoTitle;
                            video.IsEnableOss = data.IsEnableOss;
                            video.VideoImageAddress = data.VideoImageAddress;
                            video.SecondModularID = string.IsNullOrEmpty(data.SecondModularID) ? 0 : data.SecondModularID.ToInt();
                        }
                        else
                        {
                            title.Videos.Add(new Rds_StudyReport_BookCatalogues_Video()
                            {
                                VideoNumber = data.VideoNumber.ToInt(),
                                VideoTitle = data.VideoTitle,
                                VideoImageAddress = data.VideoImageAddress,
                                IsEnableOss = data.IsEnableOss,
                                SecondModularID = string.IsNullOrEmpty(data.SecondModularID) ? 0 : data.SecondModularID.ToInt()
                            });
                        }
                    }
                    hashRedis.Set<Rds_StudyReport_BookCatalogues_BookID>("Rds_StudyReport_BookCatalogues_" + data.BookId,
                          data.FirstTitleID + "_" + data.SecondTitleID + "_" + ModuleType, title);
                }
                hashRedis.Set<Rds_StudyReport_ModuleTitle>("Rds_StudyReport_ModuleTitle_" + data.UserId.Substring(0, 2),
                    data.UserId + "_" + (int)SubjectEnum.English + "_" + ModuleType, learningRecord);
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "CreateOrUpdateRds_StudyReport_ModuleTitleForHearResource!Data="+data.ToJson());
            }
        }


        /// <summary>
        /// 趣配音Module表
        /// </summary>
        /// <param name="data"></param>
        /// <param name="ModuleType"></param>
        private void CreateOrUpdateRds_StudyReport_ModuleForInterestDebbing(RedisVideoInfo data, int ModuleType)
        {
            Rds_StudyReport_Module module = null;
            if (!string.IsNullOrEmpty(data.SecondTitleID) && data.SecondTitleID != "0")
            {
                //更新或新增书本报告
                module = hashRedis.Get<Rds_StudyReport_Module>("Rds_StudyReport_Module",
                    data.ClassID + "_" + (int)SubjectEnum.English + "_" + ModuleType + "_" + data.BookId + "_" +
                    data.FirstTitleID + data.SecondTitleID);
                if (module != null)
                {
                    module.Flag = 0;
                }
                else
                {
                    module = new Rds_StudyReport_Module();
                    module.ClassID = data.ClassID;
                    module.SubjectID = (int)SubjectEnum.English;
                    module.ModuleType = ModuleType;
                    module.FirstTitleID = data.FirstTitleID.ToInt();
                    module.FirstTitle = data.FirstTitle;
                    module.SecondTitleID = data.SecondTitleID.ToInt();
                    module.SecondTitle = data.SecondTitle;
                    module.BookID = Convert.ToInt32(data.BookId);
                    module.Flag = 0;
                }
                hashRedis.Set<Rds_StudyReport_Module>("Rds_StudyReport_Module", data.ClassID + "_" + (int)SubjectEnum.English + "_" + ModuleType + "_" + data.BookId + "_" + data.FirstTitleID + data.SecondTitleID, module);
            }
            else
            {
                module = hashRedis.Get<Rds_StudyReport_Module>("Rds_StudyReport_Module",
                   data.ClassID + "_" + (int)SubjectEnum.English + "_" + ModuleType + "_" + data.BookId + "_" +
                   data.FirstTitleID);
                if (module != null)
                {
                    module.Flag = 0;
                }
                else
                {
                    module = new Rds_StudyReport_Module();
                    module.ClassID = data.ClassID;
                    module.SubjectID = (int)SubjectEnum.English;
                    module.ModuleType = ModuleType;
                    module.FirstTitleID = data.FirstTitleID.ToInt();
                    module.FirstTitle = data.FirstTitle;
                    module.SecondTitleID = data.SecondTitleID.ToInt();
                    module.SecondTitle = data.SecondTitle;
                    module.BookID = data.BookId.ToInt();
                    module.Flag = 0;
                }
                hashRedis.Set<Rds_StudyReport_Module>("Rds_StudyReport_Module", data.ClassID + "_" + (int)SubjectEnum.English + "_" + ModuleType + "_" + data.BookId + "_" + data.FirstTitleID, module);
            }
        }


        /// <summary>
        /// 趣配音书本报告记录
        /// </summary>
        /// <param name="data"></param>
        /// <param name="ModuleType"></param>
        private void CreateOrUpdateRds_StudyReport_Book(RedisVideoInfo data, int ModuleType)
        {
            //更新或新增书本报告
            var rdsBook = hashRedis.Get<Rds_StudyReport_Book>("Rds_StudyReport_Book", data.ClassID + "_" + (int)SubjectEnum.English + "_" + ModuleType + "_" + data.BookId);
            if (rdsBook != null)
            {
                rdsBook.Flag = 0;
            }
            else
            {
                rdsBook = new Rds_StudyReport_Book();
                rdsBook.ClassID = data.ClassID;
                rdsBook.SubjectID = (int)SubjectEnum.English;
                rdsBook.ModuleType = ModuleType;
                rdsBook.BookID = data.BookId.ToInt();
                rdsBook.Flag = 0;
            }
            hashRedis.Set<Rds_StudyReport_Book>("Rds_StudyReport_Book", data.ClassID + "_" + (int)SubjectEnum.English + "_" + ModuleType + "_" + data.BookId, rdsBook);
        }

        /// <summary>
        /// 趣配音班级记录新增
        /// </summary>
        /// <param name="data"></param>
        /// <param name="ModuleType"></param>
        private void CreateOrUpdateRds_StudyReport_Class(RedisVideoInfo data, int ModuleType)
        {

            //更新或新增班级报告
            var rdsClass = hashRedis.Get<Rds_StudyReport_Class>("Rds_StudyReport_Class", data.ClassID + "_" + (int)SubjectEnum.English + "_" + ModuleType);
            if (rdsClass != null)
            {
                rdsClass.Flag = 0;
            }
            else
            {
                rdsClass = new Rds_StudyReport_Class();
                rdsClass.ClassID = data.ClassID;
                rdsClass.SubjectID = (int)SubjectEnum.English;
                rdsClass.ModuleType = ModuleType;
                rdsClass.Flag = 0;
            }
            hashRedis.Set<Rds_StudyReport_Class>("Rds_StudyReport_Class", data.ClassID + "_" + (int)SubjectEnum.English + "_" + ModuleType, rdsClass);
        }

        #endregion
    }
}

